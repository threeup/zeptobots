Shader "Custom/Ramped" {
    Properties
    {
        _Color ("Color ", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
    }

    CGINCLUDE
        #include "UnityCG.cginc"
        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _Color2;
        
        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
        };


        float calculateRampCoefficient( float t, int stripeCount )
        {
            float fStripeCount = float(stripeCount);
            float modifiedT = fmod( floor( t * fStripeCount ), fStripeCount );
            float rampCoefficient = lerp( 0.1, 1.0, modifiedT / (fStripeCount-1.0) );
            
            return rampCoefficient;
        }
        
 
        v2f vert (appdata_full v)
        {
            half3 lightColor = ShadeVertexLights(v.vertex, v.normal);
            v.color.rgb = lightColor.rgb*_Color;
            
            v2f o;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            o.color = v.color;
            return o;
        }
        half4 frag (v2f i) : COLOR
        {
            half4 col =  i.color;
            col.r = calculateRampCoefficient(col.r, 4);
            col.g = calculateRampCoefficient(col.g, 4);
            col.b = calculateRampCoefficient(col.b, 4);
            return col;
        }
    ENDCG
    
    SubShader {
        Tags {"LightMode" = "Vertex"}  
        Pass {
            CGPROGRAM
            #pragma glsl
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            ENDCG 
        }
    }
}