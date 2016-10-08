using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	string[] mapData;

	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start(){
		nodeDiameter = nodeRadius * 2;

		mapData = FindObjectOfType<LevelManager>().ReadLevelText();

		gridWorldSize.x = Mathf.RoundToInt(mapData[0].ToCharArray().Length * nodeDiameter);
		gridWorldSize.y = Mathf.RoundToInt(mapData.Length * nodeDiameter);

		gridSizeX = Mathf.RoundToInt(mapData[0].ToCharArray().Length);
		gridSizeY = Mathf.RoundToInt(mapData.Length);

		CreateGrid();
	}

	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY];

		//bottom left corner of grid
		Vector3 worldUpperRight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

//		Vector3 worldUpperRight = transform.position - Vector3.right * gridSizeX / 2 - Vector3.down * gridSizeY / 2;


		for (int x = 0; x < gridSizeX; x ++){
			for (int y = 0; y < gridSizeY; y ++){
				Vector3 worldPoint = worldUpperRight + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.down * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

				grid[x,y] = new Node(walkable, worldPoint, x, y);

			}
		}
	} 

	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1 ; x ++){
			for (int y = -1; y <= 1 ; y ++){
				if (x == 0 && y == 0){
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid[x,y];
	}

	public List<Node> path; 
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

		if (grid != null){
			foreach(Node n in grid){

				Gizmos.color = (n.walkable)?Color.white:Color.red;

				if (path != null){
					if(path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}

				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
				 
			}
		}
	}


}
