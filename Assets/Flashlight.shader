Shader "Custom/FlashlightBeam"
{
    Properties
    {
        _BeamColor ("Beam Color", Color) = (1, 1, 1, 1)
        _BeamIntensity ("Beam Intensity", Range(0, 1)) = 1.0
        _BeamLength ("Beam Length", Float) = 5.0
        _BeamWidth ("Beam Width", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        #pragma target 3.0

        struct Input
        {
            float3 worldPos; // World position of the fragment
            float2 uv_MainTex; // UV coordinates
        };

        fixed4 _BeamColor;
        float _BeamIntensity;
        float _BeamLength;
        float _BeamWidth;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Radial distance from center of cone
            float radialDistance = length(IN.uv_MainTex - 0.5);

            // Length fade based on world position (distance along the cone)
            float lengthFade = saturate(1.0 - (IN.worldPos.z / _BeamLength));

            // Radial fade to taper the beam
            float radialFade = saturate(1.0 - (radialDistance * _BeamWidth));

            // Combine effects with intensity
            float beamEffect = _BeamIntensity * radialFade * lengthFade;

            // Ensure a minimum alpha for visibility
            beamEffect = max(beamEffect, 0.1);

            // Output color and transparency
            o.Albedo = _BeamColor.rgb * beamEffect;
            o.Alpha = _BeamColor.a * beamEffect;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
