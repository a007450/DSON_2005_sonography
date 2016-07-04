// https://en.wikibooks.org/wiki/Cg_Programming/Unity/Cutaways
Shader "Custom/CrossSection2" {
	Properties {
		section_depth ("section depth (x, y, z, depth)", vector) = (0,90,0,5)
		section_depth2 ("depth2",float) = 0
		_Color ("section color", color) = (1, 1, 1, 0)
		color_map ("color map", 2D) = "white" {}
	}
	SubShader {

      // first pass (is executed before the second pass)
      Pass {
         Cull Front // cull only front faces
 
         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment fragment_shader 
         
         uniform float4 section_depth;
		 uniform float section_depth2;
		 uniform float4 _Color;
		 uniform sampler2D color_map;
		 
		 float4x4 rotate(float3 r) { 
			float3 c, s; 
			sincos(r.x, s.x, c.x); 
			sincos(r.y, s.y, c.y); 
			sincos(r.z, s.z, c.z);
			return float4x4( c.y*c.z,	 -s.z,     	s.y, 		0, 
							 	 s.z, 	c.x*c.z,    -s.x, 		0, 
								-s.y,     s.x, 		c.x*c.y,	0, 
								   0,       0,       0, 		1 );
		}
			
         struct vertexInput {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD;
         };
         
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posInObjectCoords : TEXCOORD0;
            float4 normal	: TEXCOORD1; 
			float4 vertex	: TEXCOORD2;
			float4 mask		: TEXCOORD3;
         };
          			
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
            float4x4 r = rotate(radians(section_depth.xyz));
            float4 c = float4(input.vertex.xyz, 1);
            output.mask = mul(r,c);
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.posInObjectCoords = input.vertex; 
            return output;
         }
 
         void fragment_shader( vertexOutput input, out float4 finalcolor : COLOR ) {
			if(input.posInObjectCoords.z > section_depth.w){
					discard;
			}				
				
			float diffuse = .3;
				
			finalcolor = _Color;//float4(0,0,0,1);								
		
		 }
 
         ENDCG  
      }

      // second pass (is executed after the first pass)
      Pass {
         Cull Back // cull only back faces

         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment fragment_shader 
         
         uniform float4 section_depth;
		 uniform float section_depth2;
		 uniform float4 _Color;
		 uniform sampler2D color_map;
		  
         struct vertexInput {
            float4 vertex : POSITION;
            float4 color	: COLOR;
			float2 texcoord : TEXCOORD;
			float3 normal	: NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float2 texcoord : TEXCOORD;
            float4 posInObjectCoords : TEXCOORD0;
            float3 normal	: TEXCOORD1; 
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.texcoord = input.texcoord;
            output.normal = input.normal;
            output.posInObjectCoords = input.vertex; 
 
            return output;
         }
         
         void fragment_shader( vertexOutput input, out float4 finalcolor : COLOR ) {
			if(input.posInObjectCoords.z > section_depth.w){
					discard;
			}				
				
			float diffuse = .6;
				
			finalcolor = float4(0,0,0,1);								
			finalcolor.xyz = tex2D(color_map, input.texcoord).xyz *(diffuse);
		 }
		 
         ENDCG  
      }
   }
  
	Fallback "Diffuse" // SubShader
}
