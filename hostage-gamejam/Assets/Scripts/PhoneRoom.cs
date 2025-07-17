using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRoom : Room
{

    [Header("References")]
    public ConverenceRoom converenceRoom;  // assign in Inspector
    public ExchangeRoom exchangeRoom;      // assign in Inspector

    private bool dealInProgress = false;

    public override void OnPawnEnter(Pawn pawn)
    {

        if (dealInProgress)
        {
            Debug.Log("Deal already in progress. Please wait.");
            return;
        }

        StartCoroutine(HandleDeal());
    }


    private IEnumerator HandleDeal()
    {
        dealInProgress = true;

        Debug.Log("Calling the police...");
        yield return new WaitForSeconds(1f); // 1 second delay for visuals

        checkDeal();

        if (converenceRoom == null || exchangeRoom == null)
        {
            Debug.LogWarning("ConferenceRoom or ExchangeRoom not assigned.");
            dealInProgress = false;
            yield break;
        }

        // Get wants and gives from ConferenceRoom
        List<Pawn> wants = converenceRoom.GetWants();
        List<Pawn> gives = converenceRoom.GetGives();

        // Grant the rewards (the "wants")
        foreach (var pawn in wants)
        {
            if (pawn.pawnType == PawnType.Robber)
            {
                
                switch (pawn.robberType)
                {
                   
                    case RobberType.DefaultRobber:
                        Inventory.Instance.AddMoney(2000);
                        Debug.Log("Granted: +2000 money.");
                        break;

                    case RobberType.GasRobber:
                        Inventory.Instance.AddGas(20);
                        Debug.Log("Granted: +20 gas.");
                        break;
                }
            }
        }

        // Count the promised hostages (the "gives")
        int promisedHostages = 0;
        foreach (var pawn in gives)
        {
            if (pawn.pawnType == PawnType.Hostage)
                promisedHostages++;
        }

        Debug.Log($"Police accepted your demands. You promised to give {promisedHostages} hostages.");

        // Start the ExchangeRoom phase
        exchangeRoom.StartExchange(promisedHostages);

        dealInProgress = false;
    }

    public void checkDeal()
    {
        if (converenceRoom.pawnsOnLeft.Count > converenceRoom.pawnsOnRight.Count)
        {
            EscalationManager.Instance.IncreaseEscalation(5f);
            //unfair deal on amount asked and given
        }
        if (converenceRoom.pawnsOnLeft.Count == 0 && converenceRoom.pawnsOnRight.Count <= 1 )
        {
            EscalationManager.Instance.DecreaseEscalation(5f);
            //ask nothing, give something
        }
 

    }
}