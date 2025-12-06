using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Timer")]
    public float gameTime = 60f;
    [HideInInspector] public bool timerRunning = false;
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("Player Control")]
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;

    [Header("Panels")]
    public GameObject endPanel;
    public TextMeshProUGUI endMessage;

    [HideInInspector]
    public bool goalReached = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentTime = gameTime;
        endPanel.SetActive(false);
        UpdateTimerUI();
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame(goalReached);
        }
    }

    private void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 30)
            timerText.color = Color.red;
    }

    public void StartGame()
    {
        currentTime = gameTime;
        goalReached = false;

        playerMovement.canMove = true;
        mouseLook.canLook = true;

        timerText.gameObject.SetActive(false);  
        timerRunning = false;                    

        MouseLock.Instance.LockMouse();
    }

    public void EndGame(bool success)
    {
        timerRunning = false;

        playerMovement.canMove = false;
        mouseLook.canLook = false;

        MouseLock.Instance.UnlockMouse();

        endPanel.SetActive(true);
        endMessage.text = success ? "CONGRATULATIONS!" : "Time's Up!";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
