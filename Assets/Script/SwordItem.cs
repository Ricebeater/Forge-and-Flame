using UnityEngine;

[System.Serializable]
public class SwordState
{
    public int progressStage = 0; // 0-3

    public float smeltingScore;
    public float forgingScore;
    public float quenchingScore;

    public float GetFinalizeScore()
    {
        return (smeltingScore + forgingScore + quenchingScore) / 3f;
    }
}

public class SwordItem : MonoBehaviour
{
    public SwordState state = new SwordState();

    [SerializeField] private GameObject oreVisual;
    [SerializeField] private GameObject IngotVisual;
    [SerializeField] private GameObject BladeVisual;
    [SerializeField] private GameObject FinalSwordVisual;

    public void UpdateVisuals()
    {
        if(oreVisual) oreVisual.SetActive(state.progressStage == 0);
        if(IngotVisual) IngotVisual.SetActive(state.progressStage == 1);
        if(BladeVisual) BladeVisual.SetActive(state.progressStage == 2);
        if(FinalSwordVisual) FinalSwordVisual.SetActive(state.progressStage == 3);
    }
}
