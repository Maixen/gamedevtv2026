using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
    private float clickedAtTime = 0;
    public bool isClicked;

    [SerializeField] private float maxClickDelay;

    private void OnEnable()
    {
        isClicked = false;
    }

    public void Clicked()
    {
        clickedAtTime = Time.time;
        isClicked = true;
    }

    private void Update()
    {
        if (!isClicked)
            return;
        if(clickedAtTime + maxClickDelay < Time.time)
            isClicked = false;
    }
}
