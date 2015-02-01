using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DimensionController : MonoBehaviour {
	
	public string dimensionTag = "Dimension";
	public string metaTileTag = "MetaTile";
	public float tileSize = 1f;
	public GameObject startDimension;


	protected Dictionary<string, Transform[]> tiles;
	protected GameObject currentTile;
	protected GameObject currentDimension;
	protected GameObject[] dimensions;
	
	public void OnEnable() {
		
		PlayerController.OnMove += HandleOnMove;
		PlayerController.OnAnimationDone += HandleOnAnimationDone;
		
	}
	
	public void OnDisable() {
		
		PlayerController.OnAnimationDone -= HandleOnAnimationDone;
		
	}
	
	public void Start() {
		
		tiles = new Dictionary<string, Transform[]> ();
		
		dimensions = GameObject.FindGameObjectsWithTag (dimensionTag);

		foreach (GameObject dimension in dimensions) {

			Transform[] logicTiles;
		
			foreach (Transform child in dimension.transform)
			{
				if(child.name == "Logic"){
					foreach (Transform logitile in child) {
						child.position += Vector3.up * 10.0F;
					}
					logicTiles = child.GetComponentsInChildren<Transform>();
					tiles.Add (dimension.name, logicTiles);
					if (logicTiles.Length == 0) {
						Debug.LogError( "Dimension " + dimension.name + " is empty" );
					}
				}
			}
			
		}

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		currentTile = GetTileInDimension (startDimension, player.transform.position);

		currentDimension = startDimension;
		SetActiveTile (currentTile);
	}
	
	public void Update() {
		
	}

	void OnDrawGizmos () {

	}
	
	protected void HandleOnMove (PlayerEventArgs e) {
		
		if (!HeroCanMove (e.NormalizedPosition, e.Movement)) {
			e.Cancel();
			Debug.Log( "Cannot move " + e.Movement );
		}
		
	}
	
	protected void HandleOnAnimationDone (PlayerEventArgs e) {
		
		GameObject tile = GetTileInDimension(currentDimension, e.NormalizedPosition);
		SetActiveTile(tile);
		
	}
	
	protected bool HeroCanMove(Vector3 position, string direction) {
		
		GameObject[] neighbours = GetNeighbourTiles (currentDimension, position);
		GameObject targetTile = null;
		
		if (direction == "up" && neighbours [0] != null) {
			targetTile = neighbours[0];
		} else if (direction == "right" && neighbours [1] != null) {
			targetTile = neighbours[1];
		} else if (direction == "down" && neighbours [2] != null) {
			targetTile = neighbours[2];
		} else if (direction == "left" && neighbours[3] != null) {
			targetTile = neighbours[3];
		}
		
		
		if (currentTile != null && targetTile != null) {

			return targetTile.GetComponent<Tile>().isPassable;
			
		}
		
		return false;
		
	}
	
	protected GameObject GetTileInDimension(GameObject dimension, Vector3 position) {
		
		if (tiles.ContainsKey(dimension.name)) {
			
			Transform[] children = tiles[dimension.name];
			
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

		GameObject[] metaTiles = GameObject.FindGameObjectsWithTag (metaTileTag);
		
		foreach (var tile in metaTiles) {
			Vector3 tilePos = tile.transform.position;
			if(Mathf.Approximately(position.x, tilePos.x) && Mathf.Approximately(position.z, tilePos.z)) {
				return tile;
			}
		}
		
		return null;
		
	}

	protected GameObject[] GetNeighbourTiles(GameObject dimension, Vector3 position) {
		
		GameObject[] neighbours = new GameObject [4];
		GameObject current = GetTileInDimension (dimension, position);
		
		if (current != null) {
			
			// Above
			neighbours[0] = GetTileInDimension(dimension, position + tileSize * Vector3.forward);
			
			// Right
			neighbours[1] = GetTileInDimension(dimension, position + tileSize * Vector3.right);
			
			// Below
			neighbours[2] = GetTileInDimension(dimension, position + tileSize * Vector3.back);
			
			// Left
			neighbours[3] = GetTileInDimension(dimension, position + tileSize * Vector3.left);
			
		}
		
		
		return neighbours;
		
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
		currentTile = tile;

		if (tile != null) {
			switchDimension (GetDimension (tile));
		}
		
	}
	
	protected GameObject GetDimension(GameObject tile) {
		if (tile.GetComponent<Tile> ().TargetDimension != null)
			return tile.GetComponent<Tile> ().TargetDimension;
		else
			return tile.GetComponent<Tile> ().Dimension();
	}
	
	protected void switchDimension(GameObject dimension) {

		foreach (var dim in dimensions) {
			dim.SetActive(false);
		}
		dimension.SetActive (true);
		dimension.GetComponent<Dimension> ().SetLightning();
		currentDimension = dimension;
		
	}
	
}