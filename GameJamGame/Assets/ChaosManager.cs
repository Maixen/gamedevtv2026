using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ChaosManager : MonoBehaviour
{
    public static float chaosLevel = 0f;
    public static ChaosManager instance;

    [SerializeField] private PostProcessProfile profile;
    [SerializeField] private Material grassMaterial;

    [SerializeField] private Material pixelation;
    [SerializeField] private Vector2 pixelationIntensity;

    [SerializeField] private Vector2 LensDistortion;
    [SerializeField] private Vector2 BloomIntensity;
    [SerializeField] private Vector2 VignetteIntensity;
    [SerializeField] private Vector2 GrainIntensity;
    [SerializeField] private Vector2 ChromaticAbberation;

    private LensDistortion ppL;
    private Bloom ppB;
    private Vignette ppV;
    private Grain ppG;
    private ChromaticAberration ppC;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        profile.TryGetSettings(out ppL);
        profile.TryGetSettings(out ppB);
        profile.TryGetSettings(out ppV);
        profile.TryGetSettings(out ppG);
        profile.TryGetSettings(out ppC);

        UpdateEffects();
    }

    public bool ChaosLevelChangeRequest(float change, bool force)
    {
        if (force)
        {
            chaosLevel = Mathf.Clamp(chaosLevel + change, 0f, 1f);
            UpdateEffects();
            return true;
        }
        else
        {
            float newVal = chaosLevel + change;
            if (newVal > 1f || newVal < 0f) { return false; }
            chaosLevel = newVal;
            UpdateEffects();
            return true;
        }
    }

    public void UpdateEffects()
    {
        ppL.intensity.value = ConvertVector2ToFloatViaChaosMultiplier(LensDistortion);
        ppB.intensity.value = ConvertVector2ToFloatViaChaosMultiplier(BloomIntensity);
        ppV.intensity.value = ConvertVector2ToFloatViaChaosMultiplier(VignetteIntensity);
        ppG.intensity.value = ConvertVector2ToFloatViaChaosMultiplier(GrainIntensity);
        ppC.intensity.value = ConvertVector2ToFloatViaChaosMultiplier(ChromaticAbberation);
    }

    private void Update()
    {
        
    }

    private float ConvertVector2ToFloatViaChaosMultiplier(Vector2 vec)
    {
        float difference = Mathf.Abs(vec.y - vec.x);
        float subVal = difference * chaosLevel;
        return Mathf.Min(vec.x, vec.y) + subVal;
    }
}
