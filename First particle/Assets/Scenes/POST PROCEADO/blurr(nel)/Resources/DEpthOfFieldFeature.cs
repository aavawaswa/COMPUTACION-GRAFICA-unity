using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DEpthOfFieldFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        //ID de la imagen que guarda el pantallazo
        private RenderTargetHandle screenShotTarget;
        private Material renderingMaterial;

        

        private Material Renderingmaterial
        {
            get
            {
                 if (renderingMaterial == null)
                 {
                    renderingMaterial = new Material(Resources.Load<Shader>("Blur"));
                 }
                return renderingMaterial;
            }
        }
        public CustomRenderPass()
        {
            //Se inicializa el ID
            screenShotTarget.Init("_ColorInvertTexture");
            //Asignamos material del feature
          
          
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            VolumeStack stack = VolumeManager.instance.stack;
              if (!VolumeManager.instance.IsComponentActiveInMask<DepthOfField>(renderingData.cameraData.camera.cullingMask))
                return;
                DepthofField dof = stack.GetComponent<DepthofField>();
                Renderingmaterial.SetVector("_Pixeloffset",Vector4.one * dof.blurIntensity.value);
            //colorinvert materialSettings = volumeStack.GetComponent<colorinvert>();
            //if (materialSettings == null || !materialSettings.active || !materialSettings.IsActive()) return;
            //Renderingmaterial.SetFloat("_InvertWeigth",materialSettings.weight.value);
            //Lista de comandos para ejecutar a la hora de renderizar
            CommandBuffer cmd = CommandBufferPool.Get("Invert Colors");
            RenderTextureDescriptor screenDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            //Crear textura
            cmd.GetTemporaryRT(screenShotTarget.id, screenDescriptor);
            //Tomar pantallazo e invertir color
            cmd.Blit(renderingData.cameraData.renderer.cameraColorTarget, screenShotTarget.Identifier(), Renderingmaterial, Renderingmaterial.FindPass("Universal Forward"));
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
   

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

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
