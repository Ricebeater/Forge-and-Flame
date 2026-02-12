using UnityEngine;

public class ForgingManager : MonoBehaviour
{
    [SerializeField] private GameObject node;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnTimer = 0f;
    [SerializeField] private RectTransform spawnArea;
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            GameObject newNode = Instantiate(node, spawnArea);

            float width = spawnArea.rect.width;
            float height = spawnArea.rect.height;

            float randomX = Random.Range(-width / 2f + 50f, width / 2f - 50f);
            float randomY = Random.Range(-height / 2f + 50f, height / 2f - 50f);

            RectTransform targetRect = newNode.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(randomX, randomY);
        }    
    }
}
