Shader "Custom/ShikakuKirakira"
{
    Properties
    {
        _Speed ("speed", Float) = 2
        _Scale ("scale", Float) = 10
        _Size ("size", Float) = 0.5
    }
    CGINCLUDE
        #include "UnityCG.cginc"
        #include "Random.cginc"

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
            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
            o.uv = v.uv;
            return o;
        }
        
        half _Speed, _Scale, _Size;

        half4 shikaku(half2 uv, half2 delta){
            half2 pivot = floor(uv+delta);
            half
                t = _Time.y*_Speed - rand(pivot),
                ft = floor(t);
            half4 rct = half4(
                pivot.x+rand(pivot+half2(1.9,2.9) + ft),
                pivot.y+rand(pivot+half2(3.9,4.9) + ft),
                rand(pivot+half2(5.9,6.9) + ft) * _Size,
                frac(t)
            );
            half val = rct.x < uv.x;
            val *= rct.y < uv.y;
            val *= uv.x < rct.x+rct.z;
            val *= uv.y < rct.y+rct.z;
            return val*rct.w;
        }
        half4 frag (v2f i) : SV_Target
        {
            half2 uv = i.uv * half2(_ScreenParams.x*(_ScreenParams.w-1),1.0);
            uv *= _Scale;
            half val = 0;
            val += shikaku(uv, half2(-1,-1));
            val += shikaku(uv, half2( 0,-1));
            val += shikaku(uv, half2(-1, 0));
            val += shikaku(uv, half2( 0, 0));
            return val;
        }
    ENDCG
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}