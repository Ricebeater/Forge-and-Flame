using UnityEngine;

[CreateAssetMenu(fileName = "NewSwordOrder", menuName = "Swords/Sword Order")]
public class SwordOrderSO : ScriptableObject
{
    public string customerName;
    public SwordDataSO requestedSword;
}
