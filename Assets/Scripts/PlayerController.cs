using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//jumping
	[Header("Jumping")]
	public LayerMask groundedMask;
	public Transform groundPoint;
	public float radius;

	//waypoint
	[Header("Waypoint")]
	public LayerMask clickPoint;
	public GameObject peach;

	[Header("Audio")]
	public GameObject audioManager;
	public AudioClip marioJump;

	AudioSource backgroundAudio;
	AudioSource sfxAudio;

	Rigidbody2D rb;
	[SerializeField]
	float speed = 3;
	Animator animator;
	bool facingRight;
	bool isGrounded;
	bool isJumping;


	// Use this for initialization
	void Start () {
		facingRight = true;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		backgroundAudio = audioManager.GetComponent<AudioSource>();
		sfxAudio = gameObject.GetComponent<AudioSource>();


	}

	void Update(){
	//Making a Waypoint
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
			if (hit){
				peach.transform.position = hit.point;
				backgroundAudio.pitch = Time.timeScale = 1f;

				FindObjectOfType<Peach>().MakeGoodSound();

			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float horizontal = Input.GetAxis("Horizontal");
		HandleMovement(horizontal);
		Flip(horizontal);

		//jumping

		isGrounded = Physics2D.OverlapCircle(groundPoint.position, radius, groundedMask);


		if(Input.GetKeyDown(KeyCode.W) && isGrounded){
			rb.AddForce(new Vector2(0, 275));
			sfxAudio.PlayOneShot(marioJump, 0.1f);
		}

		if(!isGrounded){
			animator.SetBool("isJumping", true);
		} 

		if(isGrounded){
			animator.SetBool("isJumping", false);
		}

	}

	void HandleMovement(float horizontal){
		rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
		animator.SetFloat("speed", Mathf.Abs(horizontal));
	}

	void Flip(float horizontal){
		if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight){
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		} 
	}


}
