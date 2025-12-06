using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Attributes")]
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] private int minItems = 1;
    [SerializeField] private int maxItems = 6;

    [Header("Spawnpoints")]
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        SpawnRandomItems();
    }

    private void SpawnRandomItems()
    {
        int numberOfItemsToSpawn = Random.Range(minItems, maxItems + 1);
        numberOfItemsToSpawn = Mathf.Clamp(numberOfItemsToSpawn, 0, spawnPoints.Length);
        ShuffleSpawnPoints();

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            int itemIndex = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[itemIndex], spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }

    private void ShuffleSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Length);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }
    }
}
