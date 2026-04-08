using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocket;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rocket != null)
            {
                Instantiate(rocket, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        
        // Destroy the object that held this script
        
    }
}