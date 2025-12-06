using UnityEngine;

public class MouseLock : MonoBehaviour
{
    public static MouseLock Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
