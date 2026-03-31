using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public PlayableDirector director;
    public GameObject cutsceneCamera;
    public GameObject playerPOV;
    private Vector3 oriCamPos;

    [Header("Player Settings")]
    public GameObject playerController;

    private void Start()
    {
        oriCamPos = playerPOV.transform.position;
    }

    void OnEnable()
    {
        if (director != null)
        {
            director.stopped += OnCutsceneFinished;
        }
    }

    void OnDisable()
    {
        if (director != null)
        {
            director.stopped -= OnCutsceneFinished;
        }
    }

    void OnCutsceneFinished(PlayableDirector obj)
    {
        if (cutsceneCamera != null) cutsceneCamera.SetActive(false);


        if (playerController != null) playerController.SetActive(true);


        Debug.Log("cutsvene end");

        cutsceneCamera.transform.position = oriCamPos; 

        gameObject.SetActive(false);

    }
}