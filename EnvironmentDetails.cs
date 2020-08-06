using UnityEngine;
using UnityEngine.AzureSky;

[CreateAssetMenu(fileName = "Environment Profile", menuName = "Environment/EnvironmentProfile", order = 1)]
public class EnvironmentDetails : ScriptableObject
{
    public AzureSkyProfile azureProfile;

    [Header("Lights")]
    public Color brazierColor;
    public Color directionalColor;
    public Color backColor;
    public float directionalIntensity;
    public float backIntensity;

    [Header("Effects")]
    [ColorUsage(true, true)]
    public Color fireColor;
    [ColorUsage(true, true)]
    public Color embersColor;
    [ColorUsage(true, true)]
    public Color moonColor;

    [Header("Clouds")]
    public Color firstCloudColor;
    public Color secondCloudColor;
    [ColorUsage(true, true)]
    public Color emissionColor;

    [Header("Fog")]
    public Color fogColor;
    public int fogStartDistance;
}
