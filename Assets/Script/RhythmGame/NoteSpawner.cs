using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public List<NoteData> notes = new List<NoteData>();
    public int nextNoteIndex = 0;
    public float spawnAheadBeats = 2f;
    public GameObject notePrefab;
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

        float currentBeat = Conductor.instance.songPositionInBeats;

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

        float screenWidth = Screen.width;
        float xPos = (data.x / 1) * screenWidth;

        Vector3 spawnPos = new Vector3(xPos, data.y, 0);

        GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, canvas);
    }
}
