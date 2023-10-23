#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes {
    float3 positionOS : POSITION;
    float3 normalOS : NORMAL;
};

struct Interpolators {
    float4 positionCS : SV_POSITION;
    float3 vertexPositionWS : TEXCOORD0; // Store vertex positions in interpolator
};

// These are set by Unity for the light currently "rendering" this shadow caster pass
float3 _LightDirection;


// This function offsets the clip space position by the depth and normal shadow biases
float4 GetShadowCasterPositionCS(float3 positionWS, float3 normalWS) {
    float3 lightDirectionWS = _LightDirection;
    // From URP's ShadowCasterPass.hlsl
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
    // We have to make sure that the shadow bias didn't push the shadow out of
    // the camera's view area. This is slightly different depending on the graphics API
#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif
    return positionCS;
}

Interpolators Vertex(Attributes input) {
    Interpolators output;

    VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS); // Found in URP/ShaderLib/ShaderVariablesFunctions.hlsl
    VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS); // Found in URP/ShaderLib/ShaderVariablesFunctions.hlsl

    output.positionCS = GetShadowCasterPositionCS(posnInputs.positionWS, normInputs.normalWS);
    output.vertexPositionWS = posnInputs.positionWS; // Store vertex position in interpolator
    return output;
}

float4 Fragment(Interpolators input) : SV_TARGET {
    // Here, you can check if the current fragment's position matches any of the vertex positions
    // and return a specific color or value accordingly. For example:
    
    float threshold = 0.001; // Adjust this threshold as needed
    
    if (length(input.positionCS.xyz - input.vertexPositionWS) < threshold) {
        // This fragment corresponds to a vertex position, so return a specific color or value
        return float4(1.0, 0.0, 0.0, 1.0); // Red color for vertex positions
    }
    return float4(0.0, 0.0, 0.0, 0.0); // Transparent
}