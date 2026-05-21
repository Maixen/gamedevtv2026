using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public enum ChaosType
{
    Fire, Pole, Strike, Corpse
}

public class ChaosManager : MonoBehaviour
{
    public static float chaosLevel = 0f; // Zwischen 0 und 1
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

    [Space]
    [Space]

    [SerializeField] private RectTransform chaosMeter;
    float maxLength = 500;
    [SerializeField] private RectTransform chaosBar;
    private float targetChaos;
    [SerializeField] private float chaosGrowSpeed;
    [SerializeField] private ChaosType failType;
    [SerializeField] private int[] amountToFail;
    [SerializeField] private int[] problems;
    private bool safe = true;
    

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

        Rect rect = chaosMeter.rect;
        maxLength = rect.height;
        
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

    private float ConvertVector2ToFloatViaChaosMultiplier(Vector2 vec)
    {
        float difference = Mathf.Abs(vec.y - vec.x);
        float subVal = difference * chaosLevel;
        return Mathf.Min(vec.x, vec.y) + subVal;
    }

    private void Update()
    {
        if (GameManager.paused)
        {
            return;
        }
        if (targetChaos > chaosLevel || ModeManager.fuseIsOn)
        {
            chaosLevel = Lerp(chaosLevel,targetChaos,chaosGrowSpeed);
        }
        if(chaosLevel == 0)
        {
            chaosBar.rect.Set(chaosBar.rect.x, chaosBar.rect.y, chaosBar.rect.width, 0);
        }
        else
        {
            chaosBar.rect.Set(chaosBar.rect.x, chaosBar.rect.y, chaosBar.rect.width, 500 * chaosLevel);
        }
        print(chaosLevel);
        if (chaosLevel >= 1)
        {
            EndGame();
        }
    }

    private float Lerp(float based, float target,float timeMult)
    {
        return based + (target - based) * timeMult * Time.deltaTime;
    }

    public void AddProblem(ChaosType type)
    {
        problems[(int)type]++;
        UpdateChaos();
    }

    public void FixProblem(ChaosType type)
    {
        problems[(int)type]--;
        if(problems[(int)type] < 0)
        {
            problems[(int)type] = 0;
        }
        UpdateChaos();
    }

    private void UpdateChaos()
    {
        float problemPercentage = 0;
        for(int i = 0; i < problems.Length; i++)
        {
            float percentage = (float)problems[i] / amountToFail[i];
            if (problemPercentage < percentage)
            {
                failType = (ChaosType)i;
                problemPercentage = percentage;
            }
        }
        if(problemPercentage == 0)
        {
            ModeManager.instance.SafeAgain();
        }
        targetChaos = problemPercentage;
    }

    private void EndGame()
    {
        GameManager.instance.GamePauseRequest();
        print("Verloren");
        switch(failType)
        {
            //Pack hier die verschiedenen Lose Dialoge rein
            case ChaosType.Fire:
                break;
            case ChaosType.Pole:
                break;
            case ChaosType.Strike:
                break;
            case ChaosType.Corpse:
                break;
        }
    }
}
