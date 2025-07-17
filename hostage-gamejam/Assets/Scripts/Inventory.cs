using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public TMPro.TextMeshProUGUI moneyText;
    public int totalMoney = 0;

    public TMPro.TextMeshProUGUI robbersText;
    public TMPro.TextMeshProUGUI hostagesText;
    public int robberCount = 0;
    public int hostageCount = 0;

    public TMPro.TextMeshProUGUI GasText;
    public int gas = 50;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  
        }
        UpdateGasUI();
    }
  
    private void UpdateUI()
    {
        if (robbersText != null)
            robbersText.text = $"{robberCount}";
        if (hostagesText != null)
            hostagesText.text = $"{hostageCount}";
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        Debug.Log($"{totalMoney}");
        if (moneyText != null)
        {
            moneyText.text = $"{totalMoney}";
        }
    }
    public void AddGas(int amount)
    {
        gas += amount;
        UpdateGasUI();
    }
    public void SpendGas(int amount)
    {
        gas -= amount;
        if (gas < 0) gas = 0;
        UpdateGasUI();
    }

    private void UpdateGasUI()
    {
        if (GasText != null)
            GasText.text = $"{gas}";
       
    }

    public void AddPawn(Pawn pawn)
    {
        if (pawn.pawnType == PawnType.Robber)
        {
            robberCount++;
        }
        else if (pawn.pawnType == PawnType.Hostage)
        {
            hostageCount++;
        }
        UpdateUI();
    }

    public void RemovePawn(Pawn pawn)
    {
        if (pawn.pawnType == PawnType.Robber)
        {
            robberCount--;
        }
        else if (pawn.pawnType == PawnType.Hostage)
        {
            hostageCount--;
        }
        UpdateUI();
    }
}
