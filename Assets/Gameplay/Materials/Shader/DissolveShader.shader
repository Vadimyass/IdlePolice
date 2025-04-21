Shader "Custom/DissolveSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}   // Текстура спрайта
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}  // Текстура шума
        _Dissolve ("Dissolve Amount", Range(0, 1)) = 0  // Значение растворения
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)  // Цвет контура
        _EdgeWidth ("Edge Width", Range(0, 0.2)) = 0.05  // Толщина контура
        _Color ("Tint", Color) = (1,1,1,1)  // Цвет из SpriteRenderer
    }

    SubShader
{
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 100
    Blend SrcAlpha OneMinusSrcAlpha
    ZWrite Off   // 🔹 Отключает запись в Z-буфер


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
                float4 color : COLOR;  // Получаем цвет SpriteRenderer
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float _Dissolve;
            float4 _EdgeColor;
            float _EdgeWidth;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;  // Умножаем на цвет из SpriteRenderer
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mainColor = tex2D(_MainTex, i.uv) * i.color;  // Берем текстуру и цвет
                float noise = tex2D(_DissolveTex, i.uv).r;  // Текстура шума

                // Край растворения
                float edge = smoothstep(_Dissolve - _EdgeWidth, _Dissolve, noise);

                // Применяем контур
                float4 finalColor = lerp(_EdgeColor, mainColor, edge);
                finalColor.a *= edge;  // Коррекция прозрачности

                return finalColor;
            }
            ENDCG
        }
    }
}
