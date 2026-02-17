using System.IO;
using UnityEngine;

public class SongmapLoader : MonoBehaviour
{
    public Conductor conductor;

    private void Start()
    {
        LoadLevel("Loyalty.json");
    }

    public void LoadLevel(string filename)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            SongmapData songData = JsonUtility.FromJson<SongmapData>(jsonContent);

            Debug.Log($"Loaded Song: {songData.title} by {songData.artist}");
            InitializeGame(songData);
        }
        else
        {
            Debug.LogError($"Songmap file not found at: {filePath}");
        }
    }
    void InitializeGame(SongmapData data)
    {
        if (conductor != null)
        {
            conductor.songBpm = data.bpm;
            conductor.firstBeatOffset = data.startOffset;

            // Pass the notes to your Spawner script (Example)
            // noteSpawner.SetNotes(data.notes);
        }
    }
}
