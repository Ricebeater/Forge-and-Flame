using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public List<NoteData> notes = new List<NoteData>();
    public GameObject notePrefab;
    public int nextNoteIndex = 0;
    public float spawnAheadBeats = 2f;
    public float hitLineY = -4f;

    public Transform canvas;

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

        if (nextNoteIndex < notes.Count && notes[nextNoteIndex].timeInBeats <= currentBeat + spawnAheadBeats)
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

        Vector3 spawnPos = new Vector3(data.x, data.y, 0);

        GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, canvas);
        
        NoteController ctrl = noteObj.GetComponent<NoteController>();
        ctrl.Init(data, spawnAheadBeats, hitLineY);
    }
}
