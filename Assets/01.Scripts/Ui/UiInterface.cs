using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlot
{
    public int itemId { get; set; }
    public bool SetItem(int _itemId); 
}