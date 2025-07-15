using UnityEngine;

public class VaultRoom : Room
{
    public int MoneyperPawnGain = 100;
    public float GainInterval = 3f;
    private float timer = 0f;

    private void Start()
    {
        roomName = "Vault";
    }

    public void Update()
    {
        if (pawnsInRoom.Count == 0)
            return;  // no pawns, nothing to do

        timer += Time.deltaTime;

        if (timer >= GainInterval)
        {
            timer = 0f;

            float moneyThisTick = 0f;

            foreach (Pawn pawn in pawnsInRoom)
            {
                moneyThisTick += MoneyperPawnGain;
                Inventory.Instance.AddMoney(MoneyperPawnGain);
            }
         
        }
    }
}

