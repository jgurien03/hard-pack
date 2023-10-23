Shader "Hidden/SSFog" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1; // Store screen position
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _FogColor = float4(0.5, 0.2, 0.1, 1.0); // Adjust fog color
            float _FogDensity = 0.05; // Increase fog density
            float _FogOffset = -10.0; // Adjust fog offset
            float _FogStrength = 5.0; // Increase fog strength

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Store the screen position for later use
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                // Sample the main texture
                half4 texColor = tex2D(_MainTex, i.uv);

                // Sample the camera depth texture
                half depthValue = tex2D(_CameraDepthTexture, i.screenPos.xy / i.screenPos.w).r;

                // Convert depth value from linear to view space
                float depthLinear = LinearEyeDepth(depthValue);

                // Calculate fog factor based on depth
                half fogFactor = saturate((depthLinear - _FogOffset) * _FogDensity);

                // Adjust fogFactor to control the fog strength
                fogFactor *= _FogStrength;

                // Modify lerp to make fog color more dominant
                half4 finalColor = lerp(texColor, _FogColor, fogFactor * fogFactor);

                return finalColor;
            }
            ENDCG
        }
    }
}