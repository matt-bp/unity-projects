Shader "Custom/Always On Top"
{
    Properties
    {
        _Tint ("Tint", Color) = (0, 0, 0, 1)
    }
    
    SubShader
    {
        ZTest Always
        
        Pass
        {
            HLSLPROGRAM

            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram

            #include "UnityCG.cginc"

            float4 _Tint;

            float4 VertexProgram (float4 position : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(position);
            }
            
            float4 FragmentProgram() : SV_TARGET
            {
                return _Tint;
            }
            
            ENDHLSL
        }
    }
}