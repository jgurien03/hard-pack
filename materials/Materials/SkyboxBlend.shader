Shader "Skybox/BlendedCubemapsWithExposure" {
    Properties {
        _Tint ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _Blend ("Blend", Range(0.0, 1.0)) = 0.5
        _Exposure1 ("Exposure 1", Range(0.1, 10.0)) = 1.0
        _Exposure2 ("Exposure 2", Range(0.1, 10.0)) = 1.0
        _CubeTex1 ("Cubemap 1", Cube) = "" {}
        _CubeTex2 ("Cubemap 2", Cube) = "" {}
    }

    SubShader {
        Tags { "Queue" = "Background" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float3 pos : TEXCOORD0;
                float4 pos4 : SV_POSITION;
            };

            float4 _Tint;
            float _Blend;
            float _Exposure1;
            float _Exposure2;
            samplerCUBE _CubeTex1;
            samplerCUBE _CubeTex2;

            v2f vert (appdata_t v) {
                v2f o;
                o.pos4 = UnityObjectToClipPos(v.vertex);
                o.pos = normalize(UnityObjectToWorldNormal(v.vertex));
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                half3 texColor1 = texCUBE(_CubeTex1, i.pos) * _Exposure1;
                half3 texColor2 = texCUBE(_CubeTex2, i.pos) * _Exposure2;
                half3 blendedColor = lerp(texColor1, texColor2, _Blend);
                blendedColor = lerp(blendedColor, _Tint.rgb, _Tint.a); // Apply tint color
                return half4(blendedColor, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Skybox/6 Sided"
}