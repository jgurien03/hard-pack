using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumetricFogPass : ScriptableRenderPass
{
    private Material fogMaterial;
    private RenderTargetIdentifier source;
    private RenderTargetIdentifier destination;

    private int temporaryRTId = Shader.PropertyToID("_TemporaryRT");

    public VolumetricFogFeature.CustomFogSettings settings;

    public VolumetricFogPass(VolumetricFogFeature.CustomFogSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        this.fogMaterial = settings.fogMaterial;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // Create a temporary render target with the same descriptor as the camera target
        destination = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        // Set fog properties using settings from FogEffectFeature
        cmd.GetTemporaryRT(temporaryRTId, descriptor, FilterMode.Bilinear);
        source = new RenderTargetIdentifier(temporaryRTId);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Volumetric Pass")))
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