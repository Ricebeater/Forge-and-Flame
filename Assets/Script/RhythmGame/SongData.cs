using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongmapData
{
    public string title;
    public string artist;
    public float bpm;
    public float startOffset;
    public List<NoteData> notes;
}

[System.Serializable]
public class NoteData
{
    public float timeInBeats;
    public float x;
    public float y;
    public int type;
}

