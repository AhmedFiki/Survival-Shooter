using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public Vector2Int size;
    public Vector2 cellSize; // The size of each grid cell

    [SerializeField]
    List<InventoryCell> inventoryCells = new List<InventoryCell>();
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    InventoryManager inventoryManager;
    public List<InventoryCell> cellsWithinCorners = new List<InventoryCell>();

    bool constantHighlight = true;
    public bool mouseOverGrid = false;


    private void Start()
    {
        inventoryManager = InventoryManager.instance;
    }
    private void Update()
    {




        if (Input.GetKeyDown(KeyCode.E))
        {
            constantHighlight = !constantHighlight;
        }
       /* if (inventoryManager.currentHeldItem == null)
        {
            UnHighlightAllCells();
        }*/
        if (inventoryManager.currentHeldItem != null && inventoryManager.currentHeldItem.GetCorners() != null && constantHighlight)
        {
            Vector3[] c = inventoryManager.currentHeldItem.GetCorners(); 




            //Debug.Log(c[0] + " " + c[1] + " " + c[2] + " " + c[3]);
            cellsWithinCorners = GetCellsWithinCorners(c);
        }
    }


    private List<InventoryCell> GetCellsWithinCorners(Vector3[] corners)
    {
        cellsWithinCorners.Clear();
        // Determine the minimum and maximum x and y values from the corners
        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);

        foreach (InventoryCell cell in inventoryCells)
        {


            //  Debug.Log(cell.transform.position.x + " " + cell.transform.position.y + " ; " + corners[0] + " " + corners[2]);
            if (cell.transform.position.x >= corners[0].x && cell.transform.position.x <= corners[2].x &&
                cell.transform.position.y >= corners[0].y && cell.transform.position.y <= corners[2].y)
            {
                cell.Highlight();
                cellsWithinCorners.Add(cell);
            }
            else
            {
                cell.UnHighlight();
            }

        }
        
        
       // Debug.Log(cellsWithinCorners.Count + " cells within corners");
        return cellsWithinCorners;
    }
    private InventoryCell GetTopLeftCell(List<InventoryCell> cells)
    {
        int min = 99;
        int minIndex = 0;
        for (int i = 0; i < cells.Count; i++)
        {

            if (cells[i].position.x + cells[i].position.y < min)
            {
                minIndex = i;
                min = cells[i].position.x + cells[i].position.y;
            }


        }
        Debug.Log(cells.Count+"   "+minIndex);
        return cells[minIndex];
    }
    public void UnHighlightAllCells()
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.UnHighlight();
        }
    }

    public bool AttemptPlacement()
    {
        //GetCellsWithinCorners(inventoryManager.currentHeldItem.GetCorners());

        if (CanPlace())
        {
            inventoryManager.currentHeldItem.ResetOccupyingCells();

            Debug.Log("can place");
            PlaceItemInCell( inventoryManager.currentHeldItem);
            return true;
        }
        else
        {
            Debug.Log("Cannot place x");
            return false;

        }
    }
    bool CanPlace()
    {

        if (cellsWithinCorners.Count != (inventoryManager.currentHeldItem.size.x * inventoryManager.currentHeldItem.size.y))
        {

            return false;
        }
        foreach (InventoryCell cell in cellsWithinCorners)
        {

            if (cell.isOccupied)
            {

                return false;

            }

        }

        return true;
    }
    public void PlaceItemInCell(InventoryItem item)
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        Debug.Log(cellsWithinCorners.Count+"h");
        foreach (InventoryCell c in cellsWithinCorners)
        {

            c.isOccupied = true;
            c.occupyingItem = item;

            gameObjectList.Add(c.gameObject);
        }
         item.SetOccupyingCells(cellsWithinCorners);
        Debug.Log("Piic");
        item.occupyingGrid = this;

        item.transform.position = CalculateMiddlePosition(gameObjectList.ToArray()) ;


    }
    public void ChangePivotTopLeft(GameObject gameObject)
    {

        gameObject.GetComponent<RectTransform>().pivot =new Vector2(0,1) ;

    }
    public void CenterPivot(GameObject gameObject)
    {
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

    }

    private Vector3 CalculateMiddlePosition(GameObject[] objects)
    {
        if (objects == null || objects.Length == 0)
        {
            Debug.LogWarning("No GameObjects provided to calculate the middle position.");
            return Vector3.zero;
        }

        Vector3 sumPositions = Vector3.zero;

        foreach (GameObject obj in objects)
        {
            sumPositions += obj.transform.position;
        }

        return sumPositions / objects.Length;
    }
    public void ActiveGrid()
    {
       mouseOverGrid = true;
        inventoryManager.activeInventoryGrid = this;

    }
    public void DeActiveGrid()
    {
        mouseOverGrid = false;
        inventoryManager.activeInventoryGrid = null;
    }
}
