using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Vector3 player1RespawnPosition = Vector3.zero;
    public Vector3 player2RespawnPosition = Vector3.zero;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Vector3 camera1Offset = new Vector3(0, 10, 0);
    public Vector3 camera1Rotation = new Vector3(90, 0, 0);
    public Vector3 camera2Offset = new Vector3(0, 10, 0);
    public Vector3 camera2Rotation = new Vector3(90, 0, 0);

    private float checkInterval = 1f;
    private float lastCheckTime;

    void Start()
    {
        lastCheckTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;
            CheckAndRespawnPlayers();
        }
    }

    public void UpdatePlayer1Respawn(Vector3 position)
    {
        player1RespawnPosition = position;
        Debug.Log("player 1 respawn updated to: " + position);
    }

    public void UpdatePlayer2Respawn(Vector3 position)
    {
        player2RespawnPosition = position;
        Debug.Log("player 2 respawn updated to: " + position);
    }

    void CheckAndRespawnPlayers()
    {
        if (GameObject.FindWithTag("player 1") == null && player1Prefab != null)
        {
            Instantiate(player1Prefab, player1RespawnPosition, Quaternion.identity);
            Debug.Log("Respawned player 1 at: " + player1RespawnPosition);
            GameObject cam = GameObject.FindWithTag("Camera1");
            if (cam != null)
            {
                cam.transform.position = player1RespawnPosition + camera1Offset;
                cam.transform.rotation = Quaternion.Euler(camera1Rotation);
                Debug.Log("Positioned Camera1 at: " + cam.transform.position + " with rotation: " + camera1Rotation);
            }
        }
        if (GameObject.FindWithTag("player 2") == null && player2Prefab != null)
        {
            Instantiate(player2Prefab, player2RespawnPosition, Quaternion.identity);
            Debug.Log("Respawned player 2 at: " + player2RespawnPosition);
            GameObject cam = GameObject.FindWithTag("Camera2");
            if (cam != null)
            {
                cam.transform.position = player2RespawnPosition + camera2Offset;
                cam.transform.rotation = Quaternion.Euler(camera2Rotation);
                Debug.Log("Positioned Camera2 at: " + cam.transform.position + " with rotation: " + camera2Rotation);
            }
        }
    }
}