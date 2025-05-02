Shader "Custom/StarrySky" {
    Properties {
        _MainTex ("Star Texture", 2D) = "white" {}
        _TwinkleSpeed ("Twinkle Speed", Float) = 2.0
        _Intensity ("Star Brightness", Float) = 1.5
        _BackgroundColor ("Background Color", Color) = (0.02, 0.05, 0.15, 1)
    }

    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _TwinkleSpeed;
            float _Intensity;
            float4 _BackgroundColor;
            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float flicker = sin(_Time.y * _TwinkleSpeed + i.uv.x * 10 + i.uv.y * 15) * 0.5 + 0.5;
                fixed4 stars = tex2D(_MainTex, i.uv);
                stars.rgb *= flicker * _Intensity;
                stars.a *= flicker;

                // Mezcla las estrellas con el fondo azul oscuro
                fixed4 finalColor = lerp(_BackgroundColor, stars, stars.a);
                return finalColor;
            }
            ENDCG
        }
    }
}
