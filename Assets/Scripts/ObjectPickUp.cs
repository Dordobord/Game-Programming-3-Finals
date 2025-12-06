/* using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float smoothLerp = 20f;

    private Rigidbody rb;
    private bool isHolding = false;
    private Transform holdPoint;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (isHolding && holdPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, holdPoint.position, smoothLerp * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, holdPoint.rotation, smoothLerp * Time.deltaTime);;
        }
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (!isHolding)
            PickupObject(interactor);
        else
            ThrowObject(interactor);
    }

    private void PickupObject(PlayerInteraction interactor)
    {
        if (interactor.heldObj != null) 
            return;

        holdPoint = interactor.holdPoint;

        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = false;

        isHolding = true;
        interactor.heldObj = this;
    }

    private void ThrowObject(PlayerInteraction interactor)
    {
        isHolding = false;

        Vector3 throwDirection = holdPoint.forward;
        holdPoint = null;

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = throwDirection * throwForce;

        col.enabled = true;

        interactor.heldObj = null;
    }

    public string GetDescription()
    {
        return isHolding ? "Throw" : "Grab";
    }

    public bool IsHolding()
    {
        return isHolding;
    }
}
 */