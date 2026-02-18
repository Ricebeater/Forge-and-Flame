using Unity.Cinemachine;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance { get; private set; }

    [Header("Info")]
    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;

    public AudioSource musicSource;

    [Header("Setting")]
    public float firstBeatOffset = 0;

    public static float inputOffset = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dspSongTime = (float)AudioSettings.dspTime;
    }


    [SerializeField, Range(0,4)] private float pitchChange;
    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;

        musicSource.pitch = pitchChange;
    }

    public void init()
    {
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    public float GetAudioTime()
    {
        return songPosition + inputOffset;
    }
}
