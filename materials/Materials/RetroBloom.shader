Shader "Custom/BloomShader"
{
    Properties
    {
        _Bloom_Texture ("Bloom Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" }
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            sampler2D _Bloom_Texture;
 
            half4 frag(v2f i) : SV_Target
            {
                // Sample the bloom texture and apply it with intensity
                half4 bloomColor = tex2D(_Bloom_Texture, i.uv);
                return bloomColor; // You can apply bloom post-processing here if needed
            }
            ENDCG
        }
    }
}