Shader "Custom/Night"
{
    Properties
    {
        _NightColor ("Night Color", Color) = (0.0, 0.0, 0.2, 1)
        _NightDensity ("Night Density", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "NightEffect"
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _NightColor;
            float _NightDensity;

            v2f vert (float4 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Compute distance from camera
                float distance = length(i.worldPos - _WorldSpaceCameraPos);
                float NightFactor = 1.0 - exp(-_NightDensity * distance);

                // Apply fog color
                return lerp(fixed4(0, 0, 0, 0), _NightColor, NightFactor);
            }
            ENDCG
        }
    }
}
