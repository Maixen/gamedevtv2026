using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMover : MonoBehaviour
{
    [SerializeField] private float moveHeight;
    [SerializeField] private float lerpSpeed;

    private void Update()
    {
        Vector3 mousePos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePos = hit.point;
        }

        Vector3 targetPos = mousePos + Vector3.up * moveHeight;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
