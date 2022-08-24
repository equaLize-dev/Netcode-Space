Shader "Custom/UnlitTextureMix"
{
    Properties
    {
        _Tex1 ("Texture 1", 2D) = "white" {}
        _Tex2 ("Texture 2", 2D) = "white" {} 
        _MixValue("Mix Value", Range(0,1)) = 0.5 
        _Color("Main Color", COLOR) = (1,1,1,1) 
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" } 
        LOD 100 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert 
             #pragma fragment frag 
            #include "UnityCG.cginc"
                
            sampler2D _Tex1;
            float4 _Tex1_ST;
            sampler2D _Tex2;
            float4 _Tex2_ST;
            float _MixValue;
            float4 _Color;
            
            struct VectorToFragment
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            VectorToFragment vert (appdata_full v)
            {
                VectorToFragment result;
                result.vertex = UnityObjectToClipPos(v.vertex);
                result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
                return result;
            }

            fixed4 frag(VectorToFragment i) : SV_Target
            {
                fixed4 color;
                color = tex2D(_Tex1, i.uv) * _MixValue;
                color += tex2D(_Tex2, i.uv) * (1 - _MixValue);
                color = color * _Color;
                return color;
            }

            ENDCG
        }
    }
}
