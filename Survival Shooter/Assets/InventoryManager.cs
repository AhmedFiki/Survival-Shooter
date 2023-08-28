using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            activeInventoryGrid.GetNextCells(currentHighlightedCell,3);
        }if (Input.GetKeyDown(KeyCode.R))
        {
            activeInventoryGrid.GetLowerCells(currentHighlightedCell,3);
        }if (Input.GetKeyDown(KeyCode.Y))
        {
           LogListContents( activeInventoryGrid.FindEmptySpace(new Vector2Int(3,3)));
            
        }
    }
    public void LogListContents<T>(List<T> list)
    {
        string logMessage = "List Contents:";
        foreach (T item in list)
        {
            logMessage += "\n" + item.ToString();
        }
        logMessage += "\n" +list.Count;

        Debug.Log(logMessage);
    }
}
