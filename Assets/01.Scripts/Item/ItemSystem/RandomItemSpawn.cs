using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomItemSpawn
{
    public int[] ItemId; // 아이템 Id
    public int Count; // 아이템이 몇개인지
    public int[] Amount; // 몇스택인지

    public RandomItemSpawn(int[] _itemId, int _count, int[] _amount)
    {
        ItemId = _itemId;
        Count = _count;
        Amount = _amount;
    }
}

public static class ItemDropGenerator
{
    public static List<RandomItemSpawn> GetRandomDrop(int boxAmount)
    {
        List<RandomItemSpawn> dropList = new List<RandomItemSpawn>(boxAmount);

        for (int j = 0; j < dropList.Count; j++)
        {
            int spawnItemCount = -1;
            int ranItemCount = Random.Range(1, 101);

            if (ranItemCount <= 15) { spawnItemCount = 1; } // 15%로 1개
            else if (ranItemCount <= 50) { spawnItemCount = 2; } // 35%로 2개
            else if (ranItemCount <= 65) { spawnItemCount = 3; } // 35%로 3개
            else if (ranItemCount <= 100) { spawnItemCount = 4; } // 15%로 4개
            else { DebugHelper.Log("터지면 안되는 이상한 버그 터짐"); }

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

            var keys = ItemManager.Instance.itemDB.Keys;
            foreach (int key in keys)
            {
                if (key >= 301) { itemKeys.Add(key); }
                else { continue; }
            }

            HashSet<int> usedItemIds = new HashSet<int>(spawnItemCount);
            do
            {
                int ranNum = Random.Range(0, itemKeys.Count);
                usedItemIds.Add(ranNum);

            } while (usedItemIds.Count >= spawnItemCount);

            dropList[j].ItemId = usedItemIds.ToArray();;
            dropList[j].Count = spawnItemCount;
            dropList[j].Amount = spawnItemAmount;
        }


        return dropList;
    }
}
