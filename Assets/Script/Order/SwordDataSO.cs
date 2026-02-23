using UnityEngine;

[CreateAssetMenu(fileName = "newSwordData", menuName = "Swords/Sword Data")]
public class SwordDataSO : ScriptableObject
{
    public SwordType swordType;
    public string swordName;
    public Sprite icon;
    public GameObject prefab;
}
