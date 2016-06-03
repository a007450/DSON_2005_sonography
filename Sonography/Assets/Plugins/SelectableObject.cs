using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour
{
	private Rect m_SelectionWindowRect = new Rect (10.0f, 10.0f, 200.0f, 50.0f);
	private Material[] origMaterials; 
	private bool isHovering = false;		// chks for whether mouse is hovering over obj
	private GUISkin mySkin;
	
	private bool isHilight = false;			// if hilighted 
	private ShowHideParts ShowHideParts;
	
	private void Awake() {
		
		ShowHideParts = gameObject.AddComponent<ShowHideParts>();
			
		if (GetComponent<Renderer>()) {
			int i = 0;
			origMaterials = GetComponent<Renderer>().materials;
			foreach(Material m in GetComponent<Renderer>().materials){	// store object's original materials
				origMaterials[i] = new Material(m);			
				i++;
			}
		}
						
	}
	
	void Start()
	{
   	 	mySkin = Resources.Load("styles") as GUISkin;
	}
	
	public Material[] OriginalMaterials(){
		return origMaterials;	
	}
	
	public void Deselect(){
		ShowHideParts.ChangeMaterialColor(gameObject, origMaterials, "original");
		
	}
	
	public void Select(){
		ShowHideParts.ChangeMaterialColor(gameObject, origMaterials, "select");
		isHovering = true;
	}
	
	public void Hilight(){
		ShowHideParts.ChangeMaterialColor(gameObject, origMaterials, "select");
		isHilight = true;
	}
	public void Unhilight(){
		Deselect();
		isHilight = false;
	}
	
	private void OnMouseOver() {
		if(origMaterials.Length > 0){
			ShowHideParts.ChangeMaterialColor(gameObject, origMaterials, "select");
		}
		
		isHovering = true;
	}
		
	private void OnMouseExit() {
		if(!isHilight)
			Deselect();
		isHovering = false;
	}
	
	private void OnDisable ()
	{
		SelectManager.Deselect (gameObject);
	}
	
	private void OnGUI ()
	{				
		GUI.skin = mySkin;	
		if (isHovering)
		{
			string txt =  transform.parent.name; 
			Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(txt), "label");
			m_SelectionWindowRect.x = Input.mousePosition.x + 30;
			m_SelectionWindowRect.y = Screen.height-Input.mousePosition.y - 30;
			m_SelectionWindowRect.width = labelRect.width;
			m_SelectionWindowRect.height = labelRect.height;
						
			GUI.Label(m_SelectionWindowRect,txt);
		}
				
	}
		
	void SelectionWindow (int id)
	{
		GUI.DragWindow ();
	}
}

public class SelectManager
{
	private static GameObject s_ActiveSelection;
	
	public static GameObject ActiveSelection
	{
		get
		{
			return s_ActiveSelection;
		}
		set
		{
			s_ActiveSelection = value;
		}
	}
	
	public static void Select (GameObject gameObject, bool selectionValue)
	{
		if (selectionValue)
		{
			Select (gameObject);
		}
		else
		{
			Deselect (gameObject);
		}
	}
	
	public static void Select (GameObject gameObject)
	{
		ActiveSelection = gameObject;
	}
	
	public static void Deselect (GameObject gameObject)
	{
		if (ActiveSelection == gameObject)
		{
			ActiveSelection = null;
		}
	}
	
	public static bool IsSelected (GameObject gameObject)
	{
		return ActiveSelection == gameObject;
	}
}
