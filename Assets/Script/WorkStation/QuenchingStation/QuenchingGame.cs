using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuenchingGame : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private float currentCharge = 0f;
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float chargeRate = 1f;
    [SerializeField] private float minChargeNeed;
    [SerializeField] private float maxChargeNeed;
    [SerializeField]private bool isMinigameActive = false;

    [Header("Checker")]
    [SerializeField] private bool isMaxedOut = false;
    public float currentChargeRate;

    private bool isCharging = false;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI chargeText;
    [SerializeField] private Image chargeBar;
    [SerializeField] private GameObject miniGameUI;
    [SerializeField] private Image chargeNeedBar;


    private void Start()
    {
        currentCharge = 0f;
        chargeBar.fillAmount = 0f;
        miniGameUI.SetActive(false);
    }

    private void Update()
    {
        HandleUI(isMinigameActive);
        
        if (!isMinigameActive) { return; }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isCharging = true;
            currentCharge = 0f;
        }

        if (isCharging)
        {
            if (currentCharge >= maxCharge)
            {
                isMaxedOut = true;
            }
            else if (currentCharge <= 0f)
            {
                isMaxedOut = false;
            }
            DisplayCurrentChargeRate();
            Charging();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopCharging();
        }

       
    }

    public void StartMinigame()
    {
        isMinigameActive = true;
        currentCharge = 0f;
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        currentCharge = 0f;
    }

    private void StopCharging()
    {
        isCharging = false;
        if (currentCharge < minChargeNeed) { Debug.Log("To Fast!"); }
        if (currentCharge >= minChargeNeed && currentCharge <= maxChargeNeed) { Debug.Log("Just right!"); }
        if (currentCharge > maxChargeNeed) { Debug.Log("To Slow"); }
    }

   

    private void DisplayCurrentChargeRate()
    {
        if (!isMaxedOut) { currentChargeRate = (chargeRate * currentCharge) * Time.deltaTime; }
        if (isMaxedOut) { currentChargeRate = (chargeRate * (maxCharge - currentCharge)) * Time.deltaTime; }
    }

    private void Charging()
    {
        if (!isMaxedOut) { currentCharge += (chargeRate + (currentCharge * 2)) * Time.deltaTime; }
        if (isMaxedOut) { currentCharge -= (chargeRate + (currentCharge * 2)) * Time.deltaTime; }
    }

    private void HandleUI(bool isMinigameActive)
    {
        if (isMinigameActive)
        {
            miniGameUI.SetActive(true);
            chargeText.enabled = true;
            chargeBar.enabled = true;

            float chargePercent = currentCharge / maxCharge;
            float chargeNeedRange = (maxChargeNeed - minChargeNeed) / maxCharge;

            float minRatio = minChargeNeed / maxCharge;
            float maxRatio = maxChargeNeed / maxCharge;
            float rangeRatio = maxRatio - minRatio;

            chargeBar.fillAmount = chargePercent;
            chargeText.text = "Charge: " + Mathf.RoundToInt(currentCharge).ToString();
            chargeBar.rectTransform.sizeDelta = new Vector2(75f, 1000f * (chargePercent));

            chargeNeedBar.rectTransform.sizeDelta = new Vector2(
                100f,
                rangeRatio * 1000f
            );
            chargeNeedBar.rectTransform.anchoredPosition = new Vector2(
                0f,
                minRatio * 1000f
                );
        }
        else if (!isMinigameActive)
        {
            miniGameUI.SetActive(false);
            chargeText.enabled = false;
            chargeBar.enabled = false;
        }
    }

}
