using UnityEngine;

public class NoteController : MonoBehaviour
{
    private NoteData noteData;
    private float spawnAheadBeats;
    private float hitLineY;

    private float spawnY;
    private float spawnBeat;

    public void Init(NoteData data, float aheadBeats, float hitY)
    {
        noteData        = data;
        spawnAheadBeats = aheadBeats;
        hitLineY        = hitY;

        spawnY = transform.position.y + (Screen.width / 2);
        spawnBeat = Conductor.Instance.songPositionInBeats;
    }

    private void Update()
    {
        float currentBeat = Conductor.Instance.songPositionInBeats;

        float t = (currentBeat - spawnBeat) / spawnAheadBeats;
        t = Mathf.Clamp01(t);

        float newY = Mathf.Lerp(spawnY, hitLineY, t);
        transform.position = new Vector3(noteData.x * 1920f, newY, 0f);

        if (currentBeat > noteData.timeInBeats + 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
