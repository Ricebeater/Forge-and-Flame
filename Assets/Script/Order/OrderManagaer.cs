using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManagaer : MonoBehaviour
{
    public static OrderManagaer Instance { get; private set; }

    [SerializeField] private SwordOrderSO order;
    [SerializeField] private Transform playerHand;

    private GameObject currentSword;

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

    private void Start()
    {
        Debug.Log($"Order Manager: Current Order is {order.requestedSword.swordName}");
    }

    public void SubmitCompleteSword()
    {
        if(order == null || order.requestedSword == null)
        {
            Debug.LogWarning("Order Manager: No order or requested sword assigned.");
            return;
        }

        
    }

    private void SpawnSwordInHand(SwordDataSO swordData)
    {
        if(swordData.prefab == null)
        {
            Debug.LogWarning($"Order Manager: No prefab assigned for {swordData.swordName}");
            return;
        }

        if(currentSword != null)
        {
            Destroy(currentSword);
        }

        currentSword = Instantiate(swordData.prefab, playerHand);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;

        Debug.Log($"{swordData.swordName} spawned in player's hand.");
    }

    public SwordOrderSO GetCurrentOrder() => order;
}
