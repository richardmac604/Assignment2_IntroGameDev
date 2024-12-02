Shader "Custom/DayNightToggle"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DayColor ("Day Color", Color) = (1, 1, 1, 1)
        _NightColor ("Night Color", Color) = (0, 0, 0.2, 1)
        _BlendFactor ("Blend Factor", Range(0, 1)) = 0
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

            sampler2D _MainTex;
            float4 _DayColor;
            float4 _NightColor;
            float _BlendFactor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Interpolate between day and night colors
                fixed4 finalColor = lerp(_NightColor, _DayColor, _BlendFactor);

                return texColor * finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
