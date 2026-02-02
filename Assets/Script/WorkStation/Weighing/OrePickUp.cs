using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class OrePickUp : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Collider dragZone;
    private Rigidbody rb;

    private bool isDragging = false;
    private Vector3 offset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(UnityEngine.Cursor.lockState == CursorLockMode.Locked)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPickUp();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            DropOre();
        }

        if (isDragging)
        {
            DragOre();
        }
    }

    private void TryPickUp()
    {
        

        Ray ray = GetMousePosition();
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                isDragging = true;
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;

                RaycastHit zoneHit;
                if (dragZone.Raycast(ray, out zoneHit, 100f))
                {
                    offset = transform.position - zoneHit.point;
                }
            }
        }


    }

    private void DragOre()
    {
        Ray ray = GetMousePosition();
        RaycastHit hit;

        if(dragZone.Raycast(ray, out hit, 100f))
        {
            transform.position = hit.point + offset;
        }
    }

    private void DropOre()
    {
        isDragging = false;
        rb.isKinematic = false;
    }

    private Ray GetMousePosition()
    {
        Vector2 mousePsition = Mouse.current.position.ReadValue();
        return camera.ScreenPointToRay(mousePsition);
    }
}
