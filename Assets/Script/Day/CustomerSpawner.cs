using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerNPC[] customers;
    public CustomerNPC currentCustomer {  get; private set; }

    private void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        int dayIndex = DayManager.Instance.dayNumber - 1;

        for (int i = 0; i < customers.Length; i++)
        {
            if (customers[i] != null)
            {
                customers[i].gameObject.SetActive(i == dayIndex);
            }
        }
        if (dayIndex < customers.Length && customers[dayIndex] != null)
        { 
            currentCustomer = customers[dayIndex]; 
        }
    }
}
