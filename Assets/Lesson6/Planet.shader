Shader "Custom/Planet"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Bumpmap", 2D) = "bump" {}

        _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower("Rim Power", Range(0.5,8.0)) = 3.0

        _FogColor("Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert        

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
            half fog;
        };

        sampler2D _MainTex;
        sampler2D _BumpMap;

        float4 _RimColor;
        float _RimPower;

        fixed4 _Color;

        fixed4 _FogColor;

        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            float4 hpos = UnityObjectToClipPos(v.vertex);
            hpos.x /= hpos.w;
            data.fog = min(2, dot(hpos.x, 2) * 8);
        }

        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 fogColor = _FogColor.rgb;
            #ifdef UNITY_PASS_FORWARDADD
            fogColor = 0;
            #endif
            color.rgb = lerp(color.rgb, fogColor, IN.fog);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Alpha = c.a;

            half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
