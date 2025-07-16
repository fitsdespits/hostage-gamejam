using System.Collections.Generic;
using UnityEngine;

public class CarRoom : Room
{
    public int fillIntervalSeconds = 3;      // How often to fill (seconds)
    public int gasPerPawnPerTick = 10;       // Gas units filled per pawn per tick

    public int currentGasInTank = 0;         // Current gas in tank

    private float timer = 0f;

    private void Start()
    {
        roomName = "Car Room";
    }

    private void Update()
    {
        if (pawnsInRoom.Count == 0)
            return;  // No pawns, no filling

        if (Inventory.Instance.gas <= 0)
            return;  // No gas in inventory

        timer += Time.deltaTime;

        if (timer >= fillIntervalSeconds)
        {
            timer = 0f;

            int gasAvailable = Inventory.Instance.gas;
            int totalGasFilledThisTick = 0;

            foreach (Pawn pawn in pawnsInRoom)
            {
                if (gasAvailable <= 0)
                    break;

                int fillAmount = Mathf.Min(gasPerPawnPerTick, gasAvailable);

                currentGasInTank += fillAmount;
                gasAvailable -= fillAmount;
                totalGasFilledThisTick += fillAmount;
            }

            Inventory.Instance.SpendGas(totalGasFilledThisTick);

            Debug.Log($"Filled tank +{totalGasFilledThisTick}, Tank now: {currentGasInTank}, Gas left: {Inventory.Instance.gas}");
        }
    }
}