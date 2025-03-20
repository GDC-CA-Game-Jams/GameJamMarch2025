using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 2f;
	Vector3 moveLeft = new Vector3(-1,0,0);
	Vector3 moveRight = new Vector3(1,0,0);
	Vector3 moveUp = new Vector3(0,1,0);
	Vector3 moveDown = new Vector3(0,-1,0);
	Vector3 fullScreenTopAndLeftEdges;
    Vector3 fullScreenBottomAndRightEdges;

	Animator animator;
	public void Start()
	{
		animator = GetComponent<Animator>();
		fullScreenTopAndLeftEdges = Camera.main.ScreenToWorldPoint(new Vector3(10, Screen.height-100, 10));//x,y,z
        fullScreenBottomAndRightEdges = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width+100, 10, 10));//x,y,z
                                                                                                          //50 and -50 so it is near, but not at, the screen boundaries
                                                                                                          //Screen.height is a thing too
                                                                                                          //but i want the upper left corner which is x coord 0
                                                                                                          //bottom left corner is (0,0) (x,y)
                                                                                                          //top right is pixelWidth, pixelHeight (x,y)
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
		StayInBoundaries();
			
    }

	public void StayInBoundaries()
    {
        //Keep object within screen boundaries
        Vector3 temp = transform.position;
        if (temp.y > fullScreenTopAndLeftEdges.y) { temp.y = fullScreenTopAndLeftEdges.y; }
        if (temp.x < fullScreenTopAndLeftEdges.x) { temp.x = fullScreenTopAndLeftEdges.x; }
        if (temp.y < fullScreenBottomAndRightEdges.y) { temp.y = fullScreenBottomAndRightEdges.y; }
        if (temp.x > fullScreenBottomAndRightEdges.x) { temp.x = fullScreenBottomAndRightEdges.x; }
        transform.position = temp;
    }
}
