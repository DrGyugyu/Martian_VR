using Photon.Pun;
using TMPro;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    private Item item;
    [SerializeField] private TextMeshPro itemText;
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        GameObject itemSpawn = PhotonNetwork.Instantiate(item.GetItemObj().name, position, Quaternion.identity);
        ItemWorld itemWorld = itemSpawn.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }
    public void SetItem(Item item)
    {
        this.item = item;
        itemText.SetText(item.ToString());
    }
    public Item GetItem()
    {
        return this.item;
    }
    public void DestroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    // public static ItemWorld DropItem(Vector3 playerPos, Item item)
    // {
    //     Vector3 dropPos = playerPos - Vector3.forward;
    //     ItemWorld itemWorld = SpawnItemWorld(dropPos, item);
    //     // int forcedir = 2 * UnityEngine.Random.Range(0, 2) - 1;
    //     // itemWorld.GetComponent<Rigidbody>().AddForce(2 * Vector3.one * forcedir, ForceMode.Impulse);
    //     return itemWorld;
    // }
    public TextMeshPro GetItemText()
    {
        return itemText;
    }
}