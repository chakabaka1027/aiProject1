using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AStar : MonoBehaviour {

	PathRequestManager requestManager;

	Grid grid;

	void Awake(){
		grid = GetComponent<Grid>();
		requestManager = GetComponent<PathRequestManager>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos,targetPos));
	}

	//AStar Algorithm

	public IEnumerator FindPath(Vector2 startPosition, Vector2 targetPosition){

		Vector2[] waypoints = new Vector2[0];
		bool pathSuccess = false;

		//Establishing start and target nodes from grid
		Node startNode = grid.NodeFromWorldPoint(startPosition);
		Node targetNode = grid.NodeFromWorldPoint(targetPosition);

		if(startNode.walkable && targetNode.walkable){

		//open and closed list and hashset
			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();

		//add start node to open list
			openSet.Add(startNode);

		//loop
			while (openSet.Count > 0){
				Node currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i ++){
					if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
						currentNode = openSet[i];
					}
				}

				openSet.Remove(currentNode);
				closedSet.Add(currentNode);

				//path has been found
				if(currentNode == targetNode){
					pathSuccess = true;
					break;
				}

				//check if neighbor is walkable, if not, skip
				foreach (Node neighbour in grid.GetNeighbours(currentNode)){
					if(!neighbour.walkable || closedSet.Contains(neighbour)){
						continue;
					}

				//if traversable, set f cost, parent, and add to open list if not in open list
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if(!openSet.Contains(neighbour)){
							openSet.Add(neighbour);
						}
					} 
				}
			}
		}
		yield return null;
		if (pathSuccess == true){
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
	}

	//retrace path from children and parents
	Vector2[] RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>(); 
		Node currentNode = endNode;

		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

//	Uncomment the code below for Simplified Path
//		Vector2[] waypoints = SimplifyPath(path);
//		Array.Reverse(waypoints);
//		return waypoints;

		Vector2[] waypoints = NormalPath(path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	Vector2[] NormalPath(List<Node> path){

		List<Vector2> waypoints = new List<Vector2>();
		for (int i = 1; i < path.Count; i ++) {
				
			waypoints.Add(path[i].worldPosition);
		}
		return waypoints.ToArray();

	}

	//currently unused because simplifying the path made bowser more obviously clip through obstacles when pathfinding around them
	Vector2[] SimplifyPath(List<Node> path){
		List<Vector2> waypoints = new List<Vector2>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	//checking distances, diagonals most costly
	int GetDistance(Node nodeA, Node nodeB){
		int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(distanceX > distanceY){
			return 14 * distanceY + 10 * (distanceX - distanceY);
		} else {
			return 14 * distanceX + 10 * (distanceY - distanceX);

		}
	}
}

//PseudoCode
//OPEN //the set of nodes to be evaluated
//CLOSED //the set of nodes already evaluated

//add the start node to OPEN
// 
//loop
//        current = node in OPEN with the lowest f_cost
//        remove current from OPEN
//        add current to CLOSED
// 
//        if current is the target node //path has been found
//                return
// 
//        foreach neighbour of the current node
//                if neighbour is not traversable or neighbour is in CLOSED
//                        skip to the next neighbour
// 
//                if new path to neighbour is shorter OR neighbour is not in OPEN
//                        set f_cost of neighbour
//                        set parent of neighbour to current
//                        if neighbour is not in OPEN
//                                add neighbour to OPEN