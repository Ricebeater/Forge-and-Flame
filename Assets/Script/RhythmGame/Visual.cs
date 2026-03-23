using UnityEngine;
using UnityEngine.InputSystem;

public class Visual : MonoBehaviour
{
    [SerializeField] GameObject[] highlightLane;

    private void Update()
    {
        for (int lane = 0; lane < highlightLane.Length; lane++)
        {
            if (Input.GetKey(HitManager.Instance.lanesKey[lane]))
            {
                highlightLane[lane].SetActive(true);
            }
            else
            {
                highlightLane[lane].SetActive(false);
            }
        }
        
    }
}
