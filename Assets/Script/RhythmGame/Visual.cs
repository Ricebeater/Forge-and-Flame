using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Visual : MonoBehaviour
{
    [SerializeField] GameObject[] highlightLane;
    [SerializeField] Image progressBar;
    [SerializeField] GameObject niceTextPopUp;
    [SerializeField] Transform firstLane;

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

            if (Input.GetKeyDown(HitManager.Instance.lanesKey[lane]))
            {
                GameObject nicePopUp = Instantiate(niceTextPopUp, firstLane, false);
            }
        }

        progressBar.fillAmount = Conductor.Instance.songPosition / Conductor.Instance.musicSource.clip.length;
    }
}
