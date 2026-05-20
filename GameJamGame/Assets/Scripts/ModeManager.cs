using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mode
{
    None = 0,
    Cut = 1,
    Water = 2,
    Screw = 3,
    Gun = 4
}

public class ModeManager : MonoBehaviour
{
    public static bool fuseIsOn;
    public static bool fuseForceOff;
    public static Mode mode;
    public static ModeManager instance;
    [SerializeField] private GameObject[] tools;

    [SerializeField] private Sprite fuseOn, fuseOff;
    [SerializeField] private Image fuseRenderer;
    private bool canClickFuse;
    [SerializeField] private float fuseDelay;

    [SerializeField] private Color activeTool;
    [SerializeField] private Color deactiveTool;

    [SerializeField] private Image modeSaw, modeWater, modeScrew, modeGun;
    [SerializeField] private RectTransform modeSawC, modeWaterC, modeScrewC, modeGunC;
    public Vector3 modeSawV, modeWaterV, modeScrewV, modeGunV;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        modeSawV = modeSawC.localScale;
        modeWaterV = modeWaterC.localScale;
        modeScrewV = modeScrewC.localScale;
        modeGunV = modeGunC.localScale;

        ChangeMode(0);
        canClickFuse = true;
        SetSwitch(true);
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
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        modeSaw.color = deactiveTool;
        modeWater.color = deactiveTool;
        modeScrew.color = deactiveTool;
        modeGun.color = deactiveTool;
        modeSawC.localScale = modeSawV;
        modeWaterC.localScale = modeWaterV;
        modeScrewC.localScale = modeScrewV;
        modeGunC.localScale = modeGunV;

        switch (mode)
        {
            case Mode.None:
                break;
            case Mode.Cut:
                modeSaw.color = activeTool;
                modeSawC.localScale = modeSawV * 1.2f;
                break;
            case Mode.Water:
                modeWater.color = activeTool;
                modeWaterC.localScale = modeWaterV * 1.2f;
                break;
            case Mode.Screw:
                modeScrew.color = activeTool;
                modeScrewC.localScale = modeScrewV * 1.2f;
                break;
            case Mode.Gun:
                modeGun.color = activeTool;
                modeGunC.localScale = modeGunV * 1.2f;
                break;
            default:
                break;
        }
    }

    public void ToggleSwitch()
    {
        SetSwitch(!fuseIsOn);
    }

    public void SetSwitch(bool on)
    {
        if (!canClickFuse) { return; }
        if (!on) { fuseIsOn = false; fuseRenderer.sprite = fuseOff; return; }

        if (!fuseForceOff)
        {
            fuseIsOn = true; fuseRenderer.sprite = fuseOn;
            return;
        }
        else
        {
            canClickFuse = false;
            fuseRenderer.sprite = fuseOn;
            Invoke(nameof(ToggleSwitchFollow), fuseDelay);
        }
    }

    private void ToggleSwitchFollow()
    {
        canClickFuse = true;
        SetSwitch(false);
    }
}
