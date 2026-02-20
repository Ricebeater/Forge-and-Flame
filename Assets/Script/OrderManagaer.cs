using TMPro;
using UnityEngine;

public class OrderManagaer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private PlayerInteractor player;
    [SerializeField] private int basePrice = 50;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI customerText;

    private int currentMoney = 0;
    private bool hasActiveOrder = false;

    private void Start()
    {
        GenerateNewOrder();
        if (hasActiveOrder) { Debug.Log(""); }
    }

    public void GenerateNewOrder()
    {
        hasActiveOrder = true;
        customerText.text = "Customer: I need a sharp sword! (Status: Waiting)";
    }

    public void SpawnRawMaterial()
    {
        if (player.currentHeldItem == null)
        {
            GameObject newSwordObj = Instantiate(swordPrefab);
            SwordItem newSword = newSwordObj.GetComponent<SwordItem>();
            player.PickUpSword(newSword);
            newSword.UpdateVisuals();
        }
    }

    public void SubmitSword()
    {
        if (player.currentHeldItem != null)
        {
            SwordItem sword = player.currentHeldItem;

            if (sword.state.progressStage == 3)
            {
                float finalQuality = sword.state.GetFinalizeScore();
                int payout = CalculatePayout(finalQuality);

                currentMoney += payout;
                moneyText.text = "Money: $" + currentMoney;
                customerText.text = $"Customer: Amazing! Quality: {finalQuality:F0}%";

                player.RemoveSword();
                hasActiveOrder = false;

                Invoke(nameof(GenerateNewOrder), 3f);
            }
            else
            {
                customerText.text = "Customer: This isn't finished yet!";
            }
        }
    }

    private int CalculatePayout(float quality)
    {
        return basePrice + Mathf.RoundToInt(quality * 2);
    }
}
