using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public int dayNumber = 1;
    public int customersPerDay = 5;

    public UnityEvent OnDayStart;
    public UnityEvent OnDayEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDay()
    {
        Debug.Log($"Day {dayNumber} start");
        OnDayStart?.Invoke();
    }

    public void EndDay()
    {
        Debug.Log("Day Ended.");
        OnDayEnd?.Invoke();

        dayNumber++;
    }
}
