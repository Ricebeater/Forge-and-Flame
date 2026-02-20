using Unity.Cinemachine;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }

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

    [SerializeField, Range(0.25f,2f)] private float playbackSpeed;
    
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

    private void Start()
    {
        dspSongTime = (float)AudioSettings.dspTime;
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = secPerBeat > 0 ? songPosition / secPerBeat : 0f;

        musicSource.pitch = playbackSpeed;
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

    #region Debug controls

    [SerializeField]private bool isSeeking;

    public void SeekPreview(float timeInSeconds)
    {
        isSeeking = true;
        musicSource.time = Mathf.Clamp(timeInSeconds, 0f, musicSource.clip.length);
    }

    public void CommitSeek(float timeInSeconds)
    {
        timeInSeconds = Mathf.Clamp(timeInSeconds, 0f, musicSource.clip.length);
        musicSource.time = timeInSeconds;
        dspSongTime = (float)AudioSettings.dspTime - timeInSeconds - firstBeatOffset;
        isSeeking = false;
    }

    public void SetSpeed(float speed)
    {
        playbackSpeed = Mathf.Clamp(speed, 0.25f, 2f);
        musicSource.pitch = playbackSpeed;

        secPerBeat = (60f / songBpm) / playbackSpeed;
    }

    #endregion
}
