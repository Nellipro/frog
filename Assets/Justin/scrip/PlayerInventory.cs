using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum ItemType { None, SpeedBoost, JumpBoost, Shield }

    public ItemType currentItem = ItemType.None;
    public KeyCode useItemKey = KeyCode.Space;
    public float effectDuration = 5f;
    public Color jumpBoostColor = Color.red;

    void Update()
    {
        if (currentItem != ItemType.None && Input.GetKeyDown(useItemKey))
        {
            UseItem();
        }
    }

    public void GiveItem(ItemType item)
    {
        if (currentItem != ItemType.None)
        {
            Debug.Log(name + " already has an item: " + currentItem);
            return;
        }

        currentItem = item;
        Debug.Log(name + " picked up " + item);
    }

    void UseItem()
    {
        Debug.Log(name + " used " + currentItem);

        switch (currentItem)
        {
            case ItemType.SpeedBoost:
                transform.localScale *= 1.2f;
                StartCoroutine(ResetScale(effectDuration));
                break;
            case ItemType.JumpBoost:
                Renderer rend = GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = jumpBoostColor;
                    StartCoroutine(ResetColor(rend, effectDuration));
                }
                break;
            case ItemType.Shield:
                Debug.Log(name + " shield is active for " + effectDuration + " seconds.");
                break;
        }

        currentItem = ItemType.None;
    }

    System.Collections.IEnumerator ResetScale(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.localScale /= 1.2f;
    }

    System.Collections.IEnumerator ResetColor(Renderer rend, float delay)
    {
        yield return new WaitForSeconds(delay);
        rend.material.color = Color.white;
    }
}
