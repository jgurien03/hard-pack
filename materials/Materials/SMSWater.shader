Shader "Custom/SMSWater" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Layer1Tex ("Layer 1 Texture", 2D) = "white" {}
        _Layer2Tex ("Layer 2 Texture", 2D) = "white" {}
        _Cutoff1 ("Cutoff 1", Range(0, 1)) = 0.3
        _Cutoff2 ("Cutoff 2", Range(0, 1)) = 0.5
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
            };
            
            sampler2D _MainTex;
            sampler2D _Layer1Tex;
            sampler2D _Layer2Tex;
            float _Cutoff1;
            float _Cutoff2;
            
            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                // Sample textures
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);
                fixed4 layer1TexColor = tex2D(_Layer1Tex, i.uv);
                fixed4 layer2TexColor = tex2D(_Layer2Tex, i.uv);
                
                // Calculate alpha values based on cutoff thresholds
                float alpha1 = (mainTexColor.a > _Cutoff1) ? 1.0 : 0.0;
                float alpha2 = (mainTexColor.a > _Cutoff2) ? 1.0 : 0.0;
                
                // Combine layers based on alpha values
                fixed4 finalColor = lerp(layer1TexColor, layer2TexColor, alpha2) * alpha1;
                
                return finalColor;
            }
            ENDCG
        }
    }
}