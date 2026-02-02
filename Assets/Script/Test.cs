using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int targetFPS;

    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
    }
}
