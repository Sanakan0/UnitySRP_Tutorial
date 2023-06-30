#ifndef CUSTOM_UNITY_INPUT_INCLUDED
#define CUSTOM_UNITY_INPUT_INCLUDED
CBUFFER_START(UnityPerDraw)
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4 unity_LODFade;
real4 unity_WorldTransformParams;

float4 unity_LightmapST;
float4 unity_DynamicLightmapST;

float4 unity_SHAr;
float4 unity_SHAg;
float4 unity_SHAb;
float4 unity_SHBr;
float4 unity_SHBg;
float4 unity_SHBb;
float4 unity_SHC;

float4 unity_ProbeVolumeParams;
float4x4 unity_ProbeVolumeWorldToObject;
float4 unity_ProbeVolumeSizeInv;
float4 unity_ProbeVolumeMin;

float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

float4 unity_ProbesOcclusion;

CBUFFER_END
bool4 unity_MetaFragmentControl;

float3 _WorldSpaceCameraPos;

//UNITY_INSTANCING_BUFFER_START(UnityPerDraw)

//UNITY_DEFINE_INSTANCED_PROP(float4x4, unity_ObjectToWorld)
//UNITY_DEFINE_INSTANCED_PROP(float4x4, unity_WorldToObject)
//UNITY_DEFINE_INSTANCED_PROP(float4, unity_LODFade)
//UNITY_DEFINE_INSTANCED_PROP(real4, unity_WorldTransformParams)
////float4x4 unity_ObjectToWorld;
////float4x4 unity_WorldToObject;
////float4 unity_LODFade;
////real4 unity_WorldTransformParams;
//UNITY_INSTANCING_BUFFER_END(UnityPerDraw)



float4x4 unity_MatrixVP;
float4x4 unity_MatrixV;
float4x4 glstate_matrix_projection;

float4x4 unity_MatrixPreviousM;
float4x4 unity_MatrixPreviousMI;
#endif