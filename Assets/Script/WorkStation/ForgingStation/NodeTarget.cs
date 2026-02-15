using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NodeTarget : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float shrinkTime = 1.2f;
    [SerializeField] private float hitWindow = 0.2f;

    [SerializeField] private Image targetNode;
    [SerializeField] private RectTransform closingRing;
    [SerializeField] private ForgingGame gameManager;

    private float timer = 0f;
    private bool hasBeenClicked = false;

    public void SetUp(ForgingGame manager)
    {
        gameManager = manager;
    }

    private void Update()
    {
        if(hasBeenClicked) { return; }

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
            gameManager.ReportHit(true);
        }

        if (!isSuccess)
        {
            targetNode.color = Color.red;
            Debug.Log("Missed Hit!");
            gameManager.ReportHit(false);
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
