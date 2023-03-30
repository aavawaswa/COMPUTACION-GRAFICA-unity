using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class colorinvertfeatre : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        //ID de la imagen que guarda el pantallazo
        private RenderTargetHandle screenShotTarget;
        private Material renderingMaterial;

        public CustomRenderPass(Material renderingMaterial)
        {
            //Se inicializa el ID
            screenShotTarget.Init("_ColorInvertTexture");
            //Asignamos material del feature
            this.renderingMaterial = renderingMaterial;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //Lista de comandos para ejecutar a la hora de renderizar
            CommandBuffer cmd = CommandBufferPool.Get("Invert Colors");
            RenderTextureDescriptor screenDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            //Crear textura
            cmd.GetTemporaryRT(screenShotTarget.id,screenDescriptor);
            //Tomar pantallazo e invertir color
            cmd.Blit(renderingData.cameraData.renderer.cameraColorTarget, screenShotTarget.Identifier(), renderingMaterial, renderingMaterial.FindPass("Universal Forward"));
            //Devolver pantallazo a la pantalla
            cmd.Blit(screenShotTarget.Identifier(), renderingData.cameraData.renderer.cameraColorTarget);
            context.ExecuteCommandBuffer(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(screenShotTarget.id);
        }
    }

    CustomRenderPass m_ScriptablePass;
    [SerializeField] private RenderPassEvent renderEvent;
    [SerializeField] private Material renderingMaterial;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(renderingMaterial);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = renderEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
