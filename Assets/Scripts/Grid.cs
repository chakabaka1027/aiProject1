using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

	public bool gizmosActive = true;

	public Transform seeker;
	public Transform target;

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public GameObject cube;

	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start(){
		nodeDiameter = nodeRadius * 2;

		gridWorldSize.x = 37 * nodeDiameter;
		gridWorldSize.y = 21 * nodeDiameter;

		gridSizeX = 37;
		gridSizeY = 21;

		Invoke("CreateGrid", 0.2f);
	}

	//create a grid of nodes by looping through x and y
	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY];

		//upper left corner of grid
		Vector2 worldUpperLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));

		for (int x = 0; x < gridSizeX; x ++){
			for (int y = 0; y < gridSizeY; y ++){
				Vector2 worldPoint = worldUpperLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.down * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapPoint(worldPoint, unwalkableMask));  
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	} 

	//functionality to check neighbors in Astar script by starting at the upper left node and moving left to right through all neighbors
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

	//functionality to take a vector3 in the world and find its position on the grid
	public Node NodeFromWorldPoint(Vector2 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (-worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid[x,y];
	}

	public List<Node> path;

	//debug tools to allow visualization of grid
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

		if (grid != null && gizmosActive == true){

			Node playerNode = NodeFromWorldPoint(seeker.position);
			Node targetNode = NodeFromWorldPoint(target.position);


			foreach(Node n in grid){

				Gizmos.color = (n.walkable)?Color.white:Color.red;

				if (path != null){
					if(path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}
				//test
				if(playerNode == n){
					Gizmos.color = Color.cyan;
				}

				if(targetNode == n){
					Gizmos.color = Color.cyan;
				}

				Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
				 
			}
		}
	}


}
