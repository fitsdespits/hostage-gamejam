using UnityEngine;
using UnityEngine.UI;

public class EscalationManager : MonoBehaviour
{
    public static EscalationManager Instance { get; private set; }

    [Header("Escalation Settings")]
    public float currentEscalation = 0f;
    public float maxEscalation = 100f;
    public float minEscalation = 0f;

    [Header("Optional UI")]
    public Image escalationSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private float delayTimer = 0f;

    private void Update()
    {
        delayTimer += Time.deltaTime;

        if (delayTimer >= 30f) // every 30 seconds of inactivity
        {
            EscalationManager.Instance.IncreaseEscalation(3f);
            delayTimer = 0f;
            
        }
    }

    public void IncreaseEscalation(float amount)
    {
        currentEscalation = Mathf.Clamp(currentEscalation + amount, minEscalation, maxEscalation);
        UpdateUI();
        Debug.Log($"Escalation increased: {currentEscalation}/{maxEscalation}");
    }

    public void DecreaseEscalation(float amount)
    {
        currentEscalation = Mathf.Clamp(currentEscalation - amount, minEscalation, maxEscalation);
        UpdateUI();
        Debug.Log($"Escalation decreased: {currentEscalation}/{maxEscalation}");
    }

    private void UpdateUI()
    {
        if (escalationSlider != null)
        {
            escalationSlider.fillAmount = currentEscalation / maxEscalation;
        }
    }
}