using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//jumping
	public LayerMask groundedMask;
	public Transform groundPoint;
	public float radius;

	//waypoint
	public LayerMask clickPoint;

	Rigidbody2D rb;
	[SerializeField]
	float speed = 3;
	Animator animator;
	bool facingRight;
	bool isGrounded;
	bool isJumping;

	public bool isPressingLeft;
	public bool isPressingRight;
	public bool isPressingJump;


	// Use this for initialization
	void Start () {
		facingRight = true;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

	}

	void Update(){
	//Making a Waypoint
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
			if (hit){
				hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
//				FindObjectOfType<Grid>().FindGroundNode(Input.mousePosition);
				FindObjectOfType<Grid>().NodeFromWorldPoint(hit.point);

				FindObjectOfType<AStar>().FindPath(gameObject.transform.position, hit.collider.gameObject.transform.position);

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
