using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryCell : MonoBehaviour
{

    public bool isOccupied = false;
    public bool highlighted = false;

    public Vector2Int position;

    public InventoryItem occupyingItem = null;

    [SerializeField]
    public Color originalColor;

    InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = InventoryManager.instance;
        originalColor = GetComponent<Image>().color;
    }

    public void Highlight()
    {
        if(isOccupied)
        {
            ChangeColorTo(Color.red);
        }
        else
        {
            ChangeColorTo(Color.blue);
        }
        
       

        highlighted = true;
        inventoryManager.currentHighlightedCell = this;
    }

    public void MouseHighlight()
    {
        if (isOccupied)
        {
            ChangeColorTo(Color.cyan);
        }
        else
        {
            ChangeColorTo(Color.gray);
        }



        highlighted = true;
        inventoryManager.currentHighlightedCell = this;
    }
    public void UnHighlight()
    {
        ResetColor();
        highlighted = false;
        inventoryManager.currentHighlightedCell = null;

    }


    public void ChangeColorTo(Color color)
    {
        // Change the image color to blue
        GetComponent<Image>().color = color;
    }

    public void ResetColor()
    {
        // Reset the image color to its original color
        GetComponent<Image>().color = originalColor;
    }
}
