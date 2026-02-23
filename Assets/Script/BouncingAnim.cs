using UnityEngine;

public class BouncingAnim : MonoBehaviour
{
    [SerializeField] private float height = 5.3f;
    [SerializeField] private float speed = 2f;
    private bool playAnim = false;

    private Vector3 startPos;

    private void Start()
    {
        playAnim = false;
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (playAnim)
        {
            float newY = startPos.y + Mathf.Abs(Mathf.Sin(Time.time * speed)) * height;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    private void OnEnable()
    {
        playAnim = true;
    }

}
