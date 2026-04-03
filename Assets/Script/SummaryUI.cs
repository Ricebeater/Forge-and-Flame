using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SummaryUI : MonoBehaviour
{
    public static SummaryUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Day Info")]
    [SerializeField] private TextMeshProUGUI dayText;

    [Header("Sword Data")]
    [SerializeField] private TextMeshProUGUI swordNameText;
    [SerializeField] private Image swordIcon;

    [Header("Ranking Text")]
    [SerializeField] private TextMeshProUGUI smeltRankText;
    [SerializeField] private TextMeshProUGUI forgeRankText;
    [SerializeField] private TextMeshProUGUI quenchRankText;
    [SerializeField] private TextMeshProUGUI overallRankText;

    public event System.Action OnContinue;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
    }

    private void Update()
    {
        GetSwordIcon();
    }

    public void Show(float smelt, float forge, float quench)
    {
        float final = (smelt + forge + quench) / 3f;
        smeltRankText.text      = $"{GetRank(smelt)}";
        forgeRankText.text      = $"{GetRank(forge)}";
        quenchRankText.text     = $"{GetRank(quench)}";
        overallRankText.text    = $"{GetRank(final)}";

        panel.SetActive(true);

        StartCoroutine(EnableInputNextFrame());
    }

    private IEnumerator EnableInputNextFrame()
    {
        yield return null;
    }

    public void DismissSummary()
    {
        panel.SetActive(false);
        Debug.Log("Dismiss Pressed");
        OnContinue?.Invoke();
    }

    private string GetRank(float score)
    {
        if (score >= 100f) return "S";
        if (score >= 90f) return "A";
        if (score >= 75f) return "B";
        if (score >= 50f) return "C";
        return "F";
    }

    private void GetSwordIcon()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + DayManager.Instance.dayNumber.ToString();
        }

        SwordDataSO sword = OrderManager.Instance.GetCurrentOrder()?.requestedSword;
        if (sword != null)
        {
            swordNameText.text = sword.name;
            swordIcon.sprite = sword.icon;

        }
    }
}
