using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlot
{
    public int itemId { get; }
    public int count { get; }
    public void SetSlot(int _itemCount);
    public bool SetSlot(int _itemId, int _itemCount);
}