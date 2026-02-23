using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    
    public GameObject notePrefab;
    public Transform spawnCanvas;

    public Transform[] lanePositions;

    public float spawnAheadBeats = 2f;
    
    private List<NoteData> notes = new List<NoteData>();
    private int nextNoteIndex = 0;

    [SerializeField] private Transform hitLine;

    public void SetNotes(List<NoteData> noteList)
    {
        notes = noteList;
        notes.Sort((a, b) => a.timeInBeats.CompareTo(b.timeInBeats));

        nextNoteIndex = 0;
        Debug.Log($"NoteSpawner: recieved {notes.Count} notes.");
    }

    private void Update()
    {
        if(notes == null) { return; }

        float currentBeat = Conductor.Instance.songPositionInBeats;

        while (nextNoteIndex < notes.Count && notes[nextNoteIndex].timeInBeats <= currentBeat + spawnAheadBeats)
        {
            SpawnNote(notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    private void SpawnNote(NoteData data)
    {
        if(notePrefab == null)
        {
            Debug.LogWarning("NoteSpawner: notePrefab is not assigned.");
            return;
        }

        if (data.lane < 0 || data.lane >= lanePositions.Length)
        {
            Debug.LogWarning($"NoteSpawner: note has invalid lane index {data.lane}. Check your JSON and lanePositions array.");
            return;
        }

        float hitLineY = hitLine.position.y;

        Vector3 spawnPos = lanePositions[data.lane].position;
        
        GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, spawnCanvas);

        NoteController ctrl = noteObj.GetComponent<NoteController>();

        ctrl.Init(data, spawnAheadBeats, hitLineY, spawnPos);
    }

    public void ClearOldNotes()
    {
        foreach(Transform child in spawnCanvas)
        {
            Destroy(child.gameObject);
        }

        notes.Clear();
        nextNoteIndex = 0;
    }
}
