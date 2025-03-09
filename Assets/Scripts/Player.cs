using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 2f;
		Vector3 moveLeft = new Vector3(-1,0,0);
		Vector3 moveRight = new Vector3(1,0,0);
		Vector3 moveUp = new Vector3(0,1,0);
		Vector3 moveDown = new Vector3(0,-1,0);

	Animator animator;
	public void Start()
	{
		animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += moveLeft * Time.deltaTime*speed; //x,y,z
			sr.flipX = false;
			animator.SetBool ("WalkLeftRight", true);
			animator.SetBool ("WalkUp", false);
			animator.SetBool ("WalkDown", false);
		}

		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			transform.position += moveUp * Time.deltaTime*speed; //x,y,z
			animator.SetBool ("WalkUp", true);
			animator.SetBool ("WalkLeftRight", false);
			animator.SetBool ("WalkDown", false);
		}

		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			transform.position += moveDown * Time.deltaTime*speed; //x,y,z
			animator.SetBool ("WalkDown", true);
			animator.SetBool ("WalkLeftRight", false);
			animator.SetBool ("WalkUp", false);
		}

		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			transform.position += moveRight * Time.deltaTime*speed; //x,y,z
			sr.flipX = true;
			animator.SetBool ("WalkLeftRight", true);
			animator.SetBool ("WalkUp", false);
			animator.SetBool ("WalkDown", false);
		}
		//if the player is not moving
		if(!(Input.GetKey(KeyCode.W))&&!(Input.GetKey(KeyCode.A))&&!(Input.GetKey(KeyCode.S))&&!(Input.GetKey(KeyCode.D)))
		{
			if(!(Input.GetKey(KeyCode.UpArrow))&&!(Input.GetKey(KeyCode.LeftArrow))&&!(Input.GetKey(KeyCode.DownArrow))&&!(Input.GetKey(KeyCode.RightArrow))){
			animator.SetBool ("WalkLeftRight", false);
			animator.SetBool ("WalkUp", false);
			animator.SetBool ("WalkDown", false);
			}
		}
			
    }
}
