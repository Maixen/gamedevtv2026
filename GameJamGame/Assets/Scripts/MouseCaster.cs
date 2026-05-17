using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) 
            return;
        if (UICast()) 
            return;
        RaycastHit hit;
        Ray viewRay = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(viewRay, out hit);
        if (hit.collider.gameObject.layer != 6) 
            return;
        print(hit.collider.name + " got hit");
        hit.collider.gameObject.GetComponent<ClickTrigger>().Clicked();
    }

    private bool UICast()
    {
        //falls was im Menü gehittet wird soll ja nicht noch nach dem Boden gesucht werden
        // hier raycast zur taskbar oder dialog
        return false;
    }
}
