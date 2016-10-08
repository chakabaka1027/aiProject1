﻿using UnityEngine;
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

		Invoke("CreateGrid", 0.1f);
	}

	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY];

		//bottom left corner of grid
		Vector2 worldUpperRight = Camera.main.ScreenToWorldPoint(new Vector2(0 + nodeRadius, Screen.height - nodeRadius));

		for (int x = 0; x < gridSizeX; x ++){
			for (int y = 0; y < gridSizeY; y ++){
				Vector2 worldPoint = worldUpperRight + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.down * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));  

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

	public Node NodeFromWorldPoint(Vector2 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
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

				Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
				 
			}
		}
	}


}
