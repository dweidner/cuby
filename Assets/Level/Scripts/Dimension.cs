using UnityEngine;
using System.Collections;

public class Dimension : MonoBehaviour {


	[Header("Beleuchtung")]
	public Color ColorTop = new Color(1,1,1,1);
	public Color ColorLeft = new Color(1,1,1,1);
	public Color ColorRight = new Color(1,1,1,1);
	[Range(0, 1)]
	public float globalIllumination = .5f;

	[Header("Farben")]
	public Color backgroundColor = new Color(.5f,.5f,.5f,1);

	[Header("Farben für Gizmos")]
  	public Color GUIColor = Color.blue;

	void Start () {
		SetLightning();
	}
	
	// Update is called once per frame
	void OnDrawGizmos () {
		Debug.Log ("in DrawGizmo");
//		if (Active) {

			SetLightning();
//		}
	}

	public void SetLightning(){
		Shader.SetGlobalColor ("_ColorX", ColorRight);
		Shader.SetGlobalColor ("_ColorY", ColorTop);
		Shader.SetGlobalColor ("_ColorZ", ColorLeft);
		Shader.SetGlobalFloat ("_GlobalIllumination", globalIllumination);
		Camera.main.backgroundColor = backgroundColor;
	}
	
}
