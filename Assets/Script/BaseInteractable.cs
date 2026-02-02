using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color originalColor;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public abstract void Interact(PlayerInteractor player);
    public abstract void EscapeInteract(PlayerInteractor player);

    public void OnHoverEnter()
    {
        if(objectRenderer != null)
            objectRenderer.material.color = highlightColor;
        Debug.Log("Hovering over " + gameObject.name);
    }

    public void OnHoverExit()
    {
        if(objectRenderer != null)
            objectRenderer.material.color = originalColor;
        Debug.Log("Stopped hovering over " + gameObject.name);
    }

}
