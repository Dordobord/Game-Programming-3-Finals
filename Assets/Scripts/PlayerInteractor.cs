using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float rangeInteraction = 3f;
    [SerializeField] private float sphereRadius = 0.2f;
    [SerializeField] private LayerMask interactMask;

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] public Transform holdPoint;
    [SerializeField] private TextMeshProUGUI promptTextUI;

    [Header("Item Settings")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float holdLerpSpeed = 20f;

    private PlayerMovement playerMovement;
    private MouseLook mouseLook;

    private IInteractable currentInteractable;

    private ShoppingItem heldItem;

    private CartPushInteractable currentCart;
    private bool controllingCart = false;
    private Transform pushPointRef;

    private bool pushingStarted = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (cam == null) 
            cam = Camera.main;
        mouseLook = cam.GetComponent<MouseLook>();
    }

    private void Update()
    {
        if (controllingCart)
        {
            HandleCartPushing();
            return;
        }

        DetectInteractables();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem != null)
            {
                ThrowItem();
            }
            else if (currentInteractable != null)
            {
                currentInteractable.Interact(this);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            DropItem();
        }

        if (heldItem != null)
        {
            heldItem.FollowHoldPoint(holdPoint, holdLerpSpeed);
        }
    }

    private void DetectInteractables()
    {
        if (heldItem != null)
        {
            ShowPrompt("[E] Throw");
            currentInteractable = null;
            return;
        }

        currentInteractable = null;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, rangeInteraction, interactMask))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                ShowPrompt($"[E] {interactable.GetDescription()}");
                return;
            }
        }

        HidePrompt();
    }

    public bool TryPickupItem(ShoppingItem item)
    {
        if (heldItem != null) return false;
        if (controllingCart) return false;

        heldItem = item;
        item.OnPickup(this);
        ShowPrompt("[E] Throw");
        return true;
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        heldItem.OnDrop();
        heldItem = null;
        HidePrompt();
    }

    public void ThrowItem()
    {
        if (heldItem == null) return;

        Vector3 velocity = cam.transform.forward * throwForce;
        heldItem.OnThrow(velocity);
        heldItem = null;
        HidePrompt();
    }
    public void StartPushingCart(CartPushInteractable cart, Transform pushPoint)
    {
        if (heldItem != null) return;

        currentCart = cart;
        controllingCart = true;
        pushPointRef = pushPoint;

        playerMovement.enabled = false;
        mouseLook.enabled = true;

        transform.position = pushPoint.position;

        currentCart.cartController.isControlled = true;
        currentCart.cartController.rb.WakeUp();

        pushingStarted = true;

        HidePrompt();
    }

    public void StopPushingCart()
    {
        controllingCart = false;
        playerMovement.enabled = true;

        if (currentCart != null)
            currentCart.cartController.isControlled = false;

        pushingStarted = false;
        currentCart = null;
        pushPointRef = null;

        cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x,0f,0f);

        HidePrompt();
    }

    private void HandleCartPushing()
    {
        if (!pushingStarted) return;

        if (pushPointRef != null)
        {
            transform.position = pushPointRef.position;

            Vector3 direction = currentCart.transform.right;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            float pitch = cam.transform.localRotation.eulerAngles.x;
            Quaternion targetRot = Quaternion.LookRotation(direction);

            cam.transform.rotation = Quaternion.Euler(pitch,targetRot.eulerAngles.y,0f);
        }

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        currentCart.cartController.SetInput(vertical, horizontal);

        ShowPrompt("[E] Stop Pushing");

        if (Input.GetKeyDown(KeyCode.E))
        {
            StopPushingCart();
        }
    }

    private void ShowPrompt(string msg)
    {
        if (!promptTextUI) return;
        promptTextUI.text = msg;
        promptTextUI.gameObject.SetActive(true);
    }

    private void HidePrompt()
    {
        if (!promptTextUI) return;
        promptTextUI.text = "";
        promptTextUI.gameObject.SetActive(false);
    }
}


