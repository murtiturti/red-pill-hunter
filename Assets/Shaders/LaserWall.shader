Shader "Unlit/LaserWall"
{
    Properties
    {
        _LaserColor ("Laser Color", Color) = (1, 0, 0, 1)
        _ClippedColor ("Clipped Color", Color) = (0.7, 0, 0, 1)
        _LineFrequency ("Line Frequency", Range(1,50)) = 10.0
        _LineThickness ("Line Thickness", Range(0,1)) = 0.1
        _FarThreshold ("Far Threshold", Range(10, 1000)) = 100

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
        
        // Enable blending for a glowing effect and disable depth write
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            fixed4 _LaserColor;
            fixed4 _ClippedColor;
            float _LineFrequency;
            float _LineThickness;
            float _FarThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist2Camera = length(_WorldSpaceCameraPos - i.worldPos);
                float repeatUV = frac(i.uv.y * _LineFrequency);

                // calculate distance from center
                float distance = abs(repeatUV - 0.5) / _LineThickness;
                distance = 1.0 - saturate(distance / 0.5);

                if(dist2Camera > _FarThreshold)
                {
                    // The player is far away – use a fallback effect or simply output _LaserColor.
                    return fixed4(_ClippedColor.xyz, distance);
                }

                float t = pow(distance, 6.0);
                
                fixed4 color = lerp(_LaserColor, fixed4(1, 1, 1, 1), t);
                color.a = distance;
                
                return color;
            }
            ENDCG
        }
    }
}
