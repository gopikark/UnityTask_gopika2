using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_UnSelect : MonoBehaviour
{
    bool isHighlighted = false;
    public GameObject AcuPointText;

    void HighlightRed()
    {
        AcuPointText.GetComponent<Renderer>().enabled = true; // true to show
    }

    void RemoveHighlight()
    {
        AcuPointText.GetComponent<Renderer>().enabled = false;
    }

    void OnMouseDown()
    {
        isHighlighted = !isHighlighted;
        Debug.Log(isHighlighted);
        if (isHighlighted == true)
        {
            AcuPointText.GetComponent<Renderer>().enabled = false;
        }
        if (isHighlighted == false)
        {
            AcuPointText.GetComponent<Renderer>().enabled = true;
        }
    }
}
