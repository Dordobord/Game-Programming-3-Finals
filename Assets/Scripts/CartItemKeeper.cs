using UnityEngine;

public class CartItemKeeper : MonoBehaviour
{
    private Transform cartRoot;

    private void Awake()
    {
        cartRoot = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        ShoppingItem item = other.GetComponent<ShoppingItem>();
        if (!item) return;

        Rigidbody rb = other.attachedRigidbody;
        if (!rb) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;

        other.isTrigger = true;

        other.transform.SetParent(cartRoot);
    }

    private void OnTriggerExit(Collider other)
    {
        ShoppingItem item = other.GetComponent<ShoppingItem>();
        if (!item) return;

        Rigidbody rb = other.attachedRigidbody;
        if (!rb) return;

        rb.isKinematic = false;
        rb.useGravity = true;

        other.isTrigger = false;

        other.transform.SetParent(null);
    }
}
