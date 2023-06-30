using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer {
    ScriptableRenderContext context;
    Camera camera;
    CullingResults cullingres;
    const string bufname = "Render Camera";
    CommandBuffer cmdbuf = new CommandBuffer { name = bufname };
    static ShaderTagId unlit = new ShaderTagId("SRPDefaultUnlit");
    static ShaderTagId litShaderTagId = new ShaderTagId("CustomLit");
    Lighting lighting = new Lighting();
    public void Render(ScriptableRenderContext context, Camera camera,
        bool useDynamicBatching, bool useGPUInstancing,ShadowSettings shadowSettings) {
        this.context = context;
        this.camera = camera;
        PrepareBuffer();
        PrepareForSceneWindow();    
        if (!Cull(shadowSettings.maxDistance)) {
            return;
        }

        cmdbuf.BeginSample(SampleName);
        ExecuteBuffer();
        lighting.Setup(context,cullingres, shadowSettings);
        cmdbuf.EndSample(SampleName);

        Setup();
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnsupportedShaders();
        DrawGizmos();
        lighting.Cleanup();
        Submit();
    }
    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing) {
        var sortingsettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };
        var drawingsettings = new DrawingSettings(unlit, sortingsettings) {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing,
            perObjectData = PerObjectData.Lightmaps | PerObjectData.LightProbe |
            PerObjectData.ShadowMask| PerObjectData.OcclusionProbe | PerObjectData.OcclusionProbeProxyVolume|
                PerObjectData.LightProbeProxyVolume
        };
        drawingsettings.SetShaderPassName(1, litShaderTagId);
        var filteringsettings = new FilteringSettings(RenderQueueRange.opaque);

        context.DrawRenderers(cullingres, ref drawingsettings, ref filteringsettings);

        context.DrawSkybox(camera);

        sortingsettings.criteria = SortingCriteria.CommonTransparent;
        drawingsettings.sortingSettings = sortingsettings;
        filteringsettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(cullingres, ref drawingsettings, ref filteringsettings);
    }
    void Submit() {
        cmdbuf.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void Setup() {
        context.SetupCameraProperties(camera);
        CameraClearFlags flags = camera.clearFlags;
        cmdbuf.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ?
                camera.backgroundColor.linear : Color.clear);
        cmdbuf.BeginSample(SampleName);
        ExecuteBuffer();
        
    }
    void ExecuteBuffer() {
        context.ExecuteCommandBuffer(cmdbuf);
        cmdbuf.Clear();
    }

    bool Cull(float maxShadowDistance) {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p)) {
            p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
            cullingres = context.Cull(ref p);
            return true;
        }

        return false;
    }



}

