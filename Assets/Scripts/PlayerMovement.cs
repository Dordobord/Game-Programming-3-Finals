using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("References")]
    [SerializeField] private CharacterController cc;

    public bool canMove = false;  

    private float gravity = -50f;
    private float ySpeed = 0f;
    private Vector3 moveDirection;
    private Vector3 prevPos;

    public Vector3 CurrentVelocity { get; private set; }

    private void Reset()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        prevPos = transform.position;
    }

    private void Update()
    {
        if (!canMove) return;  

        HandleMovement();
        HandleGravity();
        ApplyMovement();
        UpdateVelocity();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (!cc.isGrounded) return;

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.sqrMagnitude > 1)
            move.Normalize();

        moveDirection = move * moveSpeed;

        if (Input.GetButtonDown("Jump"))
            ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void HandleGravity()
    {
        ySpeed += gravity * Time.deltaTime;

        if (cc.isGrounded && ySpeed < 0)
            ySpeed = -2f;
    }

    private void ApplyMovement()
    {
        Vector3 finalMovement = moveDirection + Vector3.up * ySpeed;
        cc.Move(finalMovement * Time.deltaTime);
    }

    private void UpdateVelocity()
    {
        CurrentVelocity = (transform.position - prevPos) / Time.deltaTime;
        prevPos = transform.position;
    }
}
