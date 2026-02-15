using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForgingGame : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float shrinkingSpeed = 1f;

    [Header("UI")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    private bool isMinigameActive = false;
    private float spawnTimer = 0f;
    private int score = 0;

    private void Start()
    {
        gameUI.SetActive(false);
        isMinigameActive = false;
    }

    public void StartMiniGame()
    {
        isMinigameActive = true;
        score = 0;

        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void EndMiniGame()
    {
        isMinigameActive = false;
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        HandleUI();

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            GameObject newNode = Instantiate(nodePrefab, spawnArea);

            float width = spawnArea.rect.width;
            float height = spawnArea.rect.height;

            float randomX = Random.Range(-width / 2f + 50f, width / 2f - 50f);
            float randomY = Random.Range(-height / 2f + 50f, height / 2f - 50f);

            RectTransform targetRect = newNode.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(randomX, randomY);

            NodeTarget script = newNode.GetComponent<NodeTarget>();
            script.SetUp(this);
        }    
    }

    public void ReportHit(bool isSuccess)
    {
        if (isSuccess) score += 100;
        else score -= 50;

    }

    private void HandleUI()
    {
        gameUI.SetActive(isMinigameActive);
        scoreText.text = "Score: " + score;
    }
}
