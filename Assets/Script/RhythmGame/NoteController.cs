using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    public int lane { get; private set; }
    public float targetBeat { get; private set; }

    private NoteData noteData;
    private float spawnAheadBeats;
    private float hitLineY;

    private float spawnY;
    private float spawnBeat;
    private float laneX;

    private bool isHit = false;
    private bool isMissed = false;

    private Image spriteImage;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
    }

    public void Init(NoteData data, float aheadBeats, float hitY, Vector3 spawnPosition)
    {
        noteData        = data;
        spawnAheadBeats = aheadBeats;
        hitLineY        = hitY;

        lane            = data.lane;
        targetBeat      = data.timeInBeats;

        laneX           = spawnPosition.x;
        spawnY          = spawnPosition.y;
        spawnBeat       = Conductor.Instance.songPositionInBeats; 

        HitManager.Instance.RegisterNote(this);
    }

    private void Update()
    {
        if (isHit) { return; }

        float currentBeat = Conductor.Instance.songPositionInBeats;
     
        float t = (currentBeat - spawnBeat) / spawnAheadBeats;
        float newY = Mathf.Lerp(spawnY, hitLineY, Mathf.Clamp01(t));
        transform.position = new Vector3(laneX, newY, 0f);


        if(!isMissed && currentBeat > targetBeat + 0.22f)
        {
            isMissed = true;
            OnMiss();
        }

        if (currentBeat > targetBeat + 0.5f)
        {
            DestroyNote();
        }
    }

    public void Hit(Judgement judgement)
    {
        if (isHit) { return; }
        isHit = true;

        HitManager.Instance.UnregisterNote(this);

        switch (judgement)
        {
            case Judgement.Perfect:
                spriteImage.color = Color.green;
                StartCoroutine(HitAnimation());
                break;
            case Judgement.Great:
                spriteImage.color = Color.greenYellow;
                StartCoroutine(HitAnimation());
                break;
            case Judgement.Nice:
                spriteImage.color = Color.yellow;
                StartCoroutine(HitAnimation());
                break;
            case Judgement.Miss:
                OnMiss();
                break;
        }
    }

    public void OnMiss()
    {
        SetColor(new Color(1f, 0.3f, 0.3f, 0.5f));
        StartCoroutine(HitAnimation());
    }

    private void SetColor(Color color)
    {
        if(spriteImage != null)
        {
            spriteImage.color = color;
        }
    }

    private IEnumerator HitAnimation()
    {
        float duration = 0.12f;
        float elapsed = 0f;
        Vector3 origin = transform.localScale;
        Vector3 target = origin * 1.6f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(origin, target, elapsed / duration);
            yield return null;
        }

        DestroyNote();
    }

    private void DestroyNote()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(HitManager.Instance != null)
        {
            HitManager.Instance.UnregisterNote(this);
        }   
    }
}
