using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemManager : MonoBehaviour
{
    private static System.Random random = new();
    [SerializeField] private AssetData dataBundle;
    public Inventory _inventory;
    public Inventory Inventory => _inventory ??= Player.Instance.GetComponent<Inventory>();

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


    /// <summary>
    /// 아이템 제거시 호출 요망
    /// </summary>
    /// <param name="itemId"></param>
    public void RemoveItem(int itemId)
    {
        Inventory.RemoveInventoryItem(itemId);
    }

    /// <summary>
    /// 아이템 사용시 호출 요망
    /// </summary>
    /// <param name="itemId"></param>
    public void UseItem(int itemId)
    {
        Inventory.UseInventoryItem(itemId);
    }

    /// <summary>
    /// 주아이템 등록시 호출 요망
    /// </summary>
    /// <param name="index"></param>
    public void SetItemSlot(int index, bool isFirst)
    {
        if (isFirst)
        {
            if (index == 0)
            {
                Inventory.firstSlotItem = null;
            }
            else
            {
                Inventory.firstSlotItem = Instance.itemDB[index];
            }

        }
        else
        {
            if (index == 0)
            {
                Inventory.secondSlotItem = null;
            }
            else
            {
                Inventory.secondSlotItem = Instance.itemDB[index];
            }
        }
    }

    /// <summary>
    /// 맵 아이템 생성 메서드
    /// </summary>
    /// <param name="position"></param>
    /// <param name="itemInfo"></param>
    /// <returns></returns>
    public ItemHandler SpawnItem(Vector3 position, ItemInfo itemInfo)
    {
        var Mapitem = Instantiate(itemInfo.itemPrefab, position, Quaternion.identity);

        return Mapitem.GetComponent<ItemHandler>();
    }
    
    public ItemInfo[] GetRandomItems(int amount)
    {
      var result = new ItemInfo[amount];
      var items = (from item in itemDB.Values orderby random.Next() select item).ToList();

      for (int i = 0; i < amount; i++)
      {
        result[i] = items[i % items.Count];
      }
      return result;
    }

    public Dictionary<int, ItemInfo> itemDB = new Dictionary<int, ItemInfo>();
}