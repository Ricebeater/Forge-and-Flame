using UnityEngine;

public class BellowAnim : MonoBehaviour
{
    [SerializeField] Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!OrderManager.Instance.IsCurrentStep(CraftingStep.Smelting)) { anim.Play("Rest", 0, 0f); return; }
        if (Input.GetMouseButtonDown(0) && TutorialUI.Instance.isTutorialShow == false)
        {
            anim.Play("Blow", 0, 0f);
        }

    }
}
