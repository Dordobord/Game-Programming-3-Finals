/* using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterScanner : MonoBehaviour
{
    [Header("Counter Panel Attributes")]
    [SerializeField] private GameObject panel3D;
    [SerializeField] private Transform listContainer;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI requirementText;
    [SerializeField] private GameObject listEntryPrefab;

    private List<ShoppingItem> items = new List<ShoppingItem>();
    private float total = 0f;
    private int requirement;
    private bool isCompleted = false;

    private void Start()
    {
        requirement = Random.Range(500, 1001);
        requirementText.text = $"Goal: ${requirement}";
        panel3D.SetActive(false);
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        ShoppingItem item = other.GetComponent<ShoppingItem>();
        if (item == null) return;

        if (items.Contains(item)) return;

        items.Add(item);
        total += item.Price;
        requirement -= Mathf.RoundToInt(item.Price);

        other.enabled = false;

        StartCoroutine(DeleteItemDelay(item.gameObject));
        UpdateUI();

        if (requirement <= 0)
            FinishScanning();
    }

    private void FinishScanning()
    {
        isCompleted = true;
        requirementText.text = "Goal Reached!";

        GameManager.Instance.EndGame(true);
    }

    private IEnumerator DeleteItemDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
    }

    private void UpdateUI()
    {

        panel3D.SetActive(items.Count > 0);

        foreach (Transform child in listContainer)
            Destroy(child.gameObject);

        foreach (ShoppingItem item in items)
        {
            var entry = Instantiate(listEntryPrefab, listContainer);
            entry.GetComponent<TextMeshProUGUI>().text = $"{item.ItemName} - ${item.Price:F2}";
        }

        totalText.text = $"Total: ${total:F2}";
        requirementText.text = $"Goal: ${Mathf.Max(requirement, 0)}";
    }
}
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterScanner : MonoBehaviour
{
    [Header("Counter Panel Attributes")]
    [SerializeField] private GameObject panel3D;
    [SerializeField] private Transform listContainer;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI requirementText;
    [SerializeField] private GameObject listEntryPrefab;

    private List<ShoppingItem> items = new List<ShoppingItem>();
    private float total = 0f;
    private int requirement;
    private bool isCompleted = false;

    private UIBounce requirementBounce;

    private void Start()
    {
        requirement = Random.Range(500, 1001);

        requirementBounce = requirementText.GetComponent<UIBounce>();

        SetRequirementText($"Goal: ${requirement}");

        panel3D.SetActive(false);

        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        ShoppingItem item = other.GetComponent<ShoppingItem>();
        if (item == null) return;

        if (items.Contains(item)) return;

        AudioManager.main.PlaySFX("Beep");

        items.Add(item);
        total += item.Price;
        requirement -= Mathf.RoundToInt(item.Price);

        other.enabled = false;

        StartCoroutine(DeleteItemDelay(item.gameObject));
        UpdateUI();

        if (requirement <= 0)
            FinishScanning();
    }

    private void FinishScanning()
    {
        isCompleted = true;

        SetRequirementText("Goal Reached!");

        GameManager.Instance.goalReached = true;

        GameManager.Instance.EndGame(true);
    }

    private IEnumerator DeleteItemDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
    }

    private void UpdateUI()
    {
        panel3D.SetActive(items.Count > 0);

        foreach (Transform child in listContainer)
            Destroy(child.gameObject);

        foreach (ShoppingItem item in items)
        {
            var entry = Instantiate(listEntryPrefab, listContainer);
            entry.GetComponent<TextMeshProUGUI>().text = $"{item.ItemName} - ${item.Price:F2}";
        }

        totalText.text = $"Total: ${total:F2}";

        SetRequirementText($"Goal: ${Mathf.Max(requirement, 0)}");
    }

    private void SetRequirementText(string value)
    {
        requirementText.text = value;
        requirementBounce.Play();
    }
}
