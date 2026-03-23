using System.Collections;
using UnityEngine;

public class FeedbackText : MonoBehaviour
{
    public void DestroyAtTheEnd()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.localPosition += Vector3.up;
    }
}
