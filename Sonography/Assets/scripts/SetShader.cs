using UnityEngine;
using System.Collections;

public class SetShader : MonoBehaviour {
	public float[] values;
	public Transform[] targets;
	public Transform lookat;
	
	private float w_ = 4.81f;
		
	// GUI
	public GUISkin mySkin;
	private string[] sections ;
	private int sectionsInt = 0;
	
	// Use this for initialization
	void Start () {
		int i = 0;
		SetSection( values[i] );
		sections = new string[values.Length];
		foreach(float val in values){
			i++;
			sections[i-1] = "" + i + "";
		}
		
		
	}
	
	// Update is called once per frame
	private int oldInt = 0;
	void OnGUI () {
		GUI.skin = mySkin;
		sectionsInt = GUILayout.SelectionGrid(sectionsInt, sections, 1);
		if (oldInt != sectionsInt){		
			SetSection( values[sectionsInt] );
			oldInt = sectionsInt;
			Transform targ = targets[sectionsInt];
			lookat.position =  Vector3.MoveTowards(targ.position, targ.position, 10* Time.deltaTime);
		}
				
	}
	
	void SetSection(float new_w){
		
		Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
		w_ = new_w;
		
		foreach(Renderer re in rends){
			Material[] mats = re.materials;
			
			foreach(Material m in mats){
				if(m.shader.name == "Custom/CrossSection2" || m.shader.name == "Custom/CrossSection" ){	
					Vector4 v = m.GetVector("section_depth");
					v.w = w_;
					m.SetVector("section_depth", v);
				}
			}	
		}
		
	}
	
}
