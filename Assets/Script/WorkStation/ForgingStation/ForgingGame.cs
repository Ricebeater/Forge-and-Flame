using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ForgingGame : MonoBehaviour, IPointerDownHandler
{
    private float shrinkTime = 1.2f;
    private float hitWindow = 0.2f;

    [SerializeField] private Image targetNode;
    [SerializeField] private RectTransform closingRing;

    private float timer = 0f;
    private bool hasBeenClicked = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / shrinkTime;
        float scale = Mathf.Lerp(2.5f, 1f, progress);

        closingRing.localScale = Vector3.one * scale;

        if (timer >= shrinkTime + hitWindow)
        {
            HandleHit(false);
        }
    }

    private void HandleHit(bool isSuccess)
    {
        hasBeenClicked = true;

        if (isSuccess)
        {
            targetNode.color = Color.green;
            Debug.Log("Successful Hit!");
        }

        if (!isSuccess)
        {
            targetNode.color = Color.red;
            Debug.Log("Missed Hit!");
        }

        Destroy(gameObject, 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasBeenClicked) return;

        float timeDifference = Mathf.Abs(timer - shrinkTime);

        if (timeDifference <= hitWindow)
        {
            HandleHit(true);
        }
        else
        {
            HandleHit(false);
        }
    }
}
