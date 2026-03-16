using UnityEngine;

public class HeatController : MonoBehaviour
{
    private Material metalMat;

    [Header("Heat Parameters")]
    public float heatLevel = 0f;
    public float heatSpeed = 0.2f;
    public float coolSpeed = 0.1f;

    private bool isInsideFire = false;

    void Start()
    {
        metalMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (isInsideFire)
        {
            heatLevel = Mathf.MoveTowards(heatLevel, 1f, heatSpeed * Time.deltaTime);
        }
        else
        {
            heatLevel = Mathf.MoveTowards(heatLevel, 0f, coolSpeed * Time.deltaTime);
        }

        if (metalMat != null)
        {
            metalMat.SetFloat("_Heat_Amount", heatLevel);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HeatZone"))
        {
            isInsideFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HeatZone"))
        {
            isInsideFire = false;
        }
    }
}