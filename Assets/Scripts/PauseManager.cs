using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        playerMovement.canMove = false;
        mouseLook.canLook = false;

        MouseLock.Instance.UnlockMouse();
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        playerMovement.canMove = true;
        mouseLook.canLook = true;

        MouseLock.Instance.LockMouse();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
