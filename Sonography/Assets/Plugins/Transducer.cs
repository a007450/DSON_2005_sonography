using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

/* 
	root_points -- loat at target for transducer
	surface point -- name of gameobject points same as image to be loaded

*/

public class Transducer : MonoBehaviour {
	
	public GameObject surface_points;
	public GameObject root_points;
	
	public Color linecolor = Color.blue;
	
	public int current_i = 1;
	public float speed = 10;
	
	public string imgPrefix = "carotid/Image";
		
	private float[] pos_y_ref;	
	private Transform[] surfaces;	
	private Image canvas_image;
	
	private Sprite[] images;
		
	void Start(){
		surfaces = surface_points.GetComponentsInChildren<Transform>();
		pos_y_ref = new float[surfaces.Length];
		canvas_image = GameObject.Find("Canvas/Image").GetComponent<Image>();	
		
		// load associated images -- images in Resources folder
		int i = 0;
		images = new Sprite[surfaces.Length];
		foreach(Transform s in surfaces) {
			if (s.parent == surface_points.transform){
				string imgname = imgPrefix + s.name;
				Sprite sp = Resources.Load<Sprite>(imgname);
				
				if(sp){
					images[i] = sp;	
				};
				
				pos_y_ref[i] = s.position.y;	
			}
			i++;
		}
		
		
	}
	
	void OnDrawGizmos(){
		// debug draw line btn surface points and target points
		surfaces = surface_points.GetComponentsInChildren<Transform>();
		Transform[] targets = root_points.GetComponentsInChildren<Transform>();
		
		pos_y_ref = new float[surfaces.Length];	
		
		if (surfaces.Length == targets.Length && surfaces.Length !=0){
						
			int i = 0;
			foreach(Transform s in surfaces) {
				
				if (s.parent == surface_points.transform){
					Transform t = targets[i];				
					Gizmos.color = linecolor;
					Gizmos.DrawLine (s.position, t.position);		
					s.LookAt(t);
					pos_y_ref[i] = s.position.y;							
				}
				i++;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		surfaces = surface_points.GetComponentsInChildren<Transform>();
		float max =  surfaces.Length-1;
		current_i = (int) Mathf.Clamp(current_i, 1, max);
				
		Transform s_point = surfaces[current_i];
		
		Transform[] ts_ = root_points.GetComponentsInChildren<Transform>();
		Transform targ = ts_[current_i].transform;
		
		Transform transducer_model = transform.Find("model_holder");
				
        transform.position = s_point.position;
        transform.LookAt(targ);
        transducer_model.localEulerAngles = new Vector3(0,0,targ.localEulerAngles.z);
       	
       	if(canvas_image.sprite){
       		
       		if(canvas_image.sprite.name  != "Image"+ surfaces[current_i].name){
       			
       			canvas_image.sprite = images[current_i];
       		};
       	}
	}
	
	private Vector3 screenPoint;
	private Vector3 offset;
		
	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
		
	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		
		float nearest_dist = Mathf.Abs(transform.position.y - cursorPosition.y);
		int i = 0;
		
		foreach(float ypos in pos_y_ref){
			
			if ( Mathf.Abs(ypos - cursorPosition.y) < nearest_dist && Mathf.Abs(current_i - i) < 2) {
				
				if (current_i != i){
					current_i = i;
					
				}
			}
			
			i++;
		};
		
		Application.ExternalEval("console.log('dragging'); ");
		
	}
	
}
