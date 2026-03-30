using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Visual : MonoBehaviour
{
    [SerializeField] GameObject[] highlightLane;
    [SerializeField] Image progressBar;
    [SerializeField] GameObject niceTextPopUp;
    [SerializeField] GameObject greatTextPopUp;
    [SerializeField] GameObject perfectTextPopUp;
    [SerializeField] GameObject missTextPopUp;
    [SerializeField] Transform laneOne;
    [SerializeField] Transform laneTwo;
    [SerializeField] Transform laneThree;

    private void Start()
    {
        if (HitManager.Instance == null)
        {
            Debug.LogWarning("HitManager instance not found!");
            return;
        }

        HitManager.Instance.OnNoteHit += ShowPopUp;
    }
    private void OnEnable()
    {
        if (HitManager.Instance == null)
        {
            Debug.LogWarning("HitManager instance not found!");
            return;
        }
        HitManager.Instance.OnNoteHit += ShowPopUp;
    }
    private void OnDisable() => HitManager.Instance.OnNoteHit -= ShowPopUp;

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

        progressBar.fillAmount = Conductor.Instance.songPosition / Conductor.Instance.musicSource.clip.length;
    }
    private void ShowPopUp(Judgement judgement, int lane)
    {
        Transform spawnPoint = lane switch
        {
            0 => laneOne,
            1 => laneTwo,
            2 => laneThree,
            _ => laneOne
        };

        GameObject popUpPrefab = judgement switch
        {
            Judgement.Perfect => perfectTextPopUp,
            Judgement.Great   => greatTextPopUp,
            Judgement.Nice    => niceTextPopUp,
            Judgement.Miss    => missTextPopUp,
            _ => niceTextPopUp
        };

        GameObject popUp = Instantiate(popUpPrefab, spawnPoint, false);

    }
}
