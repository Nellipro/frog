using UnityEngine;

public class RocketDirection : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public GameObject prefabToSpawn; // MAKE SURE TO DRAG YOUR PREFAB HERE IN THE INSPECTOR
    
    private Vector3 targetPosition;
    private bool targetLocked = false;
    private bool hasSpawned = false;
    private int phase = 0;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Phase 0: Move Up for 1 second
        if (phase == 0) 
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (timer > 1f) phase = 1; 
        }
        // Phase 1: Move Right & Find Player
        else if (phase == 1) 
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            
            GameObject player = GameObject.FindGameObjectWithTag("player 2");
            if (player != null) 
            {
                targetPosition = player.transform.position;
                targetLocked = true;
                phase = 2;
                Debug.Log("Target Locked at: " + targetPosition);
            }
        }
        // Phase 2: Move to the LOCKED location
        else if (phase == 2 && targetLocked)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Distance check to trigger activation
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                ActivateEffect();
            }
        }
    }

    void ActivateEffect()
    {
        // Prevent double-spawning
        if (hasSpawned) return;
        hasSpawned = true;

        if (prefabToSpawn != null)
        {
            // Spawns the prefab at the current position with no rotation
            GameObject spawnedObj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            
            // Debug to confirm it worked
            Debug.Log("Prefab spawned successfully: " + spawnedObj.name);
        }
        else
        {
            Debug.LogError("PREFAB MISSING: You forgot to drag the prefab into the inspector slot on " + gameObject.name);
        }

        Destroy(gameObject);
    }

    // Safety collision check
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player 2")) ActivateEffect();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player 2")) ActivateEffect();
    }
}