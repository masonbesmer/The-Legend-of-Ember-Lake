using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
 
 
[DisallowMultipleRendererFeature]
internal class VolumetricFog : ScriptableRendererFeature
{
    private VolumetricFogPass m_VolumetricFogPass = null;

    public Material fogMaterial;
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

    [Serializable]
    public struct Settings{
        public float density;
        public Color fogColor;
        public int fogSteps;
        public float maxDistance;
        public float jitter;
    }

    public Settings settings = new Settings{
        density = 0.1f,
        fogColor = Color.white,
        fogSteps = 10,
        maxDistance = 100f,
        jitter = 0.1f
    };

 
    public override void Create()
    {
        if (m_VolumetricFogPass == null)
        {
            m_VolumetricFogPass = new VolumetricFogPass(
                renderPassEvent,
                fogMaterial
            );
        }

        m_VolumetricFogPass.Setup(settings);
    }
 
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_VolumetricFogPass);
    }
 
    private class VolumetricFogPass : ScriptableRenderPass
    {
        
        RenderTargetHandle tempTexture;

        Material fogMaterial;
        Settings settings;

        public VolumetricFogPass(RenderPassEvent renderPassEvent, Material fogMaterial)
        {
            this.renderPassEvent = renderPassEvent;
            this.fogMaterial = fogMaterial;
        }

        // This isn't part of the ScriptableRenderPass class and is our own addition.
        // For this custom pass we need the camera's color target, so that gets passed in.
        public void Setup(Settings settings)
        {
           this.settings = settings;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            // create a temporary render texture that matches the camera
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        }
 
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) 
        {
            CommandBuffer cmd = CommandBufferPool.Get("Volumetric Fog");
            cmd.Clear();

            // Set the fog material properties

            fogMaterial.SetInt("_StepCount", settings.fogSteps);
            fogMaterial.SetFloat("_Density", settings.density);
            fogMaterial.SetFloat("_MaxDistance", settings.maxDistance);
            fogMaterial.SetFloat("_Jitter", settings.jitter);
            fogMaterial.SetColor("_FogColor", settings.fogColor);

            cmd.SetGlobalInt("_StepCount", settings.fogSteps);
            cmd.SetGlobalFloat("_MaxDistance", settings.maxDistance);
            cmd.SetGlobalFloat("_Density", settings.density);
            cmd.SetGlobalColor("_FogColor", settings.fogColor);
            cmd.SetGlobalFloat("_Jitter", settings.jitter);

            
            cmd.Blit(renderingData.cameraData.renderer.cameraColorTarget, tempTexture.id, fogMaterial, 0);
            cmd.Blit(tempTexture.id, renderingData.cameraData.renderer.cameraColorTarget);

            context.ExecuteCommandBuffer(cmd);
            
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }
}