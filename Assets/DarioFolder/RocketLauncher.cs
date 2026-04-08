using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Rocket Settings")]
    public GameObject rocket;
    public Transform spawnPoint;
    public int rightDistance = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

            FlyingObject moveScript = obj.GetComponent<FlyingObject>();
            moveScript.rightDistance = rightDistance;
        }
    }
}