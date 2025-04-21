Shader "Custom/RadialProgress"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0.0
        _Color ("Color", Color) = (1,1,1,1)
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

            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Progress;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5); // Центр круга
                float angle = atan2(i.uv.y - center.y, i.uv.x - center.x) / 6.2831853 + 0.75; // Нормализуем угол в диапазон [0,1] и корректируем начало
                
                if (angle > 1.0) angle -= 1.0; // Сдвиг для корректного отображения прогресса от 0 до 1

                if (angle > _Progress)
                    discard;

                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}