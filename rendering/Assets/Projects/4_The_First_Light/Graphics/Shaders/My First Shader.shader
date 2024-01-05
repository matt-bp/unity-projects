Shader "Custom/My First Lighting Shader"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Pass
        {
            Tags 
            {
                "LightMode" = "ForwardBase"
            }
            
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            
            #include "UnityStandardBRDF.cginc" // For DotClamped, and other lighting functions

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST; // Getting tiling and offset information
            float _Smoothness;

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
                float3 worldPos: TEXCOORD2;
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex); // This appends _ST during the pre-processing step
                // i.normal = normalize(mul(transpose((float3x3)unity_ObjectToWorld), v.normal));
                // i.normal = normalize(mul(v.normal, transpose((float3x3)unity_ObjectToWorld)));
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.worldPos = mul(unity_ObjectToWorld, v.position);

                return i;
            }

            float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                i.normal = normalize(i.normal);
                // return float4(i.normal * 0.5 + 0.5, 1); // Visualize normal
                // return saturate(dot(float3(0, 1, 0), i.normal)); // Alternatively, use below
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                
                float3 lightColor = _LightColor0.rgb;
                float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
                float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);

                // Just diffuse reflection
                // return float4(diffuse, 1);

                // float3 reflectionDir = reflect(-lightDir, i.normal);
                float3 halfVector = normalize(lightDir + viewDir);
                
                // Visualizing reflection direction
                // return float4(reflectionDir * 0.5 + 0.5, 1);
                
                return pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
                
            }
            ENDCG
        }
    }
}