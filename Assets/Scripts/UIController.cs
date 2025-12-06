using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    [Header("Start Menu")]
    public GameObject instructionsPanel;
    public Button startButton;

    [Header("Requirement Animation")]
    public RectTransform introParent;   
    public RectTransform introText;    
    public RectTransform targetPos;

    public float duration = 1f;

    private CanvasGroup introCg;

    private Vector2 startPos;
    private Vector2 endPos;

    private Vector3 startScale;
    private Vector3 endScale;

    private void Awake()
    {
        introCg = introText.GetComponent<CanvasGroup>();   

        introCg.alpha = 0f;   

        startPos = introText.anchoredPosition;
        endPos = targetPos.anchoredPosition;

        startScale = introText.localScale;
        endScale = Vector3.one * 0.4f;
    }

    private void Start()
    {
        instructionsPanel.SetActive(true);
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        instructionsPanel.SetActive(false);
        StartCoroutine(AnimateIntro());
        GameManager.Instance.StartGame();
    }

    private IEnumerator AnimateIntro()
    {
        introCg.alpha = 1f;  

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            introText.anchoredPosition = Vector2.Lerp(startPos, endPos, p);
            introText.localScale = Vector3.Lerp(startScale, endScale, p);

            yield return null;
        }

        introText.anchoredPosition = endPos;
        introText.localScale = endScale;
    }
}
