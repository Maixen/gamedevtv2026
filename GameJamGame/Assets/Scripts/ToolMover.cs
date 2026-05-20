using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMover : MonoBehaviour
{
    [SerializeField] private float moveHeight;
    [SerializeField] private float lerpSpeed;

    private void Update()
    {
        if (GameManager.paused)
            return;
        transform.position = Vector3.Lerp(transform.position, MouseCaster.collisionPos, lerpSpeed * Time.deltaTime);
    }
}
