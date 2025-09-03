Shader "Custom/OutlineOnly"
{
    Properties
    {
        _Width("Width", Range(0,0.1)) = 0.02
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
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

            float _Width;

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz + worldNormal * _Width;
                o.pos = UnityWorldToClipPos(float4(worldPos, 1.0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (_Width <= 0.0001)
                    return fixed4(0,0,0,0); // Transparente total (apagado)
                return fixed4(0.5,0.5,0.5,1);     // Borde negro visible
            }
            ENDHLSL
        }
    }
    FallBack Off
}
