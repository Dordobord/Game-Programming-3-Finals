using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIIntro : MonoBehaviour
{
    public RectTransform introText;
    public RectTransform targetPos;

    public GameObject timerUI;
    public float delayBeforeMove = 0.5f;
    public float duration = 1f;

    private CanvasGroup _cg;

    private Vector2 startPos;
    private Vector2 endPos;

    private Vector3 startScale;
    private Vector3 endScale;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();

        _cg.alpha = 0f;

        startPos = introText.anchoredPosition;
        endPos = targetPos.anchoredPosition;

        startScale = introText.localScale;
        endScale = Vector3.one * 0.4f;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateIntro());
    }

    private IEnumerator AnimateIntro()
    {
        _cg.alpha = 1f;

        yield return new WaitForSeconds(delayBeforeMove);

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

        timerUI.SetActive(true);   
        GameManager.Instance.timerRunning = true;   
    }
}
