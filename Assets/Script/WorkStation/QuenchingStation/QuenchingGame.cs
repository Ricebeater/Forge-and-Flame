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

    [Header("For Game")]
    public bool isMiniGameActive = false;
    [SerializeField]private bool isMiniGameFinnised = false;
    
    //scoring
    [SerializeField]private int currentRound = 0;
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

    //fix tutorial proceed button on relaese bug when closing tutorial
    [Header("Debug")]
    [SerializeField] private bool firstClick;

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
        if (TutorialUI.Instance.isTutorialShow) { return; }

        if (isMiniGameFinnised) { return; }

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
            if (firstClick)
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
            else
            {
                firstClick = true;
            }
        }

       
    }

    public void StartMinigame()
    {
        if (TutorialUI.Instance.isTutorialShow) { return; }

        //adjust charge needed range base on what day it is
        int rangeByDay = (20 - (DayManager.Instance.dayNumber) * 5);
        Debug.Log("random range : " + rangeByDay);
        maxChargeNeed = Random.Range(20, 90);
        minChargeNeed = maxChargeNeed - rangeByDay;

        if (DayManager.Instance.dayNumber == 1)
        {
            firstClick = false;
        }
        else
        {
            firstClick = true;
        }
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
        
        if (DayManager.Instance.dayNumber == 1)
        {
            firstClick = false;
        }
        else
        {
            firstClick = true;
        }

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
        int dayNumber = DayManager.Instance.dayNumber;
        if  (!isMaxedOut)    { currentCharge += (chargeRate + (currentCharge * 2 * dayNumber)) * Time.deltaTime; }
        if  (isMaxedOut)     { currentCharge -= (chargeRate + (currentCharge * 2 * dayNumber)) * Time.deltaTime; }
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
