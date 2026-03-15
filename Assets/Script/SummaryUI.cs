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

    public void Show(float smelt, float forge, float quench)
    {
        SwordDataSO sword = OrderManager.Instance.GetCurrentOrder()?.requestedSword;
        if (sword != null)
        {
            swordNameText.text = sword.name;
            swordIcon.sprite = sword.icon;
        }

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
        if (score >= 60f) return "C";
        if (score >= 40f) return "D";
        return "F";
    }
}
