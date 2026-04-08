using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    public float speed = 5f;
    public int rightDistance = 5;

    private Transform player;
    private Vector3 startPos;

    private enum State
    {
        GoingUp,
        GoingRight,
        GoingToPlayer
    }

    private State currentState = State.GoingUp;

    void Start()
    {
        startPos = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("player 2");

        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.GoingUp:
                transform.Translate(Vector3.up * speed * Time.deltaTime);

                if (transform.position.y >= startPos.y + 3f)
                {
                    currentState = State.GoingRight;
                }
                break;

            case State.GoingRight:
                transform.Translate(Vector3.right * speed * Time.deltaTime);

                if (transform.position.x >= startPos.x + rightDistance)
                {
                    currentState = State.GoingToPlayer;
                }
                break;

            case State.GoingToPlayer:
                if (player == null) return;

                Vector3 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * speed * Time.deltaTime);
                break;
        }
    }
}