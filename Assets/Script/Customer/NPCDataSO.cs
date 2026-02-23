using UnityEngine;

[CreateAssetMenu(fileName = "newNPCData", menuName = "NPCs/NPC Data")]
public class NPCDataSO : ScriptableObject
{
    [Header("Identity")]
    public string npcName;
    public Sprite portrait;

    [Header("Dialogue")]
    [TextArea] public string orderDialogue;      
    [TextArea] public string waitingDialogue;    
    [TextArea] public string deliveryDialogue;
    
    [Header("Order")]
    public SwordOrderSO swordOrder;
}
