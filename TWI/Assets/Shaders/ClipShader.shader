    Shader "Custom/ClipShader" {
        Properties {
            _MainTex ("Color", 2D) = "white" {}
            _Cutoff ("Threshold", Range (0,1)) = 0.5
            _Fade ("Opacity", Range (0, 1)) = 1
        }
        SubShader {
          Tags { "Queue"="Transparent" "RenderType"="Transparent" }
          Lighting Off
          CGPROGRAM
          #pragma surface surf Lambert alpha
          struct Input {
              float2 uv_MainTex;
          };
          sampler2D _MainTex;
          float _Cutoff;
          float _Fade;
       
          void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            clip (c.a - _Cutoff);
            o.Albedo = c.rgb;
            o.Alpha =  _Fade;
          }
          ENDCG
        }
    }