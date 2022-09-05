using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private static readonly List<ShaderTagId> _drawingShaderTagIds = new() { new ShaderTagId("SRPDefaultUnlit") }; 
    private ScriptableRenderContext _context;
    private CullingResults _cullingResult;
    private CommandBuffer _commandBuffer;
    private Camera _camera;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _camera = camera;
        _context = context;
        
        DrawUI();

        if (_camera.TryGetCullingParameters(out var cullingParameters))
        {
            SetBufferSettings(cullingParameters);
        }

        Draw();
        DrawGizmos();
        Submit();
    }
    private void DrawUI()
    {
#if UNITY_EDITOR
        ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
#endif
    }

    private void Draw()
    {
        var drawingSettings = SetDrawingSettings(
            _drawingShaderTagIds, SortingCriteria.CommonOpaque, out var sortingSettings);
        
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
        _context.DrawSkybox(_camera);
        
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        
        _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
    }
    
    private void DrawGizmos()
    {
        _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
        _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
    }

    private DrawingSettings SetDrawingSettings(List<ShaderTagId> shaderTags, SortingCriteria sortingCriteria, 
        out SortingSettings sortingSettings)
    {
        sortingSettings = new SortingSettings(_camera)
        {
            criteria = sortingCriteria,
        };
        
        var drawingSettings = new DrawingSettings(shaderTags[0], sortingSettings);
        
        for (var i = 1; i < shaderTags.Count; i++)
        {
            drawingSettings.SetShaderPassName(i, shaderTags[i]);
        }

        return drawingSettings;
    }

    private void SetBufferSettings(ScriptableCullingParameters cullingParameters)
    {
        _cullingResult = _context.Cull(ref cullingParameters);
        _context.SetupCameraProperties(_camera);
        _commandBuffer = new CommandBuffer { name = _camera.name };
        _commandBuffer.ClearRenderTarget(true, true, Color.clear);
        _commandBuffer.BeginSample(_camera.name);
        ExecuteCommandBuffer();
    }
    

    private void ExecuteCommandBuffer()
    {
        _context.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();
    }

    private void Submit()
    {
        _commandBuffer.EndSample(_camera.name);
        ExecuteCommandBuffer();
        _context.Submit();
    }
}
