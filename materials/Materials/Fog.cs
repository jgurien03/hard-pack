// FogEffectFeature.cs
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogEffectFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class FogEffectSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Color fogColor = Color.gray;
        [Range(0.0f, 1.0f)] public float fogDensity = 0.02f;
        [Range(0.0f, 100.0f)] public float fogOffset = 0.0f;
        [Range(0.0f, 1.0f)] public float fogStrength = 0.02f;
    }

    [SerializeField] private FogEffectSettings settings;

    private FogPass fogPass;

    public override void Create()
    {
        fogPass = new FogPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(fogPass);
    }
}