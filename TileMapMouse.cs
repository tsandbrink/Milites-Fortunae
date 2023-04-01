using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {

	TileMap _tileMap;
	
	public Vector3 currentTileCoord;
	
	public Transform selectionCube;
	public int isOnMap;
	void Start() {
		_tileMap = GetComponent<TileMap>();
	}
	
	// Update is called once per frame
	void Update () {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
		
			if (GetComponent<Collider> ().Raycast (ray, out hitInfo, Mathf.Infinity)) {
				int x = Mathf.FloorToInt (hitInfo.point.x / _tileMap.tileSize);
				int z = Mathf.FloorToInt (hitInfo.point.z / _tileMap.tileSize);
				//Debug.Log ("Tile: " + x + ", " + z);
			
				currentTileCoord.x = x + .5f;
				currentTileCoord.z = z + .5f;
				currentTileCoord.y = -.4f;
				isOnMap = 0;
				selectionCube.transform.position = currentTileCoord;
			} else {
				isOnMap = 1;
			}

	}
}