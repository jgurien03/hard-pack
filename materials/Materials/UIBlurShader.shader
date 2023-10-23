Shader "Custom/UIBlurShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0.0, 10.0)) = 1.0
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = half4(0, 0, 0, 0);
                int blurPixels = 5; // Adjust this for blur intensity

                for (int x = -blurPixels; x <= blurPixels; x++)
                {
                    for (int y = -blurPixels; y <= blurPixels; y++)
                    {
                        float2 offset = float2(x, y) * _BlurSize;
                        col += tex2D(_MainTex, i.uv + offset);
                    }
                }

                col /= ((2 * blurPixels + 1) * (2 * blurPixels + 1));
                return col;
            }
            ENDCG
        }
    }
}