using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public TMPro.TextMeshProUGUI moneyText;

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
    }

    public int totalMoney = 0;

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        Debug.Log($" Total money: {totalMoney}");
        if (moneyText != null)
        {
            moneyText.text = $"${totalMoney}";
        }
    }
}
