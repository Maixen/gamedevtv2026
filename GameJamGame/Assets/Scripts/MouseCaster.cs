using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    public static MouseCaster Instance;
    public static Vector3 collisionPos = Vector3.zero;
    [SerializeField] Camera cam;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.paused)
            return;
        RaycastHit hit;
        Ray viewRay = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(viewRay, out hit);
        if (hit.collider.gameObject.layer == 5)
            return;
        collisionPos = hit.point;
        if (!Input.GetMouseButton(0))
            return;
        if (hit.collider.gameObject.layer != 6) 
            return;
        //print(hit.collider.name + " got hit");
        hit.collider.gameObject.GetComponent<ClickTrigger>().Clicked();
    }
}
