using UnityEngine;
using System.Collections;

public class ShowHideParts  : MonoBehaviour {
		 
	// Shader RenderingMode Opaque (0) <--> Transparent (3)
	// (0, 1, 2, 3) it maps to Opaque, Cutout, Fade and transparent
		
	public void MakeOpaque(GameObject go) {
		foreach(Renderer re in go.GetComponentsInChildren<Renderer>() ){
			int mat_i = 0;
			foreach (Material m in re.materials) {
				m.SetFloat("_Mode", 0);
				m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				m.SetInt("_ZWrite", 1);
				m.DisableKeyword("_ALPHATEST_ON");
				m.DisableKeyword("_ALPHABLEND_ON");
				m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				m.renderQueue = 1;
							
				mat_i++;
			}
		}
	}
	
	public void MakeTransparent(GameObject go) {
		foreach(Renderer re in go.GetComponentsInChildren<Renderer>() ){
			int mat_i = 0;
			foreach (Material m in re.materials) {
				//Texture tex = m.GetTexture("_MainTex");
				//m.CopyPropertiesFromMaterial(mat);
				//m.SetTexture("_MainTex", tex);
				m.SetFloat("_Mode", 3);
               	m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
               	m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
               	m.SetInt("_ZWrite", 0);
               	m.DisableKeyword("_ALPHATEST_ON");
               	m.EnableKeyword("_ALPHABLEND_ON");
               	m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
               	m.renderQueue = 3000;
				
				mat_i++;
			}
		}
	}
		
	// lagacy shader -- 
    public void ChangeMaterialColor(GameObject obj, Material[] original, string state) {
	  	
		foreach (Renderer objRenderer in obj.GetComponentsInChildren<Renderer>()) {
			int mat_i = 0;
			foreach (Material m in objRenderer.materials) {
				Color c = original[mat_i].color;
				Shader s = original[mat_i].shader;
				switch (state) {
					
					case "select":
						c.r = 2*original[mat_i].color.r;						
						c.g = 2*original[mat_i].color.g;
						c.b = 2*original[mat_i].color.b;
						c.a = m.color.a;		
						break;
					case "hilight":
						c.r = 2.5f*original[mat_i].color.r;						
						c.g = 2.5f*original[mat_i].color.g;
						c.b = 2*original[mat_i].color.b;
						c.a = m.color.a;		
						break;
					case "transparent":
						if (c.a < 1) {
							c.a = 1;
						}else{
							s = Shader.Find("Legacy Shaders/Transparent/Diffuse");	
							c.a = 0.5f;
						}
						
						break;
					default:
						// back to original
						m.shader = original[mat_i].shader;
						break;
											
				}
				
				m.shader = s;
				m.color =  c;
				mat_i++;
							
			}
			
		}	
	}
	
}

