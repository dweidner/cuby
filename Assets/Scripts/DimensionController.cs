using UnityEngine;
using System.Collections;

public class DimensionController : MonoBehaviour {
	public GameObject cube;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateDimension(){
		GameObject metaTile = getTileByPosition(cube.transform.position, "MetaTile");
		Debug.Log (metaTile);
		if(metaTile != null && metaTile.tag == "MetaTile"){
			toggleDimension(metaTile.GetComponent<MetaTile>().dimension.ToString());
		}
	}

	private GameObject getTileByPosition(Vector3 position, string tag){
		GameObject[] tiles = GameObject.FindGameObjectsWithTag(tag);
		Debug.Log (tiles.Length);
		foreach (var tile in tiles) {
			Vector3 tilePos = tile.transform.position;
			if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)){
				return tile;
			}
		}
		return null;
	}

	public void toggleDimension(string dimensionTitle){
		Camera.main.cullingMask = 0;
		Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(dimensionTitle + " visual"));

		Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
		foreach(Light light in lights)
		{
			light.intensity = 0;
			if(light.tag == dimensionTitle + "Light"){
				light.intensity = 1;
			}
		}
	}
}
