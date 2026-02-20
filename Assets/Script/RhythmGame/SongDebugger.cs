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
        
        if (!isScrubbing && songSlider != null) 
        { 
            songSlider.SetValueWithoutNotify(Conductor.Instance.songPosition);
        }

        UpdateUI();
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
