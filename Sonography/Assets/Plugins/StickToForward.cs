using UnityEngine;
using System.Collections;

public class StickToForward : MonoBehaviour {
	
	private bool setupMode = false;
	private Collider surface;
	
	void Start(){
		surface = GameObject.Find("Model Holder/Skin and eyes/Skin").GetComponent<Collider>();
	
		// move outside of skin
		if (setupMode){
			Vector3 initpos = transform.localPosition;
			transform.localPosition = initpos - transform.TransformDirection(1.2f*Vector3.forward);	
			StickToSkinRaycast();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
    	if (setupMode){
    		StickToSkinRaycast();
    	}
    }
    
    void StickToSkinRaycast(){
    	Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
         
        if (Physics.Raycast(transform.localPosition, fwd, out hit)){
        	//Debug.Log(hit.distance); 
        	if (hit.distance > 0 && hit.collider == surface){
           	    Vector3 startPos = transform.localPosition;
           	    Vector3 endPos = startPos + transform.TransformDirection(new Vector3(0,0,hit.distance));
           	    
           	  transform.localPosition = endPos;			
           	             		
           	};    
                             
        } 
        
    }
}
