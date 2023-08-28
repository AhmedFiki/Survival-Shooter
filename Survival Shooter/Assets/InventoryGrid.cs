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
        Debug.Log(cells.Count + "   " + minIndex);
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
            PlaceItemInCell(inventoryManager.currentHeldItem);
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
    public void LogListContents<T>(List<T> list)
    {
        string logMessage = "List Contents:";
        foreach (T item in list)
        {
            logMessage += "\n" + item.ToString();
        }
        Debug.Log(logMessage);
    }
    public List<InventoryCell> FindEmptySpace( Vector2Int size)
    {
        List<InventoryCell> outCells = new List<InventoryCell>();
        int topY = 0;
        List<InventoryCell> topCells = new List<InventoryCell>();
        bool continueFlag = false;
        foreach (InventoryCell cell in inventoryCells)
        {
            Debug.Log(1);

            if (cell.isOccupied)
            {

                Debug.Log(2);

                continue;

            }
            else
            {
                Debug.Log(3);

                if (GetNextCells(cell, size.x) == null)
                {
                    Debug.Log(4);

                    continue;
                }
                Debug.Log(5);

                topCells.Add(cell);
                topCells .AddRange (GetNextCells(cell, size.x));
                foreach (InventoryCell topCell in topCells)
                {
                    Debug.Log(6);

                    outCells.Add(topCell);

                    if(GetLowerCells(topCell, size.y) == null)
                    {
                        
                        continueFlag = true;
                        break;
                    }
                    outCells.AddRange(GetLowerCells(topCell,size.y));


                }
                foreach(InventoryCell outCell in outCells)
                {
                    Debug.Log(7);

                    if (outCell.isOccupied)
                    {
                        Debug.Log(8);

                        continueFlag = true;
                        break;


                    }
                }
                Debug.Log(9);

                if(continueFlag)
                {
                    outCells.Clear();
                        topCells.Clear();
                    continueFlag=false;
                    continue;
                }

                return outCells;

            }

        }
        return null;
    }
    public List<InventoryCell> GetNextCells(InventoryCell cell, int width)
    {
        List<InventoryCell> nextCells = new List<InventoryCell>();
        InventoryCell nextCell = cell;

        for (int i = 1; i < width; i++)
        {
            nextCell = GetNextCell(nextCell);

            if (nextCell == null)
            {
                Debug.Log("next cells Failed");

                return null;

            }

            nextCells.Add(nextCell);

        }
        Debug.Log("next cells success");
        return nextCells;
    }
    public InventoryCell GetNextCell(InventoryCell cell)
    {
        int cellIndex = inventoryCells.IndexOf(cell);
        int cellY = cell.position.y;
        InventoryCell nextCell = null;

        if (cellIndex >= 0 && cellIndex + 1 < inventoryCells.Count)
        {
            nextCell = inventoryCells[cellIndex + 1];
        }
        else
        {
            Debug.Log("Cell index is out of range or invalid.");

            return null;

        }

        if (nextCell == null||nextCell.position.y != cellY  )
        {

            return null;
        }

        Debug.Log(cell+" | "+nextCell);

        return nextCell;

    }
    public List<InventoryCell> GetLowerCells(InventoryCell cell, int height)
    {
        List<InventoryCell> lowerCells = new List<InventoryCell>();
        InventoryCell lowerCell = cell;

        for (int i = 1; i < height; i++)
        {
            lowerCell = GetLowerCell(lowerCell);

            if (lowerCell == null)
            {
                Debug.Log("lower cells failed");

                return null;
            }

            lowerCells.Add(lowerCell);

        }

        Debug.Log("lower cells success");

        return lowerCells;

    }
    public InventoryCell GetLowerCell(InventoryCell cell)
    {
        int newIndex = inventoryCells.IndexOf(cell) + size.x;
        InventoryCell lowerCell = null;

        if (inventoryCells.IndexOf(cell) <= -1)
        {
            Debug.Log("Item not in list");

            return null;
        }

        if(newIndex >= 0 && newIndex + 1 < inventoryCells.Count)
        {
            lowerCell= inventoryCells[newIndex];

        }
        else
        {
            Debug.Log("Cell index is out of range or invalid.");

            return null;

        }


        Debug.Log(cell + " | " +lowerCell);
        return lowerCell;
    }
    public void PlaceItemInCell(InventoryItem item)
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        Debug.Log(cellsWithinCorners.Count + "h");
        foreach (InventoryCell c in cellsWithinCorners)
        {

            c.isOccupied = true;
            c.occupyingItem = item;

            gameObjectList.Add(c.gameObject);
        }
        item.SetOccupyingCells(cellsWithinCorners);
        Debug.Log("Piic");
        item.occupyingGrid = this;

        item.transform.position = CalculateMiddlePosition(gameObjectList.ToArray());


    }
    public void ChangePivotTopLeft(GameObject gameObject)
    {

        gameObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);

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
