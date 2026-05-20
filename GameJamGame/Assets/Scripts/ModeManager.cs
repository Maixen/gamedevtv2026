using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    None = 0,
    Cut = 1,
    Water = 2,
    Screw = 3
}

public class ModeManager : MonoBehaviour
{
    public static Mode mode;
    public static ModeManager instance;
    [SerializeField] private GameObject[] tools;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeMode(0);
    }

    public void ChangeMode (int mode)
    {
        if (GameManager.paused) { return; }
        if (ModeManager.mode == (Mode)mode) { mode = 0; }
        ModeManager.mode = (Mode)mode;
        Debug.Log("New mode: " + ModeManager.mode);
        for(int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i + 1 == mode);
        }
    }
}
