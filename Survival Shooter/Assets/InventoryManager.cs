using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    public InventoryItem currentHeldItem;
    public InventoryCell currentHighlightedCell;

    public InventoryGrid activeInventoryGrid;
    public InventoryGrid playerInventoryGrid;

    public bool dragging = false;

    private void Awake()
    {

        instance = this;
        Debug.Log("IMI");

    }

}
