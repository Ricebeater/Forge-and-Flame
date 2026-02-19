using UnityEngine;
using System.Collections.Generic;

public class HitManager : MonoBehaviour
{
    public static HitManager Instance { get; private set; }

    public float perfectWindow = 0.1f;
    public float graetWindow = 0.18f;
    public float niceWindow = 0.22f;

    public KeyCode[] lanesKey = { KeyCode.A, KeyCode.S, KeyCode.D };

    private List<NoteController> activeNotes = new List<NoteController>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        for (int lane = 0; lane < lanesKey.Length; lane++)
        {
            if (Input.GetKeyDown(lanesKey[lane]))
            {
                TryHitLane(lane);
            }
        }
    }

    public void RegisterNote(NoteController note)
    {
        if(!activeNotes.Contains(note))
        {
            activeNotes.Add(note);
        }
    }

    public void UnregisterNote(NoteController note)
    {
        activeNotes.Remove(note);
    }

    private void TryHitLane(int lane)
    {
        float currentBeat = Conductor.Instance.songPositionInBeats;

        NoteController closestNote = null;
        float bestDelta = float.MaxValue;

        foreach (NoteController note in activeNotes)
        {
            if(note.lane != lane) continue;

            float delta = Mathf.Abs(note.targetBeat - currentBeat);
            if (delta > bestDelta)
            { 
                bestDelta = delta;
                closestNote = note;
            }
        }

        if(closestNote == null)
        {
            Debug.Log("Miss, no note to hit");
            return;
        }

        Judgement result;
        if      (bestDelta <= perfectWindow)     { result = Judgement.Perfect; }
        else if (bestDelta <= graetWindow)       { result = Judgement.Great; }
        else if (bestDelta <= niceWindow)        { result = Judgement.Nice; }
        else                                     { result = Judgement.Miss; }

        Debug.Log($"Lane {lane}: {result} (best delta: {bestDelta:F3} beats off)");
        closestNote.Hit(result);
    }
}
public enum Judgement
{
    Perfect,
    Great,
    Nice,
    Miss
}