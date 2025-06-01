using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour, IInteractionObject
{
    private List<FarmingData> item = new List<FarmingData>();

#if UNITY_EDITOR
    [SerializeField] private int[] id;
    [SerializeField] private int[] count;
    [SerializeField] private int[] slotNumber;
#endif

    private void Awake()
    {
        item = ItemDropGenerator.GetRandomDrop();

#if UNITY_EDITOR
        id = new int[item.Count];
        count = new int[item.Count];
        slotNumber = new int[item.Count];
        for (int i = 0; i < item.Count; i++)
        {
            id[i] = item[i].id;
            count[i] = item[i].count;
            slotNumber[i] = item[i].slotNumber;
        }
#endif
    }

    public void Interaction() // 캐바넷 주을때 호출
    {
        var status = UiManager.instance.status;

        status.SetFarming(item, UpdateData);
        status.farming.gameObject.SetActive(true);
        status.inventory.gameObject.SetActive(true);
        status.equipped.gameObject.SetActive(true);
    }

    public void UpdateData()
    {
        item = UiManager.instance.status.farmingData;
    }

    public void OnSelected()
    {

    }

    public void UnSelected()
    {

    }

}
