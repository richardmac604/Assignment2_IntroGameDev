Shader "Custom/FogEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 3D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogDensity ("Fog Density", Range(0, 1)) = 0.1
        _EnableFog ("Enable Fog", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _FogColor;
            float _FogDensity;
            float _EnableFog;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
              
                fixed4 texColor = tex2D(_MainTex, i.uv);

             
                float distance = length(i.worldPos - _WorldSpaceCameraPos);
                float fogFactor = exp(-_FogDensity * distance);

               
                fogFactor = saturate(fogFactor);
                float finalFog = lerp(1, fogFactor, _EnableFog);

              
                fixed4 finalColor = lerp(_FogColor, texColor, finalFog);

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
