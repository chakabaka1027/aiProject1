using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	public Transform seeker, target;

	Grid grid;

	void Awake(){
		grid = GetComponent<Grid>();
	}

	void Update(){
		FindPath(seeker.position, target.position);
	}
		
	public void FindPath(Vector2 startPosition, Vector2 targetPosition){
		Node startNode = grid.NodeFromWorldPoint(startPosition);
		Node targetNode = grid.NodeFromWorldPoint(targetPosition);


		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		openSet.Add(startNode);

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
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)){
				if(!neighbour.walkable || closedSet.Contains(neighbour)){
					continue;
				}

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

	void RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>(); 
		Node currentNode = endNode;

		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse();

		grid.path = path;
	}

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