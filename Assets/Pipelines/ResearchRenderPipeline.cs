using UnityEngine;
using UnityEngine.Rendering;

public class ResearchRenderPipeline : RenderPipeline
{
    private CameraRenderer _cameraRenderer = new();
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        RenderAllCameras(context, cameras);
    }
    
    private void RenderAllCameras(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            _cameraRenderer.Render(context, camera);
        }
    }
}
