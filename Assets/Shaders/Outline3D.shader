Shader "Custom/OutlineOnly"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
        _PulseAmplitude ("Pulse Amplitude", Range(0, 0.1)) = 0
        _PulseSpeed ("Pulse Speed", Range(0, 20)) = 4
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        // Dibujamos SOLO el contorno: extruimos caras y renderizamos backfaces
        Cull Front
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float _PulseAmplitude;
            float _PulseSpeed;
            fixed4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;

                // Grosor + pulso opcional
                float width = _OutlineWidth + _PulseAmplitude * sin(_Time.y * _PulseSpeed);

                // Pasamos a view-space
                float4 posVS = mul(UNITY_MATRIX_MV, v.vertex);
                float3 nVS = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));

                // Extruimos a lo largo de la normal (en view-space para grosor más consistente en pantalla)
                posVS.xyz += nVS * width;

                // A clip space
                o.pos = mul(UNITY_MATRIX_P, posVS);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor; // Solo color de borde
            }
            ENDCG
        }
    }
    Fallback Off
}
