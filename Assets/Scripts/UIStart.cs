using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{
    public GameObject instructionsPanel;
    public UIIntro requirementIntro;
    public Button startButton;

    private void Start()
    {
        instructionsPanel.SetActive(true);
        requirementIntro.gameObject.SetActive(true);
        startButton.onClick.AddListener(OnStartPressed);
    }

    private void OnStartPressed()
    {
        AudioManager.main.PlayMusic("BGM");

        instructionsPanel.SetActive(false);

        requirementIntro.Play();

        GameManager.Instance.StartGame();
    }
}
