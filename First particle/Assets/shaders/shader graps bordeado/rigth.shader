Shader "Example/FresnelEdgeDetectionG2"
{
    Properties
    { 
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Edge Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal       : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float fresnel       : COLOR;
            };
            
            sampler2D _MainTex;
            float4 _Color;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                float3 viewVector = TransformWorldToObject (_WorldSpaceCameraPos) - IN.positionOS.xyz;
                OUT.fresnel = dot(IN.normal, normalize(viewVector));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return lerp(_Color, tex2D(_MainTex,IN.uv), smoothstep(0.2f,0.25f,IN.fresnel));
            }
            ENDHLSL
        }
    }
}