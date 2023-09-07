Shader "Custom/PongBoundary"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        // Boundary Properties
        _PosX ("Position X", Float) = 0.5
        _LineThickness ("Dash Line Thickness", Float) = 0.02
        _Length ("Dash Line Length", Float) = 0.1
        _Spacing ("Dash Line Spacing", Float) = 0.075
        _LinkDecimalPrecision ("Spacing Decimal Precision", Integer) = 3

        // Color Properties
        _DashColor ("Dash Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BackgroundColor ("Background Color", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Shader Property: the actual texture
            sampler2D _MainTex;

            // Boundary Properties
            float _PosX;
            float _LineThickness;
            float _Length;
            float _Spacing;
            int _LinkDecimalPrecision;

            // Color Properties
            float4 _DashColor;
            float4 _BackgroundColor;

            fixed4 frag (v2f i) : SV_Target
            {
                float toleranceX = _LineThickness / 2.0;

                if (i.uv.x >= _PosX - toleranceX && i.uv.x <= _PosX + toleranceX) { // if within the x boundaries
                    float linkY = _Length + _Spacing;
                    float scalar = pow(10.0, float(_LinkDecimalPrecision));

                    if (uint(i.uv.y * scalar) % uint(linkY * scalar) <= uint(_Length * scalar)) { // if pixel's y value is approximately eligible to be part of a dash
                        return fixed4(_DashColor);
                    }
                }

                return fixed4(_BackgroundColor);
            }
            ENDCG
        }
    }
}
