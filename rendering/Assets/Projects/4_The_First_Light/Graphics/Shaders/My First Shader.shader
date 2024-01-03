Shader "Custom/My First Lighting Shader"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST; // Getting tiling and offset information

            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                i.uv = TRANSFORM_TEX(v.uv, _MainTex); // This appends _ST during the pre-processing step
                i.position = UnityObjectToClipPos(v.position);
                i.normal = v.normal;
                return i;
            }

            float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                return float4(i.normal * 0.5 + 0.5, 1);
            }
            ENDCG
        }
    }
}