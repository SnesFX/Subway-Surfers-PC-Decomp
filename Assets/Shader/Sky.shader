Shader "Custom/Sky" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Background" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Background" "RenderType"="Opaque" }
  ZWrite Off
  Cull Front
  Fog { Mode Off }
  SetTexture [_MainTex] { combine texture }
 }
}
Fallback "Diffuse"
}