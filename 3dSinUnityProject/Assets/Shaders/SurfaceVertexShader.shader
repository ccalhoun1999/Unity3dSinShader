Shader "Custom/SurfaceVertexShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _AnimSpeed("Anim Speed", Range(0,10)) = 1
        //_AnimSpeedY("Anim Speed (Y)", Range(0,5)) = 1
        _Amplitude("Amplitude", Range(0,15)) = 1
        //_AnimTiling("Anim Tiling", Range(0,100)) = 1
        _Frequency("Frequency", Range(0,2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow fullforwardshadows vertex:vert 
        //#pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        /* struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        }; */

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _AnimSpeed;
        float _Frequency;
        float _Amplitude;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v)
        {
            // v.vertex.y += sin((v.vertex.x + v.vertex.y) * _AnimTiling + _Time.y * _AnimSpeedX) * _AnimScale;
            // v.vertex.x += cos((v.vertex.x - v.vertex.y) * _AnimTiling + _Time.y * _AnimSpeedY) * _AnimScale;

            v.vertex.y = _Amplitude * sin(_Frequency * sqrt(v.vertex.x*v.vertex.x + v.vertex.z*v.vertex.z) + _Time.y * _AnimSpeed);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
