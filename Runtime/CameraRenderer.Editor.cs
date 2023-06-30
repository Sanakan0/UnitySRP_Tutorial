using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

partial class CameraRenderer {
    partial void DrawUnsupportedShaders();
    partial void DrawGizmos();
    partial void PrepareForSceneWindow();
    partial void PrepareBuffer();

#if UNITY_EDITOR
    string SampleName { get; set; }

    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
    static Material errormat;

    partial void DrawUnsupportedShaders() {
        if (errormat == null) {
            errormat = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        var drawingsettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera)) { overrideMaterial = errormat };
        for (int i = 1; i < legacyShaderTagIds.Length; i++) {
            drawingsettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringsettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingres, ref drawingsettings, ref filteringsettings);
    }

    partial void DrawGizmos() {
        if (Handles.ShouldRenderGizmos()) {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareForSceneWindow() {
        ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        if (camera.cameraType == CameraType.SceneView) {
            
        }
    }

    partial void PrepareBuffer() {
        cmdbuf.name = SampleName= camera.name;
    }

#else

    const string SampleName = bufname;

#endif
}
