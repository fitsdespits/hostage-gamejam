using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRoom : Room
{
    [Header("References")]
    public ConferenceRoom conferenceRoom;  // assign in Inspector
    public ExchangeRoom exchangeRoom;      // assign in Inspector

    private bool dealInProgress = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Pawn>(out var pawn))
            return;

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

        if (conferenceRoom == null || exchangeRoom == null)
        {
            Debug.LogWarning("ConferenceRoom or ExchangeRoom not assigned.");
            dealInProgress = false;
            yield break;
        }

        // Get wants and gives from ConferenceRoom
        List<Pawn> wants = conferenceRoom.GetWants();
        List<Pawn> gives = conferenceRoom.GetGives();

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
}