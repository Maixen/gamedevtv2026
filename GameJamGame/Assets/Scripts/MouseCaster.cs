using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    public static MouseCaster Instance;
    public static Vector3 collisionPos = Vector3.zero;
    [SerializeField] Camera cam;

    [SerializeField] private LayerMask clickable;
    [SerializeField] private float sphereCastRadius;

    private bool hasHit;
    private RaycastHit lastHit;

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
        lastHit = hit;
        hasHit = true;
        if (hit.collider.gameObject.layer == 5)
            return;
        collisionPos = hit.point;
        if (!Input.GetMouseButton(0))
            return;
        if (hit.collider.gameObject.layer == 6)
            hit.collider.gameObject.GetComponent<ClickTrigger>().Clicked();
        else
        {
            Collider[] hits = Physics.OverlapSphere(hit.point, sphereCastRadius, clickable);
            //print(hit.collider.name + " got hit");
            if (hits.Length > 0)
                hits[0].gameObject.GetComponent<ClickTrigger>().Clicked();
        }
    }

    private void OnDrawGizmos()
    {
        if (hasHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lastHit.point, sphereCastRadius);
        }
    }
}
