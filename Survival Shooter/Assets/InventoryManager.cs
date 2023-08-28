using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    public InventoryItem currentHeldItem;
    public InventoryItem currentHighlightedItem;
    public InventoryCell currentHighlightedCell;

    public InventoryGrid activeInventoryGrid;
    public InventoryGrid lootInventoryGrid;
    public InventoryGrid playerInventoryGrid;

    public bool dragging = false;

    public float depositDuration = 1.0f;
    public Ease depositEase;
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
        if (Input.GetMouseButton(1))
        {
            if(currentHighlightedItem == null|| currentHighlightedItem.depositing)
            {
                return;
            }
            currentHighlightedItem.depositing = true;
            if (currentHighlightedItem.occupyingGrid == playerInventoryGrid)
            {

                AnimateFillAmount(currentHighlightedItem, lootInventoryGrid);
            }
            else
            {
                AnimateFillAmount(currentHighlightedItem, playerInventoryGrid);

            }

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            currentHeldItem.Rotate();


        }
    }

    public void AnimateFillAmount(InventoryItem item, InventoryGrid grid)
    {
        item.fillImage.fillAmount = 1;
        item.fillImage.gameObject.SetActive(true);

        item.fillImage.DOFillAmount(0, depositDuration).SetEase(depositEase).OnComplete(() => {
            item.fillImage.gameObject.SetActive(false);
            grid.DepositItem(item);
            item.depositing = false;
        });
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
