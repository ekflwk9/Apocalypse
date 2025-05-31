using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    [SerializeField] private AssetData dataBundle;

    private Dictionary<int, ItemInfo> itemDB = new Dictionary<int, ItemInfo>();
    public Inventory _inventory { get; private set; }
    public Inventory Inventory => _inventory ??= Player.Instance.GetComponent<Inventory>();

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
    /// 특정 인덱스의 아이템 정보를 가져오는 예외처리된 메서드
    /// </summary>
    public ItemInfo GetItem(int _id)
    {
        if (itemDB.ContainsKey(_id)) return itemDB[_id];
        else DebugHelper.Log($"{_id}번 아이템은 존재하지 않는 아이템");

        return null;
    }

    public Dictionary<int, ItemInfo>.KeyCollection GetAllKeys()
    {
        if (itemDB.Count == 0) DebugHelper.Log("아이템이 현재 한개도 추가되지 않은 상태");
        return itemDB.Keys;
    }
}