using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ImageEffectFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private RenderTargetIdentifier source { get; set; }
        private RenderTargetHandle destination { get; set; }
        public Material material = null;
        RenderTargetHandle _temporaryColorTexture;

        public CustomRenderPass(Material material)
        {
            this.material = material;
        }

        public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("_OutlinePass");

            RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDescriptor.depthBufferBits = 0;

            if (destination == RenderTargetHandle.CameraTarget)
            {
                cmd.GetTemporaryRT(_temporaryColorTexture.id, opaqueDescriptor, FilterMode.Point);
                Blit(cmd, source, _temporaryColorTexture.Identifier(), material, 0);
                Blit(cmd, _temporaryColorTexture.Identifier(), source);

            }
            else Blit(cmd, source, destination.Identifier(), material, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (destination == RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(_temporaryColorTexture.id);
        }
    }

    public Material material;
    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        if (material == null)
        {
            Debug.LogWarning("Missing Image Effect Material", this);
            return;
        }
        m_ScriptablePass = new CustomRenderPass(material);

        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null)
        {
            return;
        }
        m_ScriptablePass.Setup(renderer.cameraColorTarget, RenderTargetHandle.CameraTarget);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


