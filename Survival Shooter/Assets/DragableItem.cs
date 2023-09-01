using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform canvasRectTransform;

    Image image;
    public Vector3 startPosition;

    InventoryManager inventoryManager;
    InventoryItem inventoryItem;
    bool isDragging=false;
    private void Start()
    {
        inventoryItem = GetComponent<InventoryItem>();
        inventoryManager = InventoryManager.instance;
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return; // Ignore other mouse buttons
        }
        isDragging = true;
        Debug.Log("Begin Drag");
        startPosition = transform.position;
        inventoryManager.currentHeldItem = GetComponent<InventoryItem>();
        image.raycastTarget = false;
        transform.SetAsLastSibling();
        inventoryItem.UnOccupyCells();

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return; 
        }

        if (inventoryManager.activeInventoryGrid!=null && inventoryManager.activeInventoryGrid.AttemptPlacement() )
        {

        }
        else
        {
            transform.position = startPosition;
            inventoryItem.OccupyCells();


        }
        Debug.Log("End Drag");
        inventoryManager.activeInventoryGrid.UnHighlightAllCells();
        image.raycastTarget = true;
        inventoryManager.currentHeldItem = null;
        transform.SetAsFirstSibling();
        isDragging = false;


    }




}
