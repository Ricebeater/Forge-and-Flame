using System;
using UnityEngine;

public class HammerAnim : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject sparkPrefab;
    [SerializeField] Transform sparkPos;

    private bool isHitting = false;

    void Start()
    {
        anim = GetComponent<Animator>();        
        isHitting = false;
    }

    void Update()
    {
        if (!OrderManager.Instance.IsCurrentStep(CraftingStep.Forging)) { anim.Play("Rest", 0, 0f); return; }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            anim.Play("Hitting", 0, 0f);
            Instantiate(sparkPrefab, sparkPos);
        }

    }

}
