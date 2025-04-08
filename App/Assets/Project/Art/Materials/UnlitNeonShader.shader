Shader "Custom/UnlitNeonShader"
{
    Properties
    {
        _Color ("Color", Color) = (.5, .5, .5, 1)
        _EmissionColor ("Emission Color", Color) = (1, 0, 0, 1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            uniform float4 _Color;
            uniform float4 _EmissionColor;
            uniform sampler2D _MainTex;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag(v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv);
                half4 finalColor = texColor * _Color;
                finalColor += _EmissionColor;  // Add the emission color
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
