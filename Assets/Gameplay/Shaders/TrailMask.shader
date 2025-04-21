Shader "Custom/TrailMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MaskPosition ("Mask Position", Vector) = (0,0,0,0)
        _MaskRadius ("Mask Radius", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;
            float4 _MaskPosition;
            float _MaskRadius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Вычисляем расстояние от текущей позиции до позиции маски
                float dist = distance(i.worldPos, _MaskPosition.xyz);
                
                // Проверяем, находится ли точка внутри радиуса маски
                if (dist > _MaskRadius)
                {
                    // Если вне маски — делаем точку прозрачной
                    discard;
                }
                return _Color;
            }
            ENDCG
        }
    }
}