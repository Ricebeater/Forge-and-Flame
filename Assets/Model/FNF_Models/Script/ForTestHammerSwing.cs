using System.Collections;
using UnityEngine;

public class ForTestHammerSwing : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float swingAngle = 80f;
    public float swingUpSpeed = 100f;
    public float smashDownSpeed = 400f;

    [Header("Hammer Settings")]
    public float restingAngleY = 0f;
    [Header("Effects")]
    public ParticleSystem smashFX;

    private bool isSwinging = false;
    private float currentSwingAngle = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isSwinging)
        {
            StartCoroutine(SwingAndSmash());
        }
    }

    IEnumerator SwingAndSmash()
    {
        isSwinging = true;

        float targetSwingUp = swingAngle;
        float elapsedUp = 0f;

        while (elapsedUp < 1f)
        {
            elapsedUp += Time.deltaTime * (swingUpSpeed / 100f);

            float angleX = Mathf.Lerp(restingAngleY, targetSwingUp, elapsedUp);
            transform.localRotation = Quaternion.Euler(angleX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        float elapsedDown = 0f;
        while (elapsedDown < 1f)
        {
            elapsedDown += Time.deltaTime * (smashDownSpeed / 100f);

            float t = elapsedDown * elapsedDown;

            float angleX = Mathf.Lerp(targetSwingUp, restingAngleY, t);
            transform.localRotation = Quaternion.Euler(angleX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            yield return null;
        }
        isSwinging = false;

        transform.localRotation = Quaternion.Euler(restingAngleY, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}