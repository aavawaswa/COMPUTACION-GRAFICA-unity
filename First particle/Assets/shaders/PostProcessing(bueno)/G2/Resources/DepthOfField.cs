using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfField : VolumeComponent, IPostProcessComponent
{
    public FloatParameter depthStart = new FloatParameter(0);
    public FloatParameter depthEnd = new FloatParameter(10);
    public FloatParameter blurIntensity = new FloatParameter(25);

    public bool IsActive() => blurIntensity.value > 0;

    public bool IsTileCompatible() => false;
}