using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public PlayableDirector director;
    public GameObject cutsceneCamera;
    public GameObject playerPOV;
    [SerializeField] private GameObject cutsceneCam;

    [Header("Player Settings")]
    public GameObject playerController;

    private void Start()
    {

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

        if(cutsceneCam != null) cutsceneCam.SetActive(false);

        Debug.Log("cutsvene end");

        DayManager.Instance.StartDay();

        gameObject.SetActive(false);

    }
}