using UnityEngine;
public class Movement : MonoBehaviour
{
    public int playerNumber = 1;

    public Vector3 upMovement = new Vector3(0, 0, 1);
    public Vector3 downMovement = new Vector3(0, 0, -1);
    public Vector3 leftMovement = new Vector3(-1, 0, 0);
    public Vector3 rightMovement = new Vector3(1, 0, 0);
    public GameObject Camera;
    public GameObject signPrefab;
    public float rigidbodyDelay = 0.5f;
    public string ignoreLayerName = "";

    private bool movementAllowed = true;
    private bool rigidbodyPending = false;
    private GameObject spawnedSign;
    private Quaternion originalRotation;

    void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!movementAllowed)
        {
            return;
        }

        if (playerNumber == 1)
        {
            if (Input.GetKeyDown(KeyCode.W)) Move(upMovement, "up");
            if (Input.GetKeyDown(KeyCode.A)) Move(leftMovement, "left");
            if (Input.GetKeyDown(KeyCode.S)) Move(downMovement, "down");
            if (Input.GetKeyDown(KeyCode.D)) Move(rightMovement, "right");
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Move(upMovement, "up");
            if (Input.GetKeyDown(KeyCode.DownArrow)) Move(downMovement, "down");
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(leftMovement, "left");
            if (Input.GetKeyDown(KeyCode.RightArrow)) Move(rightMovement, "right");
        }
    }

    void Move(Vector3 direction, string dirName)
    {
        int layerMask = -1;
        if (!string.IsNullOrEmpty(ignoreLayerName))
        {
            int ignoreLayer = LayerMask.NameToLayer(ignoreLayerName);
            if (ignoreLayer != -1)
            {
                layerMask = ~(1 << ignoreLayer);
            }
        }
        Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, layerMask);
        if (hit.collider == null)
        {
            transform.position += direction;
            if (Camera != null) Camera.transform.position += direction;
            nothingunderneath();
        }
        else
        {
            Debug.Log("Cannot move " + dirName + ", something is blocking the way!");
        }
    }

    void nothingunderneath()
    {
        int layerMask = -1;
        if (!string.IsNullOrEmpty(ignoreLayerName))
        {
            int ignoreLayer = LayerMask.NameToLayer(ignoreLayerName);
            if (ignoreLayer != -1)
            {
                layerMask = ~(1 << ignoreLayer);
            }
        }
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit downHit, 1f, layerMask);
        if (downHit.collider == null && !rigidbodyPending)
        {
            StartCoroutine(AddRigidbodyAfterDelay(rigidbodyDelay));
        }
    }

    private System.Collections.IEnumerator AddRigidbodyAfterDelay(float delay)
    {
        movementAllowed = false;
        rigidbodyPending = true;

        originalRotation = transform.rotation;
        Quaternion downRotation = Quaternion.Euler(originalRotation.eulerAngles + new Vector3(-90f, 0f, 0f));
        
        // Smooth rotation down
        float rotationDuration = 0.5f;
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            transform.rotation = Quaternion.Lerp(originalRotation, downRotation, t);
            yield return null;
        }
        transform.rotation = downRotation;

        yield return new WaitForSeconds(0.25f);

        // Smooth rotation back up
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            transform.rotation = Quaternion.Lerp(downRotation, originalRotation, t);
            yield return null;
        }
        transform.rotation = originalRotation;

        if (signPrefab != null)
        {
            spawnedSign = Instantiate(signPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }

        yield return new WaitForSeconds(2f);

        if (spawnedSign != null)
        {
            Destroy(spawnedSign);
        }

        yield return new WaitForSeconds(delay);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        rigidbodyPending = false;
        movementAllowed = false;
    }
}
