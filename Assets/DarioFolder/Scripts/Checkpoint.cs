using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = true;
    public RespawnManager respawnManager;

    void OnTriggerEnter(Collider other)
    {
        if (isActive && respawnManager != null)
        {
            if (other.CompareTag("player 1"))
            {
                respawnManager.UpdatePlayer1Respawn(transform.position);
            }
            else if (other.CompareTag("player 2"))
            {
                respawnManager.UpdatePlayer2Respawn(transform.position);
            }
        }
    }
}
