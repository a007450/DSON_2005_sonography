var box: boolean;
var mesh: boolean;

function Awake() {
	if (box)
		AddBoxColliderToChildren();	
	else if (mesh)
		AddMeshColliderToChildren();
}

function AddBoxColliderToChildren() {
	for (var objRenderer : Renderer in GetComponentsInChildren.<Renderer>()) {
		
		objRenderer.gameObject.AddComponent.<BoxCollider>();	
		objRenderer.gameObject.AddComponent.<SelectableObject>();
	}
	
}

function AddMeshColliderToChildren() {
	for (var objRenderer : Renderer in GetComponentsInChildren.<Renderer>()) {
				
		if ( objRenderer.gameObject.GetComponent(Collider) == null){
			
			objRenderer.gameObject.AddComponent.<MeshCollider>();	
			
		}
		
		objRenderer.gameObject.AddComponent.<SelectableObject>();
		
		
	}
}
