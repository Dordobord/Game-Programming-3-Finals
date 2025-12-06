/* using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CartController : MonoBehaviour
{
    public Rigidbody rb;

    public float moveSpeed = 3f;
    public float turnSpeed = 120f;

    [HideInInspector]
    public bool isControlled = false;

    public Transform pushPoint; 

    private float inputV;
    private float inputH;

    public void SetInput(float vertical, float horizontal)
    {
        inputV = vertical;
        inputH = horizontal;
    }

    private void FixedUpdate()
    {
        if (!isControlled) return;

        Vector3 moveDir = transform.right * inputV * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);

        Quaternion turnRot =
            Quaternion.Euler(0f, inputH * turnSpeed * Time.fixedDeltaTime, 0f);

        rb.MoveRotation(rb.rotation * turnRot);
    }
}

 */
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CartController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float movementAcceleration = 4f;
    public float movementDeceleration = 6f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 320f;
    [Range(0f, 1f)]
    public float rotationSmoothing = 0.25f;

    [HideInInspector]
    public bool isControlled = false;

    public Transform pushPoint;

    private float inputForward;
    private float inputHorizontal;

    private float currentMovementSpeed = 0f;
    private bool isRotationBlocked = false;

    public void SetInput(float forwardInput, float horizontalInput)
    {
        inputForward = forwardInput;
        inputHorizontal = horizontalInput;
    }

    private void FixedUpdate()
    {
        if (!isControlled) return;

        UpdateAcceleration();
        ApplyMovement();
        ApplyRotation();

        if (Mathf.Abs(currentMovementSpeed) > 0.1f)
            AudioManager.main.PlayLoopSFX("RollingCart");
        else
            AudioManager.main.StopLoopSFX();
    }

    private void UpdateAcceleration()
    {
        float desiredSpeed = inputForward * movementSpeed;

        if (Mathf.Abs(inputForward) > 0.1f)
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, desiredSpeed, movementAcceleration * Time.fixedDeltaTime);
        else
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, 0f, movementDeceleration * Time.fixedDeltaTime);
    }

    private void ApplyMovement()
    {
        Vector3 movementVector = transform.right * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movementVector);
    }

    private void ApplyRotation()
    {
        if (isRotationBlocked) return;

        float desiredRotation = inputHorizontal * rotationSpeed * Time.fixedDeltaTime;
        float smoothedRotation = Mathf.Lerp(0f, desiredRotation, rotationSmoothing);

        Quaternion rotationDelta = Quaternion.Euler(0f, smoothedRotation, 0f);
        rb.MoveRotation(rb.rotation * rotationDelta);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contactCount == 0) return;

        Vector3 toCollision = collision.contacts[0].point - transform.position;
        float dot = Vector3.Dot(transform.right, toCollision.normalized);

        isRotationBlocked = dot > 0.5f;
    }

    private void OnCollisionExit(Collision collision)
    {
        isRotationBlocked = false;
    }
}
