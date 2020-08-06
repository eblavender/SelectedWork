using DG.Tweening;
using UnityEngine;
using UnityEngine.AzureSky;

public class EnvironmentTransitions : MonoBehaviour
{
    #region Variables
    [Range(0, 60)]
    [SerializeField] private int transitionLength;
    [SerializeField] private Light directionalLight, backLight;
    [HideInInspector] public Color brazierLightColor;
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material moonMaterial;
    [SerializeField] private Material cloudMaterial;
    [SerializeField] private Material emissionPlaneMaterial;
    [SerializeField] private Material brazierEmbersMaterial;

    [Header("Environment Profiles")]
    [SerializeField] private EnvironmentDetails[] environmentProfiles;
    [SerializeField] private EnvironmentDetails skeletonBaseProfile, skeletonGrassProfile, skeletonIceProfile, skeletonFireProfile;
    [SerializeField] private EnvironmentDetails armourBaseProfile, armourGrassProfile, armourIceProfile, armourFireProfile;
    [SerializeField] private EnvironmentDetails rockBaseProfile, rockGrassProfile, rockIceProfile, rockFireProfile;

    private AzureSkyController azure;
    private EnvironmentDetails currentDetails;

    private int index = 0;
    #endregion

    #region Initialisation
    private void Awake()
    {
        azure = GetComponent<AzureSkyController>();
        currentDetails = environmentProfiles[index];
    }
    private void Start()
    {
        SetFirstTransition();
    }
    /// <summary>
    /// Force sets the first environment profile
    /// </summary>
    public void SetFirstTransition()
    {
        SetTransition(environmentProfiles[0]);
    }
    #endregion

    #region Transition Triggers
    /// <summary>
    /// Force sets the specified environment profile 
    /// </summary>
    /// <param name="details">The environment profile that is being force set</param>
    public void SetTransition(EnvironmentDetails details)
    {
        azure.SetNewDayProfile(details.azureProfile, 0f);

        directionalLight.color = details.directionalColor;
        directionalLight.intensity = details.directionalIntensity;
        backLight.color = details.backColor;
        backLight.intensity = details.backIntensity;
        brazierLightColor = details.brazierColor;

        fireMaterial.SetColor("_EmissionColor", details.fireColor);
        brazierEmbersMaterial.SetColor("_EmissionColor", details.brazierColor);
        moonMaterial.SetColor("_EmissionColor", details.moonColor);

        cloudMaterial.SetColor("_Color_1", details.firstCloudColor);
        cloudMaterial.SetColor("_Color_2", details.secondCloudColor);
        emissionPlaneMaterial.SetColor("_EmissionColor", details.emissionColor);

        RenderSettings.fogColor = details.fogColor;
        RenderSettings.fogStartDistance = details.fogStartDistance;
    }

    /// <summary>
    /// Sets and smoothly transitions to the next environment profile
    /// </summary>
    public void NextTransition()
    {
        azure.SetNewDayProfile(currentDetails.azureProfile);

        index++;
        currentDetails = environmentProfiles[index];
        
        PlayTransition();
    }
    
    /// <summary>
    /// Force sets the environment profile of the specified boss
    /// </summary>
    /// <param name="boss">The boss type</param>
    /// <param name="element">The boss element</param>
    public void SetBossTransition(BossType boss, BossElement element)
    {
        if (boss == BossType.Skeleton)
        {
            switch (element)
            {
                case BossElement.Base:
                    SetTransition(skeletonBaseProfile);
                    break;
                case BossElement.Grass:
                    SetTransition(skeletonGrassProfile);
                    break;
                case BossElement.Ice:
                    SetTransition(skeletonIceProfile);
                    break;
                case BossElement.Fire:
                    SetTransition(skeletonFireProfile);
                    break;
            }
        }
        else if (boss == BossType.Armour)
        {
            switch (element)
            {
                case BossElement.Base:
                    SetTransition(armourBaseProfile);
                    break;
                case BossElement.Grass:
                    SetTransition(armourGrassProfile);
                    break;
                case BossElement.Ice:
                    SetTransition(armourIceProfile);
                    break;
                case BossElement.Fire:
                    SetTransition(armourFireProfile);
                    break;
            }
        }
        else if (boss == BossType.Rocky)
        {
            switch (element)
            {
                case BossElement.Base:
                    SetTransition(rockBaseProfile);
                    break;
                case BossElement.Grass:
                    SetTransition(rockGrassProfile);
                    break;
                case BossElement.Ice:
                    SetTransition(rockIceProfile);
                    break;
                case BossElement.Fire:
                    SetTransition(rockFireProfile);
                    break;
            }
        }
    }
    #endregion

    #region Transition Stages
    /// <summary>
    /// Smoothly transitions to the selected environment profile
    /// </summary>
    private void PlayTransition()
    {
        azure.SetNewDayProfile(currentDetails.azureProfile, transitionLength);

        PlayLightTransition();
        PlayEffectsTransition();
        PlayCloudsTransition();
        PlayFogTransition();
    }
    /// <summary>
    /// Smoothly transitions all of the lighting
    /// </summary>
    private void PlayLightTransition()
    {
        directionalLight.DOColor(currentDetails.directionalColor, transitionLength).SetEase(Ease.Linear);
        backLight.DOColor(currentDetails.backColor, transitionLength).SetEase(Ease.Linear);

        DOTween.To(() => brazierLightColor, x => brazierLightColor = x, currentDetails.brazierColor, transitionLength).SetEase(Ease.Linear);
        DOTween.To(() => directionalLight.intensity, x => directionalLight.intensity = x, currentDetails.directionalIntensity, transitionLength).SetEase(Ease.Linear);
        DOTween.To(() => backLight.intensity, x => backLight.intensity = x, currentDetails.backIntensity, transitionLength).SetEase(Ease.Linear);
    }
    /// <summary>
    /// Smoothly transitions all of the effect materials
    /// </summary>
    private void PlayEffectsTransition()
    {
        fireMaterial.DOColor(currentDetails.fireColor, "_EmissionColor", transitionLength).SetEase(Ease.Linear);
        brazierEmbersMaterial.DOColor(currentDetails.embersColor, "_EmissionColor", transitionLength).SetEase(Ease.Linear);
        moonMaterial.DOColor(currentDetails.moonColor, "_EmissionColor", transitionLength).SetEase(Ease.Linear);
    }
    /// <summary>
    /// Smoothly transitions all of the cloud materials
    /// </summary>
    private void PlayCloudsTransition()
    {
        cloudMaterial.DOColor(currentDetails.firstCloudColor, "_Color_1", transitionLength).SetEase(Ease.Linear);
        cloudMaterial.DOColor(currentDetails.secondCloudColor, "_Color_2", transitionLength).SetEase(Ease.Linear);
        emissionPlaneMaterial.DOColor(currentDetails.emissionColor, "_EmissionColor", transitionLength).SetEase(Ease.Linear);
    }
    /// <summary>
    /// Smoothly transitions the fog
    /// </summary>
    private void PlayFogTransition()
    {
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, currentDetails.fogColor, transitionLength).SetEase(Ease.Linear);
        DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, currentDetails.fogStartDistance, transitionLength).SetEase(Ease.Linear);
    }
    #endregion
}
