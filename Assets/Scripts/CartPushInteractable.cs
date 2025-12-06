using UnityEngine;

public class CartPushInteractable : MonoBehaviour, IInteractable
{
    [Header("Knockback Attributes")]
    [SerializeField] private float knockbackForce = 20f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private bool scaleWithSpeed = true;

    [HideInInspector] public CartController cartController;

    private Rigidbody cartRb;

    private void Awake()
    {
        cartRb = GetComponent<Rigidbody>();
        cartController = GetComponent<CartController>();
    }

    public string GetDescription()
    {
        return "Push Cart";
    }

    public void Interact(PlayerInteractor interactor)
    {
        interactor.StartPushingCart(this, cartController.pushPoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        Vector3 direction = collision.contacts[0].point - transform.position;
        direction.Normalize();

        Vector3 force = (direction * knockbackForce) + (Vector3.up * upwardForce);

        if (scaleWithSpeed && cartRb != null)
        {
            float speed = cartRb.linearVelocity.magnitude;
            force *= Mathf.Clamp(speed, 0.5f, 3f);
        }

        rb.AddForce(force, ForceMode.Impulse);
    }
}


