using UnityEngine;

[CreateAssetMenu(fileName = "DaySO", menuName = "Day/Day Data")]
public class DaySO : ScriptableObject
{
    public int dayNumber;
    public NPCDataSO customer;
    public string songfileName;
}
