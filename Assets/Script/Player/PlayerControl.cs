using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Player Camera")]
    [SerializeField, Range(0.01f, 1.00f)] private float camRotateSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private Camera playerCamera;

    public bool isWorkingAtStation;
    [HideInInspector] public bool _isWorkingAtStation => isWorkingAtStation;

    private InputAction lookAction;
    private InputAction escapeAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lookAction = InputSystem.actions.FindAction("Look");
        escapeAction = InputSystem.actions.FindAction("Escape");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isWorkingAtStation = false;
    }

    void Update()
    {
        if (!isWorkingAtStation) 
        {  DisableMouseCursor(); }
        else 
        { EnableMouseCursor(); }
        
        Looking();

    }

    #region Mouse Looking

    private float xRotation;
    private float yRotation;
    private void Looking()
    {
        if(isWorkingAtStation) return;

        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        yRotation += mouseDelta.x * camRotateSpeed;
        xRotation -= mouseDelta.y * camRotateSpeed;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void DisableMouseCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            return;
        }
    }

    private void EnableMouseCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }
    }

    #endregion

    public void GetOutOfStation()
    {
        isWorkingAtStation = false;
        rb.isKinematic = false;
    }

    public void AlignToStation(Transform target, float duration)
    {
        StartCoroutine(AlignToStationRoutine(target, duration));
    }

    private IEnumerator AlignToStationRoutine(Transform target, float duration)
    {
        isWorkingAtStation = true;

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Vector3 startPos = transform.position;
        Vector3 noYTargetPos = new Vector3(target.position.x, startPos.y, target.position.z);

        float startX = xRotation;
        float startY = yRotation;

        float targetX = (target.eulerAngles.x > 180) ? target.eulerAngles.x - 360 : target.eulerAngles.x;
        float targetY = target.eulerAngles.y;

        float timeElapsed = 0f;


        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = t * t * (3f - 2f * t); // this is call "smoothstep" cool equation you should go and research it bro

            transform.position = Vector3.Lerp(startPos, noYTargetPos, t);

            xRotation = Mathf.LerpAngle(startX, targetX, t);
            yRotation = Mathf.LerpAngle(startY, targetY, t);

            playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = noYTargetPos;

        //cam
        xRotation = targetX;
        yRotation = target.eulerAngles.y;

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
