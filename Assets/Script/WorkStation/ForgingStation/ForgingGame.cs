using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForgingGame : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int totalNodes = 5;
    private int spawnedNodes = 0;

    [Header("UI")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    public bool isMiniGameActive = false;
    private float spawnTimer = 0f;
    private int score = 0;

    private void Start()
    {
        gameUI.SetActive(false);
        isMiniGameActive = false;
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        score = 0;
        spawnedNodes = 0;

        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void EndMiniGame()
    {
        isMiniGameActive = false;
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        HandleUI();

        if (spawnedNodes == totalNodes) { return; }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            GameObject newNode = Instantiate(nodePrefab, spawnArea);

            spawnedNodes += 1;

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
        else score -= 100;

    }

    public float CalculatedScore()
    {
        return score / totalNodes;
    }

    private void HandleUI()
    {
        gameUI.SetActive(isMiniGameActive);
        scoreText.text = "Score: " + score;
    }
}
