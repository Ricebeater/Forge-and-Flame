using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SmeltingGame : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private float requiredHeat = 100f;
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private float heatIncreaseRate = 10f;
    [SerializeField] private float heatDecreaseRate = 1f;
    private bool isMinigameActive = false;

    [Header("Debug")]
    public bool isMiniGameStarted;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI heatText;
    [SerializeField] private Image HeatBar;
    [SerializeField] private GameObject smeltingUI;


    public void StartMinigame()
    {
        isMinigameActive = true;
        Debug.Log("Smelting minigame started.");
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        Debug.Log("Smelting minigame ended.");
        currentHeat = 0f;
    }

    private void Update()
    {
        isMiniGameStarted = isMinigameActive;
        currentHeat = Mathf.Clamp(currentHeat, 0f, requiredHeat);
        HandleUI();

        if (!isMinigameActive) { return; }

        if (currentHeat > 0f && currentHeat < 100) { currentHeat -= heatDecreaseRate * Time.deltaTime; }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            currentHeat += heatIncreaseRate;
        }

        
    }

    private void HandleUI()
    {
        smeltingUI.SetActive(isMinigameActive);

        int heatDisplay = Mathf.FloorToInt(currentHeat);
        heatText.text = "Heat: " + heatDisplay;

        float heatPercent = currentHeat / requiredHeat;
        HeatBar.fillAmount = heatPercent;
    }
}
