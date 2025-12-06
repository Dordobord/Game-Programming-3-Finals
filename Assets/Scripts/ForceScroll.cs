using UnityEngine;
using UnityEngine.UI;

public class ForceScroll : MonoBehaviour
{
    private ScrollRect scroll;

    void Start()
    {
        scroll = GetComponent<ScrollRect>();
    }

    void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0)
        {
            scroll.verticalNormalizedPosition += scrollInput * 0.05f;
        }
    }
}
