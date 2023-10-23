using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Custom/Ben Day Bloom", typeof(UniversalRenderPipeline))]
public class BenDayBloomEffect : VolumeComponent, IPostProcessComponent
{
    [Header("Bloom Settings")]
    public FloatParameter threshold = new FloatParameter(.9f, true);
    public FloatParameter intensity = new FloatParameter(1f, true);
    public ClampedFloatParameter scatter = new ClampedFloatParameter(.7f, 0, 1, true);
    public IntParameter clamp = new IntParameter(65472, true);
    public ClampedIntParameter maxIterations = new ClampedIntParameter(6, 0, 10);
    public NoInterpColorParameter tint = new NoInterpColorParameter(Color.white);
    public bool IsActive ()
    {
        return true;
    }

    // Update is called once per frame
    public bool IsTileCompatible ()
    {
        return false;
    }
}
