using UnityEngine;

public class Luckyblock : MonoBehaviour
{
    public PlayerInventory.ItemType[] availableItems = { PlayerInventory.ItemType.SpeedBoost, PlayerInventory.ItemType.JumpBoost, PlayerInventory.ItemType.Shield };

    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            PlayerInventory.ItemType item = availableItems[Random.Range(0, availableItems.Length)];
            inventory.GiveItem(item);
            gameObject.SetActive(false);
            Invoke("Respawn", 10f);
        }
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }
}
