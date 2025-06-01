using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDropGenerator
{
    public static List<FarmingData> GetRandomDrop()
    {

        int spawnItemCount = -1;
        int ranItemCount = Random.Range(1, 101);

        if (ranItemCount <= 15) { spawnItemCount = 1; } // 15%로 1개
        else if (ranItemCount <= 50) { spawnItemCount = 2; } // 35%로 2개
        else if (ranItemCount <= 65) { spawnItemCount = 3; } // 35%로 3개
        else if (ranItemCount <= 100) { spawnItemCount = 4; } // 15%로 4개
        else { DebugHelper.Log("터지면 안되는 이상한 버그 터짐"); }

        List<FarmingData> dropList = new List<FarmingData>(spawnItemCount);

        int[] spawnItemAmount = new int[spawnItemCount];
        for (int i = 0; i < spawnItemCount; i++)
        {
            int ranItemAmount = Random.Range(1, 101);
            if (ranItemAmount <= 13) { spawnItemAmount[i] = 1; } // 13%로 1개
            else if (ranItemAmount <= 33) { spawnItemAmount[i] = 2; } // 20%로 2개
            else if (ranItemAmount <= 53) { spawnItemAmount[i] = 3; } // 20%로 3개
            else if (ranItemAmount <= 65) { spawnItemAmount[i] = 4; } // 12%로 4개
            else if (ranItemAmount <= 75) { spawnItemAmount[i] = 5; } // 10%로 5개
            else if (ranItemAmount <= 82) { spawnItemAmount[i] = 6; } // 7%로 6개
            else if (ranItemAmount <= 88) { spawnItemAmount[i] = 7; } // 6%로 7개
            else if (ranItemAmount <= 93) { spawnItemAmount[i] = 8; } // 5%로 8개
            else if (ranItemAmount <= 97) { spawnItemAmount[i] = 9; } // 4%로 9개
            else if (ranItemAmount <= 100) { spawnItemAmount[i] = 10; } // 3%로 10개
            else { DebugHelper.Log("터지면 안되는 이상한 버그 터짐"); }
        }

        List<int> itemKeys = new List<int>();

        var keys = ItemManager.Instance.GetAllKeys();
        foreach (int key in keys)
        {
            if (key >= 301) { itemKeys.Add(key); }
            else { continue; }
        }

        HashSet<int> ItemIds = new HashSet<int>();
        do
        {
            int ranNum = Random.Range(0, itemKeys.Count);
            ItemIds.Add(itemKeys[ranNum]);

        } while (ItemIds.Count < spawnItemCount);

        HashSet<int> SlotNums = new HashSet<int>();
        do
        {
            int ranNum = Random.Range(0, 30);
            SlotNums.Add(ranNum);

        } while (SlotNums.Count < spawnItemCount);
        
        int[] itemIdArray = ItemIds.ToArray();
        int[] slotNumsArray = SlotNums.ToArray();

        for (int j = 0; j < spawnItemCount; j++)
        {
            FarmingData dummy = new FarmingData(itemIdArray[j], spawnItemAmount[j], slotNumsArray[j]);
            dropList.Add(dummy);
        }
        return dropList;
    }
}
