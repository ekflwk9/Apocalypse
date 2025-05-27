using System.Collections;
using GameItem;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public List<Transform> itemSpawnPoints;
    public GameObject itemPrefab;
    
    public void SpawnRandomItem(int amount)
    {
        List<Transform> spawnPool = new List<Transform>(itemSpawnPoints);

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, spawnPool.Count);
            Transform point = spawnPool[randomIndex];

            ItemInfo item = ItemManager.Instance.itemDB[Random.Range(0, ItemManager.Instance.itemDB.Count)];

            GameObject dummyObj = Instantiate(itemPrefab, point.position, Quaternion.identity);
            dummyObj.GetComponent<ItemHandler>().itemInfo = item;
        }
    }
}
