using UnityEngine;

public class IngotToSword : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject hitVFXPrefab;
    [SerializeField] private GameObject forgeVFXPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform swordSpawnPoint;

    [Header("Forging Settings")]
    public int maxHits = 3;
    public float stretchZ = 0.2f;
    public float shrinkY = 0.05f;

    private int hits = 0;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Hammer") && hits < maxHits)
        {
            hits++;

            if (hitVFXPrefab != null)
            {
                ContactPoint contact = col.contacts[0];
                Quaternion rot = Quaternion.LookRotation(-contact.normal);
                GameObject vfx = Instantiate(hitVFXPrefab, contact.point, rot);
                Destroy(vfx, 1f);
            }

            if (hits >= maxHits)
                SpawnSword();
            else
                Stretch();
        }
    }

    void Stretch()
    {
        Vector3 s = transform.localScale;
        s.z += stretchZ;
        s.y -= shrinkY;
        s.x -= shrinkY * 0.5f;
        transform.localScale = s;
    }

    void SpawnSword()
    {
        if (forgeVFXPrefab != null)
        {
            GameObject fvfx = Instantiate(forgeVFXPrefab, transform.position, Quaternion.identity);
            Destroy(fvfx, 2f);
        }

        if (swordPrefab != null)
        {

            if (swordSpawnPoint != null)
            {
                Instantiate(swordPrefab, swordSpawnPoint.position, swordSpawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("อย่าลืมลาก Sword Spawn Point มาใส่ใน Inspector นะครับ!");
                Instantiate(swordPrefab, transform.position, transform.rotation);
            }
        }

        Destroy(gameObject);
        Debug.Log("Forge Success: Sword Spawned at designated point!");
    }
}