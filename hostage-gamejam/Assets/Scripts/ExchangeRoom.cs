using UnityEngine;

public class ExchangeRoom : Room
{
    private bool exchangeActive = false;
    private float timer = 0f;

    private int hostagesPromised = 0;
    private int hostagesDelivered = 0;

    public float exchangeTime = 5f;

    public void StartExchange(int promisedHostages)
    {
        hostagesPromised = promisedHostages;

        if (hostagesPromised <= 0)
        {
            Debug.Log("No hostages were promised. Nothing to deliver.");
            return;
        }

        exchangeActive = true;
        timer = exchangeTime;
        hostagesDelivered = 0;

        Debug.Log($"Exchange started: you have {exchangeTime} seconds to deliver {hostagesPromised} hostages (if you choose).");
    }

    private void Update()
    {
        if (!exchangeActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EvaluateExchange();
        }
    }
    public override void OnPawnEnter(Pawn pawn)
    {

        if (!exchangeActive)
            return;

        if (pawn.pawnType != PawnType.Hostage)
            return;

        hostagesDelivered++;

        Debug.Log($"Hostage delivered: {hostagesDelivered}/{hostagesPromised}");
    }

    private void EvaluateExchange()
    {
        exchangeActive = false;

        if (hostagesDelivered >= hostagesPromised)
        {
            Debug.Log("Deal honored: you delivered what you promised.");
            EscalationManager.Instance.DecreaseEscalation(20f);
        }
        else
        {
            Debug.Log($"Deal broken: you only delivered {hostagesDelivered}/{hostagesPromised} hostages.");
            EscalationManager.Instance.IncreaseEscalation(20f);
        }

        if(hostagesDelivered > hostagesPromised)
        {
            Debug.Log("youre too kind!");
            EscalationManager.Instance.DecreaseEscalation(40f);
        }

        // Reset counts
        hostagesPromised = 0;
        hostagesDelivered = 0;
    }
}