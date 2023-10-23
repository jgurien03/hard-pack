Shader "Custom/EnhancedShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _AmbientColor ("Ambient Color", Color) = (0.1, 0.1, 0.1, 1)
        _DirectionalLightEnabled ("Directional Light Enabled", Range(0, 1)) = 0
        _CullingDistance ("Culling Distance", Range(0, 100)) = 50
        _Intensity ("Intensity", Range(0, 50)) = 0
        _DitherTex ("Dither Texture", 2D) = "white" {}
        _OutlineExtrusion("Outline Extrusion", float) = 0
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
    }
    SubShader {
        Tags {"RenderType"="Opaque"}
        LOD 100
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float3 toLightDir : TEXCOORD3; // Declare toLightDir here
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _AmbientColor;
            float _DirectionalLightEnabled;
            float _CullingDistance; // New culling distance
            float _Intensity;
            sampler2D _DitherTex; // New dither texture

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));

                // Get lighting from main light if it exists and directional light is enabled
                float3 lightDir = float3(0, 0, 0); // Default zero direction

                if (_DirectionalLightEnabled > 0) {
                    lightDir = normalize(UnityWorldSpaceLightDir(v.vertex));
                }

                float3 toEye = normalize(UnityWorldSpaceViewDir(v.vertex));

                // Apply diffuse lighting with ambient component
                float3 diffuseLighting = _Color.rgb * max(0, dot(o.normal, lightDir)) * _DirectionalLightEnabled;

                // Apply bloom effect by combining colors (you need to implement bloom separately)
                o.color = float4(_AmbientColor.rgb + diffuseLighting, 1.0);
                o.toLightDir = lightDir;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 foggedColor = i.color; // No fog effect, so color remains unchanged

                // Sample dither texture to determine dither amount
                float ditherAmount = tex2D(_DitherTex, i.uv).r;

                // Calculate smooth shading using dot product of normal and light direction
                float shading = smoothstep(0.2, 0.8, dot(i.normal, i.toLightDir));

                // Apply toon shading by dividing the shading into discrete steps
                shading = round(shading * 4) / 4;

                // Apply dithering using the dither amount
                fixed4 ditheredColor = foggedColor;
                if (ditherAmount > 0.5) {
                    ditheredColor.rgb += 0.1; // You can adjust this value for dither intensity
                } else {
                    ditheredColor.rgb -= 0.1; // You can adjust this value for dither intensity
                }

                // Calculate the blended color between toon shading and ambient color
                fixed4 ambientColor = _AmbientColor;
                fixed4 celShadedColor = fixed4(shading, shading, shading, 1);
                fixed4 blendedColor = lerp(ambientColor, ditheredColor * celShadedColor, .5); // You can adjust the blend factor here

                // Combine shading, dithering, and texture color
                fixed4 col = tex2D(_MainTex, i.uv) * blendedColor * _Intensity;
                return col;
            }
            ENDCG
        }
    }
}