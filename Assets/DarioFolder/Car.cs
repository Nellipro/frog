using UnityEngine;

public class MoveAndReset : MonoBehaviour
{
    public float speed = 2f;
    public float moveDuration = 2f;

    private Vector3 startPosition;
    private float timer;

    void Start()
    {
        startPosition = transform.position;
        timer = moveDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Move left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // When time is up, reset
        if (timer <= 0f)
        {
            transform.position = startPosition;
            timer = moveDuration;
        }
    }
}