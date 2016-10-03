using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//jumping
	public LayerMask groundedMask;
	public Transform groundPoint;
	public float radius;

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
