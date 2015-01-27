using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DimensionController : MonoBehaviour {
	
	public string dimensionTag = "Dimension";
	public string metaTileTag = "MetaTile";
	public float tileSize = 1f;
	
	protected Dictionary<string, Transform[]> tiles;
	protected GameObject[] metaTiles;
	protected GameObject currentMetaTile;
	
	public void OnEnable() {
		
		PlayerController.OnMove += HandleOnMove;
		PlayerController.OnAnimationDone += HandleOnAnimationDone;
		
	}
	
	public void OnDisable() {
		
		PlayerController.OnAnimationDone -= HandleOnAnimationDone;
		
	}
	
	public void Start() {
		
		tiles = new Dictionary<string, Transform[]> ();
		
		GameObject[] dimensions = GameObject.FindGameObjectsWithTag (dimensionTag);
		foreach (GameObject dimension in dimensions) {
			
			Transform[] children = dimension.GetComponentsInChildren<Transform>();
			tiles.Add(dimension.name, children);
			
			if (children.Length == 0) {
				Debug.LogError( "Dimension " + name + " is empty" );
			}
			
		}
		
		metaTiles = GameObject.FindGameObjectsWithTag (metaTileTag);
		
		if (metaTiles == null || metaTiles.Length == 0) {
			Debug.LogError( "No meta tiles found" );
		}
		
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		GameObject tile = GetMetaTileByPosition (player.transform.position);
		SetActiveTile (tile);


		if (tile == null) {
			Debug.LogError( "Starting tile not found");
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
		
		GameObject tile = GetMetaTileByPosition(e.NormalizedPosition);
		SetActiveTile(tile);
		
	}
	
	protected bool HeroCanMove(Vector3 position, string direction) {
		
		GameObject[] neighbours = GetNeighbourMetaTiles (position);
		GameObject targetMetaTile = null;
		
		if (direction == "up" && neighbours [0] != null) {
			targetMetaTile = neighbours[0];
		} else if (direction == "right" && neighbours [1] != null) {
			targetMetaTile = neighbours[1];
		} else if (direction == "down" && neighbours [2] != null) {
			targetMetaTile = neighbours[2];
		} else if (direction == "left" && neighbours[3] != null) {
			targetMetaTile = neighbours[3];
		}
		
		
		if (currentMetaTile != null && targetMetaTile != null) {
			
			string currentDimension = GetDimension(currentMetaTile);
			string targetDimension = GetDimension(targetMetaTile);
			
			GameObject targetTileCurrentDimension = GetTileInDimension(currentDimension, targetMetaTile.transform.position);
			GameObject targetTileNextDimension = GetTileInDimension(targetDimension, targetMetaTile.transform.position);
			
			if ( targetTileCurrentDimension != null) {
				return targetTileCurrentDimension.GetComponent<Tile>().isPassable;
			}
			
		}
		
		return false;
		
	}
	
	protected GameObject GetTileInDimension(string dimension, Vector3 position) {
		
		if (tiles.ContainsKey(dimension)) {
			
			Transform[] children = tiles[dimension];
			
			foreach (var transform in children) {
				
				Vector3 tilePos = transform.position;
				if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)) {
					return transform.gameObject;
				}
				
			}
			
		}
		
		return null;
		
	}
	
	protected GameObject GetMetaTileByPosition(Vector3 position) {
		
		foreach (var tile in metaTiles) {
			Vector3 tilePos = tile.transform.position;
			if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)) {
				return tile;
			}
		}
		
		return null;
		
	}
	
	protected GameObject[] GetNeighbourMetaTiles(Vector3 position) {
		
		GameObject[] neighbours = new GameObject [4];
		GameObject current = GetMetaTileByPosition (position);
		
		if (current != null) {
			
			// Above
			neighbours[0] = GetMetaTileByPosition(position + tileSize * Vector3.forward);
			
			// Right
			neighbours[1] = GetMetaTileByPosition(position + tileSize * Vector3.right);
			
			// Below
			neighbours[2] = GetMetaTileByPosition(position + tileSize * Vector3.back);
			
			// Left
			neighbours[3] = GetMetaTileByPosition(position + tileSize * Vector3.left);
			
		}
		
		
		return neighbours;
		
	}
	
	protected void SetActiveTile(GameObject tile) {
		
		currentMetaTile = tile;
		
		if (tile != null) {
			ShowDimension (GetDimension (tile));
		}
		
	}
	
	protected string GetDimension(GameObject tile) {
		return tile.GetComponent<MetaTile> ().dimension.ToString ();
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