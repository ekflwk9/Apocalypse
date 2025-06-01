using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour, IInteractionObject
{
    private List<FarmingData> item = new List<FarmingData>();
    private Animator anim;
    private bool isOpen;
    [SerializeField] SelectedUI selectedUI;


#if UNITY_EDITOR
    [SerializeField] private int[] id;
    [SerializeField] private int[] count;
    [SerializeField] private int[] slotNumber;
#endif

    private void Awake()
    {
        item = ItemDropGenerator.GetRandomDrop();
        anim = this.TryGetComponent<Animator>();

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
        if (!isOpen) anim.Play("Open", 0, 0);

        if (!UiManager.instance.isActive)
        {
            var status = UiManager.instance.status;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            UiManager.instance.SetActive(true);
            status.SetFarming(item, UpdateData);
            status.farming.gameObject.SetActive(true);
            status.inventory.gameObject.SetActive(true);
            status.equipped.gameObject.SetActive(true);
        }
    }

    public void UpdateData()
    {
        item = UiManager.instance.status.farmingData;
    }

    public void OnSelected()
    {
        if (selectedUI != null)
        {
            selectedUI.On();
        }
    }
    public void UnSelected()
    {
        if (selectedUI != null)
        {
            selectedUI.Off();
        }
    }
}
