using UnityEngine;
public class KeyRebinder : MonoBehaviour
{
    public string currentKey = "None";

    public Vector3 upMovement = new Vector3(0, 1, 0);
    public Vector3 downMovement = new Vector3(0, -1, 0);
    public Vector3 leftMovement = new Vector3(-1, 0, 0);
    public Vector3 rightMovement = new Vector3(1, 0, 0);
    public GameObject frog;
    public GameObject Camera;
    public GameObject signPrefab;
    public float rigidbodyDelay = 0.5f;

    private bool movementAllowed = true;
    private bool rigidbodyPending = false;
    private GameObject spawnedSign;
    private Quaternion originalRotation;

    void Start()
    {
        if (frog != null && frog.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = frog.AddComponent<Rigidbody>();
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
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    currentKey = key.ToString();
                    Debug.Log("New key set: " + currentKey);
                    KeyHasBeenSet();
                    break;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (frog != null)
        {
            Vector3 scale = frog.transform.localScale;
            frog.transform.localScale = new Vector3(scale.x, 0.1f, scale.z);
            Debug.Log("Frog hit something: " + collision.gameObject.name);
        }
    }

    void KeyHasBeenSet()
    {
        if (currentKey == "None")
        {
            return;
        }

        if (currentKey == "W")
        {
            Physics.Raycast(frog.transform.position, Vector3.forward, out RaycastHit forwardHit, 1f);
            if (forwardHit.collider == null)
            {
                frog.transform.position += upMovement;
                Camera.transform.position += upMovement;
                nothingunderneath();
            }
            else
            {
                Debug.Log("Cannot move up, something is blocking the way!");
            }
        }
        else if (currentKey == "A")
        {
            Physics.Raycast(frog.transform.position, Vector3.left, out RaycastHit leftHit, 1f);
            if (leftHit.collider == null)
            {
                frog.transform.position += leftMovement;
                Camera.transform.position += leftMovement;
                nothingunderneath();
            }
            else
            {
                Debug.Log("Cannot move left, something is blocking the way!");
            }
        }
        else if (currentKey == "S")
        {
            Physics.Raycast(frog.transform.position, Vector3.back, out RaycastHit backHit, 1f);
            if (backHit.collider == null)
            {
                frog.transform.position += downMovement;
                Camera.transform.position += downMovement;
                nothingunderneath();
            }
            else
            {
                Debug.Log("Cannot move down, something is blocking the way!");
            }
        }
        else if (currentKey == "D")
        {
            Physics.Raycast(frog.transform.position, Vector3.right, out RaycastHit rightHit, 1f);
            if (rightHit.collider == null)
            {
                frog.transform.position += rightMovement;
                Camera.transform.position += rightMovement;
                nothingunderneath();
            }
            else
            {
                Debug.Log("Cannot move right, something is blocking the way!");
            }
        }
    }

    void nothingunderneath()
    {
        Physics.Raycast(frog.transform.position, Vector3.down, out RaycastHit downHit, 1f);
        if (downHit.collider == null && !rigidbodyPending)
        {
            StartCoroutine(AddRigidbodyAfterDelay(rigidbodyDelay));
        }
    }

    private System.Collections.IEnumerator AddRigidbodyAfterDelay(float delay)
    {
        movementAllowed = false;
        rigidbodyPending = true;

        if (frog == null)
        {
            yield break;
        }

        originalRotation = frog.transform.rotation;
        Quaternion downRotation = Quaternion.Euler(originalRotation.eulerAngles + new Vector3(-90f, 0f, 0f));
        
        // Smooth rotation down
        float rotationDuration = 0.5f;
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            frog.transform.rotation = Quaternion.Lerp(originalRotation, downRotation, t);
            yield return null;
        }
        frog.transform.rotation = downRotation;

        yield return new WaitForSeconds(0.25f);

        // Smooth rotation back up
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            frog.transform.rotation = Quaternion.Lerp(downRotation, originalRotation, t);
            yield return null;
        }
        frog.transform.rotation = originalRotation;

        if (signPrefab != null)
        {
            spawnedSign = Instantiate(signPrefab, frog.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }

        yield return new WaitForSeconds(2f);

        if (spawnedSign != null)
        {
            Destroy(spawnedSign);
        }

        yield return new WaitForSeconds(delay);

        if (frog != null)
        {
            Rigidbody rb = frog.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        rigidbodyPending = false;
        movementAllowed = false;
    }
}
