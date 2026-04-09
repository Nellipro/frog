using UnityEngine;

public class LuckBlock : MonoBehaviour
{
    [Header("Spawn Options")]
    public GameObject[] spawnOptions = new GameObject[5];
    public float[] spawnChances = new float[5] { 1f, 1f, 1f, 1f, 1f };
    public string activatorTag = "Player";
    public bool destroyOnSpawn = true;

    public void SpawnRandomItem()
    {
        GameObject prefab = ChooseRandomPrefab();
        if (prefab == null)
        {
            Debug.LogWarning("LuckBlock: No valid prefab selected to spawn.");
            return;
        }

        Instantiate(prefab, transform.position, Quaternion.identity);
        if (destroyOnSpawn)
            Destroy(gameObject);
    }

    private GameObject ChooseRandomPrefab()
    {
        if (spawnOptions == null || spawnOptions.Length == 0)
            return null;

        if (spawnChances == null || spawnChances.Length != spawnOptions.Length)
            return spawnOptions[Random.Range(0, spawnOptions.Length)];

        float totalWeight = 0f;
        for (int i = 0; i < spawnChances.Length; i++)
        {
            if (spawnChances[i] > 0f)
                totalWeight += spawnChances[i];
        }

        if (totalWeight <= 0f)
            return spawnOptions[Random.Range(0, spawnOptions.Length)];

        float randomValue = Random.Range(0f, totalWeight);
        float runningTotal = 0f;

        for (int i = 0; i < spawnOptions.Length; i++)
        {
            runningTotal += spawnChances[i];
            if (randomValue <= runningTotal)
                return spawnOptions[i];
        }

        return spawnOptions[spawnOptions.Length - 1];
    }

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned)
            return;

        if (other.CompareTag(activatorTag))
        {
            hasSpawned = true;
            SpawnRandomItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned)
            return;

        if (other.CompareTag(activatorTag))
        {
            hasSpawned = true;
            SpawnRandomItem();
        }
    }
}
