using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private AssetBundle dataBundle;
    public Inventory _inventory;
    public Inventory Inventory => _inventory ??= Player.Instance.GetComponent<Inventory>();
    public ItemEquipment itemEquipment;

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ItemManager").AddComponent<ItemManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            Addressables.LoadAssetsAsync<ItemInfo>
            (new AssetLabelReference() { labelString = "Item" }, item =>
            {
                itemDB[item.itemId] = item;
            }).WaitForCompletion();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PickUp(int itemId) // 플레이어가 아이템 주웠을때 호출되는 메서드
    {
        ItemInfo item;
        if (itemDB.ContainsKey(itemId))
        {
            item = itemDB[itemId];

            Inventory.GetItem(item);
        }
    }
    // public void UseItem(ItemInfo itemInfo)
    // {

    // }

    public Dictionary<int, ItemInfo> itemDB = new Dictionary<int, ItemInfo>();
}
