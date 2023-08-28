using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    [SerializeField]TMP_Text text;


    private void Update()
    {
        text.text = "" + Input.mousePosition;
    }
}
