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
    [SerializeField] private int roundRequire = 3;
    public bool isMiniGameActive = false;
    private bool isMiniGameFinnised = false;
    
    //scoring
    private int currentRound = 0;
    private float[] roundScore;

    [Header("Debug")]
    [SerializeField] private bool isMaxedOut = false;
    public float currentChargeRate;

    private bool isCharging = false;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI chargeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image chargeBar;
    [SerializeField] private GameObject miniGameUI;
    [SerializeField] private Image chargeNeedBar;


    private void Start()
    {
        roundScore = new float[roundRequire];

        currentCharge = 0f;
        currentRound = 0;
        chargeBar.fillAmount = 0f;
        miniGameUI.SetActive(false);
    }

    private void Update()
    {
        HandleUI(isMiniGameActive);
        
        if (!isMiniGameActive) { return; }

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

            currentRound++;

             if (currentRound >= roundRequire)
             {
                isMiniGameFinnised = true;
                CalculatedScore();
                currentRound = 0;
            }
        }

       
    }

    public void StartMinigame()
    {
        roundScore = new float[roundRequire];
        isMiniGameActive = true;
        isMiniGameFinnised = false;
        currentCharge = 0f;
        currentRound = 0;
    }

    public void EndMinigame()
    {
        isMiniGameFinnised= false;
        isMiniGameActive = false;
        currentCharge = 0f;
        currentRound = 0;

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }

    }

    private void StopCharging()
    {
        isCharging = false;

        if (currentRound < roundScore.Length)
        {
            if (currentCharge < minChargeNeed) 
            { 
                Debug.Log("To Fast!");
                roundScore[currentRound] = Mathf.Abs(currentCharge - minChargeNeed);
            }
            if (currentCharge >= minChargeNeed && currentCharge <= maxChargeNeed) 
            { 
                Debug.Log("Just right!"); 
                roundScore[currentRound] = 0f;
            }
            if (currentCharge > maxChargeNeed) 
            { 
                Debug.Log("To Slow");
                roundScore[currentRound] = Mathf.Abs(currentCharge - maxChargeNeed);
            }
        }

    }

    private void DisplayCurrentChargeRate()
    {
        if  (!isMaxedOut)    { currentChargeRate = (chargeRate * currentCharge) * Time.deltaTime; }
        if  (isMaxedOut)     { currentChargeRate = (chargeRate * (maxCharge - currentCharge)) * Time.deltaTime; }
    }

    private void Charging()
    {
        if  (!isMaxedOut)    { currentCharge += (chargeRate + (currentCharge * 2)) * Time.deltaTime; }
        if  (isMaxedOut)     { currentCharge -= (chargeRate + (currentCharge * 2)) * Time.deltaTime; }
    }

    public float CalculatedScore()
    {
        float totalScore = 0f;
        foreach (float score in roundScore)
        {
            totalScore += score;
        }

        float finalScore = Mathf.Clamp(100f - totalScore, 0f, 100f);

        return finalScore;
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

        if (scoreText != null && isMiniGameFinnised)
        {
            scoreText.text = "Score: " + CalculatedScore().ToString("F1");
            scoreText.gameObject.SetActive(true);
        }

    }

}
