using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumetricFogFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomFogSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Material fogMaterial;
    }

    [SerializeField] private CustomFogSettings settings;

    private VolumetricFogPass fogPass;

    public override void Create()
    {
        fogPass = new VolumetricFogPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(fogPass);
    }
}