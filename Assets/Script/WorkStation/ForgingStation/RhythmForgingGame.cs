using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RhythmForgingGame : MonoBehaviour
{
    [SerializeField] private GameObject game;
    [SerializeField] private SongmapLoader mapLoader;

    [SerializeField] private TextMeshProUGUI scoreText;

    public bool isMiniGameActive = false;
    [SerializeField]private bool isMiniGameFinnished = false;

    private void Start()
    {
        isMiniGameActive = false;
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        isMiniGameFinnished = false;
        game.SetActive(true);
        HitManager.Instance.ResetScore();

        Conductor.Instance.ResetAudio();
        

        if (mapLoader != null)
        {
            mapLoader.StartGame();
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    public void EndMiniGame()
    {
        isMiniGameFinnished = false;
        isMiniGameActive = false;
        Conductor.Instance.ResetAudio();

        game.SetActive(false);
        mapLoader.noteSpawner.ClearOldNotes();

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }

        Debug.Log("End game");
    }

    private void Update()
    {
        if (!isMiniGameActive) { return; }

        if(Conductor.Instance.endBeat > 0f && Conductor.Instance.songPositionInBeats >= Conductor.Instance.endBeat)
        {
            isMiniGameActive = false;
            isMiniGameFinnished = true;
            Conductor.Instance.FadeOutMusic();
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + HitManager.Instance.CalculatedScore().ToString("F1");
            scoreText.gameObject.SetActive(isMiniGameFinnished);
        }
    }

}
