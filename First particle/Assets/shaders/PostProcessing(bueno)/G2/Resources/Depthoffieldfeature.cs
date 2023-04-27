using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Depthoffieldfeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private RenderTargetHandle blitTarget;
        private RenderTargetHandle blitTarget2;
        private Material material;

        private Material MAT
        {
            get
            {
                if (material == null)
                {
                    material = new Material(Resources.Load<Shader>("Blur"));
                }
                return material;
            }
        }

        public CustomRenderPass ()
        {
            blitTarget.Init("_DepthOfFieldTemporary");
            blitTarget2.Init("_DepthOfFieldTemporary");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!VolumeManager.instance.IsComponentActiveInMask<DepthOfField>(LayerMask.GetMask("Default")))
                return;

            VolumeStack stack = VolumeManager.instance.stack;
            DepthOfField effect = stack.GetComponent<DepthOfField>();
            MAT.SetFloat("_DepthStart",effect.depthStart.value);
            MAT.SetFloat("_DepthEnd", effect.depthEnd.value);
            int blurIterations = (int)effect.blurIntensity.value;


            CommandBuffer cmd = CommandBufferPool.Get("Depth Of Field");

            cmd.GetTemporaryRT(blitTarget.id, renderingData.cameraData.cameraTargetDescriptor);
            cmd.GetTemporaryRT(blitTarget2.id, renderingData.cameraData.cameraTargetDescriptor);
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTargetIdentifier destination = blitTarget.id;

            cmd.Blit(source, destination);
            source=destination;
            destination = blitTarget2.Identifier();

            for(int i = 0; i < blurIterations; i++)
            {
                cmd.Blit(source, destination,MAT, MAT.FindPass("Universal Forward"));
                (source,destination) = (destination,source);
            }
            source=destination;
            destination = renderingData.cameraData.renderer.cameraColorTarget;
            cmd.Blit(source, destination);
            context.ExecuteCommandBuffer(cmd);
            cmd.Release();
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(blitTarget.id);
        }
    }

    CustomRenderPass m_ScriptablePass;


    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


