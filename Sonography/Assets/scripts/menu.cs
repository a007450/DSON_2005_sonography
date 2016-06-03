using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required when Using UI elements.

public class menu : MonoBehaviour {
	public Image canvas_image;
	public Sprite noimage;
	public GameObject[] menuItems;
	public GameObject circleOfWillis;	
	public GameObject holder;
	public GUISkin mySkin;
	public Texture help_icon;
	
	// script references	
	private SelectableObject[] cow_so;
	private ShowHideParts showHideParts;
	
	// transducer choices
	public GameObject TransducerPaths;
	private GameObject[] pathChoices;  			// GOs tagged as "Path" -- more efficient than looping through TransducerPaths
	private int pathChoicesInt = 0;
	private int oldPathChoicesInt = -1; 			// to detect on change
    private string[] pathChoicesStrings;
	
	// GUI vars
	private bool hilightCoW = false;	
	private float originalWidth = 1024;  		// define here the original resolution
 	private float originalHeight = 650f;  
 	private Vector3 scale;  					// Auto scale UI elements
   	private bool help = false;
   	
	void Start(){
		
		showHideParts = gameObject.AddComponent<ShowHideParts>();	
		
		// find all transparent tagged items -- turn off colliders and set to transparent
		GameObject[] transparents = GameObject.FindGameObjectsWithTag("Transparent");
		
		foreach (GameObject go in transparents){
			ToggleColliders(go);	
			showHideParts.MakeTransparent(go);
		}
		
		// store all circle of willis renderers
		cow_so = circleOfWillis.GetComponentsInChildren<SelectableObject>();
		
		// Transducer paths 
		pathChoices = GameObject.FindGameObjectsWithTag("Path");
		pathChoicesStrings = new string[pathChoices.Length];
		int i =0;
		foreach(GameObject go in pathChoices){
			pathChoicesStrings[i] = go.name;				
			i++;
		}
		
		Array.Sort(pathChoicesStrings);
		Array.Resize(ref pathChoicesStrings, pathChoicesStrings.Length + 1);
		pathChoicesStrings[pathChoicesStrings.Length-1]= "None" ;		
	}
	
	void OnGUI() {
				
		GUI.skin = mySkin;
		
		scale.x = Screen.width/originalWidth; 	// calculate hor scale
     	scale.y = Screen.height/originalHeight; // calculate vert scale
     	scale.z = 1;
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
				
		// model show/hide parts options
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Anatomical Layers", GUILayout.Width(200));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		foreach( GameObject go in menuItems) {
			string btnStyle = (go.tag == "Untagged")? "button" : "off";
			
			if ( GUILayout.Button(go.name, new GUIStyle(btnStyle) ) ){
				help = false;
				go.tag = (go.tag == "Untagged")? "Transparent" : "Untagged";
				
				ToggleColliders(go);	
				
				if (go.tag == "Untagged") {
					showHideParts.MakeOpaque(go);	
				}
				if (go.tag == "Transparent") {
					showHideParts.MakeTransparent(go);	
				}			
				
			};
				
		};
		GUILayout.EndHorizontal();
		
		string cow_btnstyle = (hilightCoW)? "button": "off";	
		string cowBtnTxt = "Highlight Circle of Willis";			
		
		if ( GUILayout.Button(cowBtnTxt, new GUIStyle(cow_btnstyle) ) ){
			hilightCoW = !hilightCoW;
			
			// tag all circle of willis gos
			foreach(SelectableObject so in cow_so){	
				if (hilightCoW){
					so.Hilight();
				}else{
					so.Unhilight();
				}			
			}	
			
		};				
		
		// transducer -- choose path
		GUILayout.Space(10);
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Transducer Paths", GUILayout.Width(200));
			
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		pathChoicesInt = GUILayout.SelectionGrid(pathChoicesInt, pathChoicesStrings, 3, new GUIStyle("toggle"));
		if (oldPathChoicesInt != pathChoicesInt){
			if (pathChoicesInt != pathChoices.Length) {
				foreach(GameObject go in pathChoices){
					TransducerPaths.SetActive(true);
					go.SetActive(go.name == pathChoicesStrings[pathChoicesInt]);
				}	
				
				help = false;
			}else{ // none
				TransducerPaths.SetActive(false);
				canvas_image.sprite = noimage;
			}
			oldPathChoicesInt = pathChoicesInt;
			
		}
		
		GUILayout.EndHorizontal();
		
		// help
		help = GUI.Toggle(new Rect(Screen.width-45,0,40,40), help, help_icon, new GUIStyle("help") );
				
		if(help){
			GUI.TextArea(new Rect(367, 15, 250, 50), "Click each group to toggle the visibility of the Anatomical Layers.");
			GUI.TextArea(new Rect(390, 120, 250, 70), "Select a path.\nClick and drag the transducer up and down to view images along the path.");		
			GUI.TextArea(new Rect(Screen.width-500,Screen.height-40,500, 40), "Right click and drag to rotate. Arrow keys to pan. Scrollwheel to zoom.");
		}
		
		GUI.matrix = svMat; // restore matrix
		
	}
	
	void ToggleColliders(GameObject go){
		
		// turn off/on collider
		Collider[] cols = go.GetComponentsInChildren<Collider>();
		foreach( Collider col in cols){
			
			col.enabled = (go.tag != "Transparent");	
			
		}
	}
}
