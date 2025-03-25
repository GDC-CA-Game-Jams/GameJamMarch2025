using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2f;
	Vector3 moveLeft = new Vector3(-1,0,0);
	Vector3 moveRight = new Vector3(1,0,0);
	Vector3 moveUp = new Vector3(0,1,0);
	Vector3 moveDown = new Vector3(0,-1,0);
    Vector3 moveIdle = new Vector3(0, 0, 0);
	Vector3 fullScreenTopAndLeftEdges;
    Vector3 fullScreenBottomAndRightEdges;
	Vector3 currentMovement;
	string currentAnimation;
	string newAnimation;
	string animUp = "WalkUp";
	string animDown = "WalkDown";
	string animSide = "WalkLeftRight";
    string animIdle = "Idle";
	SpriteRenderer sr;

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
		sr = gameObject.GetComponent<SpriteRenderer>();
        //animator = gameObject.GetComponent<Animator>();
		currentAnimation = animIdle;
        //animator.Play(currentAnimation);
    }

    // Update is called once per frame
    void Update()
    {
		PlayerMovement();
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

    public void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = false;
            SetAnim(animSide, moveLeft);   
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            SetAnim(animUp, moveUp);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            SetAnim(animDown, moveDown);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = true;
            SetAnim(animSide, moveRight);   
        }
        else
        {
            SetAnim(animIdle, moveIdle);
        }

        Move();
    }

	public void Move()
	{
        transform.position += currentMovement * Time.deltaTime * speed; //x,y,z
    }

    public void SetAnim(string anim, Vector3 move)
    {
        newAnimation = anim;
        currentMovement = move;

        if (currentAnimation != newAnimation)
        {
            //animator.Play(newAnimation);
            animator.SetBool(newAnimation, true);
            animator.SetBool(currentAnimation, false);
            currentAnimation = newAnimation;
        }
    }
}
