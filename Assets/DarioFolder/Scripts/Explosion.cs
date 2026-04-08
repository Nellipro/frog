using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public float destroyDelay = 0.5f; // How long the explosion stays visible

    void Start()
    {
        Destroy(gameObject, destroyDelay);
        Debug.Log("fucking died");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing we touched is Player 2
        if (other.CompareTag("player 2"))
        {
            Debug.Log("Explosion hit Player 2!");
            Destroy(other.gameObject);
        }
    }

    // Overload for 2D games
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player 2"))
        {
            Debug.Log("Explosion hit Player 2 (2D)!");
            Destroy(other.gameObject);
        }
    }
}