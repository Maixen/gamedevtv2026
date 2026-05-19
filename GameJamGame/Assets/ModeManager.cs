using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    None = 0,
    Cut = 1,
    Water = 2,
    Reserved = 3
}

public class ModeManager : MonoBehaviour
{
    public static Mode mode;
    public static ModeManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeMode (int mode, bool force = false)
    {
        if (GameManager.paused && force == false) { return; }
        if (ModeManager.mode == (Mode)mode) { mode = 0; }
        ModeManager.mode = (Mode)mode;
        Debug.Log("New mode: " + ModeManager.mode);
    }
}
