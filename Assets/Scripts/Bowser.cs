using UnityEngine;
using System.Collections;

public class Bowser : MonoBehaviour {

	public Transform target;
	public float speed = 3;

	[Header("Audio")]
	public GameObject audioManager;
	public AudioClip bowserAngry;
	AudioSource backgroundAudio;	
	AudioSource sfxAudio;



	Vector2[] path;
	int targetIndex;

	void Start(){
		backgroundAudio = audioManager.GetComponent<AudioSource>();
		sfxAudio = gameObject.GetComponent<AudioSource>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space) && target != null && target.GetComponent<Peach>().isReset == true){
			PathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
			backgroundAudio.pitch = Time.timeScale = 1.5f;
			sfxAudio.PlayOneShot(bowserAngry, 0.5f);
		}

		if (Input.GetMouseButtonDown(0)){
			StopCoroutine("FollowPath");
		}

		//face peach
		if (target.position.x > gameObject.transform.position.x){
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			gameObject.GetComponent<SpriteRenderer>().flipX = true;

		}
	}

	public void OnPathFound(Vector2[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], new Vector2(.1f, .1f));

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}