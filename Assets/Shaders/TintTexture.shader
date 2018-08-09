Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
LOD 100
ZWrite Off
Blend SrcAlpha OneMinusSrcAlpha
Cull Off
Pass{
	CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
	struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
	float4 color : COLOR0;
};
struct v2f {
	float4 vertex : SV_POSITION;
	float2 texcoord : TEXCOORD0;
	float4 color : COLOR0;
};
sampler2D _MainTex;
float4 _MainTex_ST;
v2f vert(appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.texcoord = v.texcoord;
	o.color = v.color;
	return o;
}
fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.texcoord);
return col;
}
ENDCG
}