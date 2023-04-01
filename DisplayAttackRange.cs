using UnityEngine;
using System.Collections;

public class DisplayAttackRange : MonoBehaviour {


	public float tileSize = 1.0f;
	public int standardAttackRange;
	public int powerAttackRange;
	public int lungeAttackRange;
	public GameObject displayAttackRange;
	//public static int a =0;
	//public static int b = 0;
	
	// Use this for initialization

	
	// Update is called once per frame

	
	void BuildTexture(int attackRange){
		Color c = new Color (0, 0, 0, 0);
		Texture2D texture = new Texture2D(attackRange+1, attackRange+1);
		for (int y = 0; y < attackRange/2; y++) {
			for (int x = 0; x + y < attackRange/2; x++){

				texture.SetPixel (x, y, c);
			}
		}
		
		for (int y = attackRange/2 + 1; y < attackRange; y++) {
			for (int x = 0; y - x > attackRange/2; x++){

				texture.SetPixel (x, y, c);
			}
		}
		
		for (int x = attackRange/2 + 1; x < attackRange; x++) {
			for (int y = 0; x - y > attackRange/2; y++) {
			
				texture.SetPixel (x, y, c);
			}
		}
		
		for (int y = attackRange/2 + 1; y < attackRange; y++) {
			for (int x = attackRange/2 + 1; x < attackRange; x++) {

				texture.SetPixel (x, y, c);
			}
		}
		
		for (int y = attackRange/2 + 1; y < attackRange; y++) {
			for (int x = attackRange/2 + 1; x + y < attackRange*2 - (attackRange/2+1); x++) {
				Color d = new Color (63, 63, 63, 63);
				texture.SetPixel (x, y, d);
			}
		}
		
		texture.filterMode = FilterMode.Point;
		texture.Apply ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;
	}
	
	public void BuildMesh(int attackRange) {
		int size_x = attackRange;
		int size_z = attackRange;
		
		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];
		
		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / vsize_x, (float)z / vsize_z );
			}
		}

		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 		   1;
			}
		}
		
	
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		BuildTexture (attackRange);
	}
	
	void PositionMesh(int attackRange){
		transform.Translate (attackRange / -2, 0, attackRange / -2);
		
	}

	//void unPositionMesh(int attackRange){
	//	transform.Translate (attackRange / 2, 0, attackRange / 2);
	//}

	void BuildTextureStandard(int attackRange){
		Texture2D texture = new Texture2D(attackRange+1, attackRange+1);
		texture.filterMode = FilterMode.Point;
		texture.Apply ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;
	}

	void BuildMeshStandard(int attackRange){
		int size_x = attackRange;
		int size_z = attackRange;
		
		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];
		
		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / vsize_x, (float)z / vsize_z );
			}
		}
		
		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 		   1;
			}
		}

		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		BuildTextureStandard (attackRange);
	}
	
	public void StandardAttack(){
		if (Input.GetKey (KeyCode.Space)) {
			return;
		} else {
			BuildMesh (standardAttackRange);
			PositionMesh (standardAttackRange);
			displayAttackRange.SetActive (true);
			BattleStateMachine.ButtonSource.Play ();
		}
	}

	public void PowerAttack(){
		if (Input.GetKey (KeyCode.Space)) {
			return;
		} else {
			BuildMesh (powerAttackRange);
			PositionMesh (powerAttackRange);
			displayAttackRange.SetActive (true);
			BattleStateMachine.ButtonSource.Play ();
		}
	}

	public void LungeAttack(){
		if (Input.GetKey (KeyCode.Space)) {
			return;
		} else {
			BuildMeshStandard (lungeAttackRange);
			PositionMesh (lungeAttackRange);
			displayAttackRange.SetActive (true);
			BattleStateMachine.ButtonSource.Play ();
		}
	}

}

