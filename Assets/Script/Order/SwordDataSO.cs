using UnityEngine;

[CreateAssetMenu(fileName = "newSwordData", menuName = "Swords/Sword Data")]
public class SwordDataSO : ScriptableObject
{
    public SwordType swordType;
    public string swordName;
    [TextArea] public string description;
    public Sprite icon;
    public GameObject prefab;
}
