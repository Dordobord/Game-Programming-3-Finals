using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ShoppingItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private float itemPrice;

    private Rigidbody rb;
    private Collider col;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    public string GetDescription()
    {
        return isHeld ? "Throw" : "Grab";
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (!isHeld)
        {
            interactor.TryPickupItem(this);
        }
        else
        {
            interactor.ThrowItem();
        }
    }

    public void OnPickup(PlayerInteractor interactor)
    {
        AudioManager.main.PlaySFX("PickUp");
        isHeld = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
    }

    public void FollowHoldPoint(Transform holdPoint, float lerpSpeed)
    {
        if (!isHeld) return;

        transform.position = Vector3.Lerp(transform.position, holdPoint.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, holdPoint.rotation, lerpSpeed * Time.deltaTime);
    }

    public void OnDrop()
    {
        isHeld = false;

        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }

    public void OnThrow(Vector3 velocity)
    {
        isHeld = false;

        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;

        rb.linearVelocity = velocity; 
    }

    public string ItemName => itemName;
    public float Price => itemPrice;
}

