using Unity.VisualScripting;
using UnityEngine;

public class OreScript : MonoBehaviour
{
    [SerializeField] private int oreHealth;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Item"))
        {
            GameMgr.Instance.GetInventoryOrigin().AddItem(new Item { itemSO = InventoryUI.Instance.oreSO, amount = 1 });
        }
        oreHealth--;
        if (oreHealth <= 0)
        {
            Destroy(this.gameObject);//WIP
        }
    }
}
