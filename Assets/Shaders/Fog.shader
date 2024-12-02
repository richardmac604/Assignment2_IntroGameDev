Shader "Custom/FogShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogDensity ("Fog Density", Float) = 0.1
        _FogStart ("Fog Start", Float) = 10.0
        _FogEnd ("Fog End", Float) = 50.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _FogDensity;
            float _FogStart;
            float _FogEnd;
            float4 _FogColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float CalculateFogFactor(float distance)
            {
                float fogFactor = (distance - _FogStart) / (_FogEnd - _FogStart);
                fogFactor = saturate(fogFactor);
                return fogFactor;
            }

            half4 frag(v2f i) : SV_Target
            {
                float distance = length(i.worldPos - _WorldSpaceCameraPos);
                float fogFactor = CalculateFogFactor(distance);
                half4 col = _FogColor;
                col.rgb = lerp(col.rgb, _Color.rgb, 1.0 - fogFactor);
                col.a = 1.0;
                return col;
            }
            ENDCG
        }
    }
}
