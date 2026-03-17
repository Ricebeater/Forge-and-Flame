using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public int dayNumber = 1;

    [Header("Event")]
    public UnityEvent OnDayStart;
    public UnityEvent OnDayEnd;

    public DaySO[] days;
    public DaySO currentDay => days != null && dayNumber - 1 < days.Length 
        ? days[dayNumber - 1] 
        : null;
    
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
        StartDay();
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
        StartDay();
    }

    public bool IsFirstDay()
    {
        if (dayNumber == 1) { return true; }
        else { return false; }
    }
}
