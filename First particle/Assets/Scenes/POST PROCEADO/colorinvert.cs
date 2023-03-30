using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable,VolumeComponentMenu("Custom Effects/Color Invert")]

public class colorinvert : VolumeComponent, IPostProcessComponent
{

    public ClampedFloatParameter weigth = new ClampedFloatParameter(value:1, min:0, max:1 ,overrideState:true);

    public bool IsActive() => weigth.value > 0 ;
 
     public bool IsTileCompatible() => false;
}
