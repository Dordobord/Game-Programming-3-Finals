using UnityEngine;
using TMPro;
using System.Collections;

public class UIBounce : MonoBehaviour
{
    public float sizeIncrease = 20f;
    public float speed = 10f;

    private TextMeshProUGUI tmp;
    private float originalSize;
    private Coroutine routine;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        originalSize = tmp.fontSize;
    }

    public void Play()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Bounce());
    }

    private IEnumerator Bounce()
    {
        float target = originalSize + sizeIncrease;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            tmp.fontSize = Mathf.Lerp(originalSize, target, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            tmp.fontSize = Mathf.Lerp(target, originalSize, t);
            yield return null;
        }

        tmp.fontSize = originalSize;
        routine = null;
    }
}
