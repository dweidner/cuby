using UnityEngine;
using System.Collections;

public class TileGizmo : MonoBehaviour {
	private Color passable = Color.green;
	private Color notPassable = Color.red;
	private Color dimension1 = Color.red;
	private Color dimension2 = Color.green;
	private Color dimension3 = Color.blue;

	  private Component tileComponent;

	  void Start(){
	    tileComponent = GetComponent<Tile>();
		if (GetComponent<Tile> ().TargetDimension != null) {
			passable = GetComponent<Tile> ().TargetDimension.GetComponent<Dimension> ().GUIColor;
		}
	  }

	

  void OnDrawGizmos() {
    switch (GetComponent<Tile>().isPassable) 
    {
      case true:
		Color dimensionColor = gameObject.GetComponentInParent<Dimension>().GUIColor;
		dimensionColor.a = .5f;
		Gizmos.color = dimensionColor;
		break;
      case false:
        Gizmos.color = notPassable;
        break;
    }
    Gizmos.DrawCube(transform.position, new Vector3(1, .05f, 1));



    GameObject targetDimension = (GameObject) GetComponent<Tile>().TargetDimension;
	
	
	//LinkObject.GetComponent<hingeJoint2D>().hingeJoint2D.connectedBody = playerA.rigidbody2D;


	if(targetDimension != null){
//      Debug.Log(GetComponent<Tile>().TargetDimension);
      //Debug.Log(targetDimension.GUIColor);
		Color dimensionColor = targetDimension.GetComponent<Dimension> ().GUIColor;
		Gizmos.color = dimensionColor;
    }else{
      Gizmos.color = Color.clear;
    }


    Gizmos.DrawCube(transform.position, new Vector3(.5f, .1f, .5f));
  }
}