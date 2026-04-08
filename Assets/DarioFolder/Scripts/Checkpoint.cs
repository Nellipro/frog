using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = true;
    public RespawnManager respawnManager;

    void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;

        if (respawnManager == null)
        {
            Debug.LogWarning("Checkpoint has no RespawnManager assigned.");
            return;
        }

        if (other.CompareTag("Player1"))
        {
            respawnManager.UpdatePlayer1Respawn(transform.position);
            Debug.Log("Checkpoint updated Player1 respawn to: " + transform.position);
        }
        else if (other.CompareTag("Player2"))
        {
            respawnManager.UpdatePlayer2Respawn(transform.position);
            Debug.Log("Checkpoint updated Player2 respawn to: " + transform.position);
        }
    }
}
