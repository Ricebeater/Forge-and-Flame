using TMPro;
using UnityEngine;

public class RhythmForgingGame : MonoBehaviour
{
    [SerializeField] private GameObject game;
    [SerializeField] private SongmapLoader mapLoader;

    public bool isMiniGameActive = false;

    private void Start()
    {
        isMiniGameActive = false;
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        game.SetActive(true);

        if(mapLoader != null)
        {
            mapLoader.StartGame();
        }
    }

    public void EndMiniGame()
    {
        isMiniGameActive = false;
        game.SetActive(false);

        mapLoader.noteSpawner.ClearOldNotes();
    }

}
