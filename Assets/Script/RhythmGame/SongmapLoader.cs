using System.IO;
using Unity.Jobs;
using UnityEngine;

public class SongmapLoader : MonoBehaviour
{
    public NoteSpawner noteSpawner;

    public void StartGame()
    {
        LoadLevel("Blue-Danube.json");
        Debug.Log("Game Started with Songmap!");
    }

    private void LoadLevel(string filename)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            SongmapData songmapData = JsonUtility.FromJson<SongmapData>(jsonContent);

            Debug.Log($"Loaded Song: {songmapData.title} by {songmapData.artist}");

            InitializeGame(songmapData);
        }
        else
        {
            Debug.LogError($"Songmap file not found at: {filePath}");
        }
    }
    private void InitializeGame(SongmapData data)
    {
        Conductor.Instance.songBpm          = data.bpm;
        Conductor.Instance.firstBeatOffset  = data.startOffset;
        Conductor.Instance.endBeat          = data.endBeat;
        Conductor.Instance.init();

        if(noteSpawner != null)
        {
            noteSpawner.SetNotes(data.notes);
        }

        HitManager.Instance.SetTotalNotes(data.notes.Count);
    }

}
