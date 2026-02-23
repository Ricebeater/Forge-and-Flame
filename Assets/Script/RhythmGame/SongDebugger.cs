
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SongDebugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider songSlider;
    [SerializeField] private TMP_Text positionLabel;
    [SerializeField] private Button speedDownBtn;
    [SerializeField] private Button speedUpBtn;
    [SerializeField] private TMP_Text speedLabel;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.Tab;
    public float speedStep = 0.25f;
    public float minSpeed = 0.25f;
    public float maxSpeed = 2.0f;

    private float currentSpeed = 1f;
    private bool isScrubbing = false;
    private bool isVisible = false;

    [Header("Recording")]
    public KeyCode laneKey0 = KeyCode.A;
    public KeyCode laneKey1 = KeyCode.S;
    public KeyCode laneKey2 = KeyCode.D;
    public KeyCode exportKey = KeyCode.Return;
    public string exportTitle = "Recored Song";
    public string exportArtist = "rec";

    private List<NoteData> recordedNotes = new List<NoteData>();

    private void Start()
    {

#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
        GameObject.SetActive(false);
        return;
#endif

        if(songSlider != null && Conductor.Instance != null && Conductor.Instance.musicSource.clip != null)
        {
            songSlider.minValue = 0f;
            songSlider.maxValue = Conductor.Instance.musicSource.clip.length;
            songSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        if (speedDownBtn != null) speedDownBtn.onClick.AddListener(SpeedDown);
        if (speedUpBtn != null) speedUpBtn.onClick.AddListener(SpeedUp);
        
        ApplySpeed();

        SetVisible(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(toggleKey))
        {
            SetVisible(!isVisible);
        }

        if(!isVisible) { return; }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"Current Song Position: {Conductor.Instance.songPosition:F2}s, Beat: {Conductor.Instance.songPositionInBeats:F2}");
        }

        TryRecordLane(laneKey0, 0);
        TryRecordLane(laneKey1, 1);
        TryRecordLane(laneKey2, 2);

        if (Input.GetKeyDown(exportKey))
        {
            ExportToJson();
        }

        if (!isScrubbing && songSlider != null) 
        { 
            songSlider.SetValueWithoutNotify(Conductor.Instance.songPosition);
        }

        UpdateUI();
    }

    private void TryRecordLane(KeyCode key, int lane)
    {
        if (Input.GetKeyDown(key))
        {
            float beat = Mathf.Round(Conductor.Instance.songPositionInBeats * 100f) / 100f; //round for 2 decimal

            recordedNotes.Add(new NoteData { timeInBeats = beat, lane = lane, type = 0 });
            Debug.Log($"Recorded note — Lane: {lane}, Beat: {beat:F2}");
        }
    }

    private void ExportToJson()
    {
        recordedNotes.Sort((a, b) => a.timeInBeats.CompareTo(b.timeInBeats));

        float bpm = Conductor.Instance.songBpm;
        float endBeat = recordedNotes[recordedNotes.Count - 1].timeInBeats + 2f;

        SongmapData data = new SongmapData
        {
            title = exportTitle,
            artist = exportArtist,
            bpm = bpm,
            startOffset = Conductor.Instance.firstBeatOffset,
            endBeat = endBeat,
            notes = recordedNotes
        };

        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.streamingAssetsPath, exportTitle + ".json");

        File.WriteAllText(path, json);
        Debug.Log($"Exported {recordedNotes.Count} notes to: {path}");

        recordedNotes.Clear();
    }

    #region Slider

    public void OnBeginDrag()
    {
        isScrubbing = true;
    }

    public void OnEndDrag()
    {
        if(!isScrubbing) { return; }
        isScrubbing = false;

        Conductor.Instance.CommitSeek(songSlider.value);
    }

    private void OnSliderChanged(float value)
    {
        if(!isScrubbing) { return; }

        Conductor.Instance.SeekPreview(value);
    }

    #endregion

    #region Speed Control

    private void SpeedUp()
    {
        currentSpeed = Mathf.Min(maxSpeed, currentSpeed + speedStep);
        ApplySpeed();
    }

    private void SpeedDown()
    {
        currentSpeed = Mathf.Max(minSpeed, currentSpeed - speedStep);
        ApplySpeed();   
    }

    private void ApplySpeed()
    {
        if(Conductor.Instance != null) 
        { 
            Conductor.Instance.SetSpeed(currentSpeed);
        }

        if(speedLabel != null)
        {
            speedLabel.text = $"{currentSpeed:F2}x";
        }
    }

    #endregion

    private void UpdateUI()
    {
        if (positionLabel == null || Conductor.Instance == null) return;

        float beat = Conductor.Instance.songPositionInBeats;
        float time = Conductor.Instance.songPosition;
        positionLabel.text = $"Beat: {beat:F2}  |  Time: {time:F2}s";
    }

    private void SetVisible(bool visible)
    {
        isVisible = visible;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(visible);
        }
    }
}
