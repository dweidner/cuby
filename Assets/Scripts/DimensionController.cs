using UnityEngine;
using System.Collections;

public class DimensionController : MonoBehaviour {

	public GameObject tile;

	public void OnEnable() {

		PlayerController.OnMove += HandleOnMove;
		PlayerController.OnAnimationDone += HandleOnAnimationDone;
	
	}
	
	public void OnDisable() {

		PlayerController.OnAnimationDone -= HandleOnAnimationDone;

	}

	public void Update() {

	}

	protected void HandleOnMove (PlayerEventArgs e) {

	}
	
	protected void HandleOnAnimationDone (PlayerEventArgs e) {

		GameObject hero = e.GameObject;
		GameObject tile = GetTileByPosition(e.Position, "MetaTile");

		if(tile != null){
			SetActiveTile(tile);
		}

	}

	protected GameObject GetTileByPosition(Vector3 position, string tag) {

		GameObject[] tiles = GameObject.FindGameObjectsWithTag(tag);

		foreach (var tile in tiles) {
			Vector3 tilePos = tile.transform.position;
			if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)){
				return tile;
			}
		}

		return null;

	}

	protected void SetActiveTile(GameObject t) {

		tile = t;

		string dimension = tile.GetComponent<MetaTile> ().dimension.ToString ();
		ShowDimension (dimension);

	}

	protected void ShowDimension(string name) {

		Camera.main.cullingMask = 0;
		Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Default"));
		Camera.main.cullingMask |= (1 << LayerMask.NameToLayer(name + " visual"));

		Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
		foreach(Light light in lights) {
			light.intensity = 0;
			if(light.tag == name + "Light") {
				light.intensity = 1;
			}
		}

	}
}
