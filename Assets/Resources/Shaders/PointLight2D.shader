Shader "Hidden/PointLight2D"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "black"{}
        _Color("Color", Color) = (1,1,1,1)
        _Intensity("Intensity", Float) = 1
        _Exp("Exp", Float) = 1
        _Coords("Coords", Vector) = (0, 0, 0, 0)//x, y, size, aspect ratio
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _Coords;
            float _Intensity;
            float _Exp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float sqr_dist(float2 a, float2 b){
                float x = a.x - b.x;
                float y = a.y - b.y;
                return x * x + y * y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //sampling the screen texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 p = _Coords.xy; //light source position, in screen space

                //adjusting the uv to draw a circle indepent of aspect ratio
                i.uv -= .5;
                p -= .5;
                i.uv.x *= _Coords.w;
                p.x *= _Coords.w;
                
                float d = saturate(distance(i.uv, p)); //distance field for the light influence
                
                d = 1 - smoothstep(0, _Coords.z, d);
                d = pow(d, _Exp);
                return col + d * _Color * _Intensity;
            }
            ENDCG
        }
    }
}
