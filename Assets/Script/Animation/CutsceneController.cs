using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public PlayableDirector director;
    public GameObject cutsceneCamera;

    [Header("Player Settings")]
    public GameObject playerController;

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

        gameObject.SetActive(false); 
    }
}