using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Tile : MonoBehaviour {
	public bool isPassable = true;
	public bool isDeadly = false;

	// public GlobalVars.DimensionTypes TargetDimension = GlobalVars.DimensionTypes.Dimension1;
	[SerializeField]
	public GameObject TargetDimension;

	public GameObject Dimension(){
		return transform.parent.parent.gameObject;
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
