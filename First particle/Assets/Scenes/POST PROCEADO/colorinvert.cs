using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom Effects/Color Invert")]
public class colorinvert : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter weight = new ClampedFloatParameter(1, 0, 1, true);

    public bool IsActive() => weight.value > 0;

    public bool IsTileCompatible() => false;
}
