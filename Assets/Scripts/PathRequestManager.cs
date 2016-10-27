using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

//creates a queue if multiple astar agents are employed in order to efficiently manage simultaneous astar calculations

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	AStar astar;

	bool isProcessingPath;

	void Awake() {
		instance = this;
		astar = GetComponent<AStar>();
	}

	//place requests into queue
	public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	//pull element from queue and begin pathfinding
	void TryProcessNext() {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			astar.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	//grab the next element in the queue and run astar again
	public void FinishedProcessingPath(Vector2[] path, bool success) {
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}

	//container for necessary elements in queue
	struct PathRequest {
		public Vector2 pathStart;
		public Vector2 pathEnd;
		public Action<Vector2[], bool> callback;

		public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}