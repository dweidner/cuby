using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DimensionController : MonoBehaviour {

	public string tileTag = "MetaTile";
	public float tileSize = 1f;

	protected GameObject[] tiles;
	protected GameObject activeTile;

	public void OnEnable() {

		PlayerController.OnMove += HandleOnMove;
		PlayerController.OnAnimationDone += HandleOnAnimationDone;
	
	}
	
	public void OnDisable() {

		PlayerController.OnAnimationDone -= HandleOnAnimationDone;

	}

	public void Start() {

		tiles = GameObject.FindGameObjectsWithTag (tileTag);

		if (tiles != null || tiles.Length == 0) {
			Debug.LogError( "No tiles found" );
		}

	}

	public void Update() {

	}

	protected void HandleOnMove (PlayerEventArgs e) {

		if (!HeroCanMove (e.NormalizedPosition, e.Movement)) {
			e.Cancel();
			Debug.Log( "Cannot move " + e.Movement );
		}

	}
	
	protected void HandleOnAnimationDone (PlayerEventArgs e) {

		GameObject hero = e.GameObject;
		GameObject tile = GetTileByPosition(e.NormalizedPosition);

		if(tile != null){
			SetActiveTile(tile);
		} else {
			activeTile = null;
		}

	}

	protected bool HeroCanMove(Vector3 position, string direction) {

		GameObject[] neighbours = GetNeighbourTiles (position);

		if (direction == "up" && neighbours [0] != null) {
			return true;
		} else if (direction == "right" && neighbours [1] != null) {
			return true;
		} else if (direction == "down" && neighbours [2] != null) {
			return true;
		} else if (direction == "left" && neighbours[3] != null) {
			return true;
		}

		return false;

	}

	protected GameObject GetTileByPosition(Vector3 position) {

		foreach (var tile in tiles) {
			Vector3 tilePos = tile.transform.position;
			if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)){
				return tile;
			}
		}

		return null;

	}

	protected GameObject[] GetNeighbourTiles(Vector3 position) {

		GameObject[] neighbours = new GameObject [4];
		GameObject current = GetTileByPosition (position);

		if (current != null) {
	
			// Above
			neighbours[0] = GetTileByPosition(position + tileSize * Vector3.forward);

			// Right
			neighbours[1] = GetTileByPosition(position + tileSize * Vector3.right);

			// Below
			neighbours[2] = GetTileByPosition(position + tileSize * Vector3.back);

			// Left
			neighbours[3] = GetTileByPosition(position + tileSize * Vector3.left);

		}


		return neighbours;

	}

	protected void SetActiveTile(GameObject t) {

		activeTile = t;

		string dimension = t.GetComponent<MetaTile> ().dimension.ToString ();
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
