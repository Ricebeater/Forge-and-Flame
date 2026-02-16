using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int targetFPS;
    [SerializeField] private OrderManagaer orderManager;

    private void Awake()
    {
        orderManager = gameObject.GetComponent<OrderManagaer>();
        Application.targetFrameRate = targetFPS;
    }

    private void Start()
    {
        orderManager.SpawnRawMaterial();
    }
}
