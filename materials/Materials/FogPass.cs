using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogPass : ScriptableRenderPass
{
    private Material fogMaterial;
    private RenderTargetIdentifier source;
    private RenderTargetIdentifier destination;

    private int temporaryRTId = Shader.PropertyToID("_TemporaryRT");

    public FogEffectFeature.FogEffectSettings settings;

    public FogPass(FogEffectFeature.FogEffectSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        if (fogMaterial == null) fogMaterial = CoreUtils.CreateEngineMaterial("Hidden/SSFog");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // Create a temporary render target with the same descriptor as the camera target
        destination = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        // Set fog properties using settings from FogEffectFeature
        fogMaterial.SetColor("_FogColor", settings.fogColor);
        fogMaterial.SetFloat("_FogDensity", settings.fogDensity);
        fogMaterial.SetFloat("_FogOffset", settings.fogOffset);
        fogMaterial.SetFloat("_FogStrength", settings.fogStrength);
        cmd.GetTemporaryRT(temporaryRTId, descriptor, FilterMode.Bilinear);
        source = new RenderTargetIdentifier(temporaryRTId);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Fog Pass")))
        {
            // Blit the source to the destination with the fog material
            Blit(cmd, destination, source, fogMaterial);
            Blit(cmd, source, destination);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        // Release the temporary render target
        cmd.ReleaseTemporaryRT(temporaryRTId);
    }
}