using UnityEngine;

public class Patrol : MonoBehaviour
{
    public GameObject[] patrolPoints;
    public float speed = 2f;
    public float patrolDistance = 0.5f;
    public float movementRange = 0.105f;
    public float eatingTime = 2f;

    //Enemy Movement
    Vector3 moveLeft = new Vector3(-1, 0, 0);
    Vector3 moveRight = new Vector3(1, 0, 0);
    Vector3 moveUp = new Vector3(0, 1, 0);
    Vector3 moveDown = new Vector3(0, -1, 0);
    //Vector3 desiredMovement;

    //Enemy Animation
    string animUp = "Up";
    string animDown = "Down";
    string animSide = "LeftRight";
    string currentAnimation;
    string newAnimation;

    private GameObject currentPatrol;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool eating = false;
    private float eatTimer = 0f;
    private float savedSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        savedSpeed = speed;

        GetPatrolPoint();

        anim.SetBool("IsRunning", true);
    }

    void Update()
    {
        if (eating == true)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatingTime)
            {
                speed = savedSpeed;
                eatTimer = 0;
                eating = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (eating != true)
        {
            EnemyMovement();
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
        
    }

    void EnemyMovement()
    {
        CheckPatrol();

        Vector3 desiredMovement = Vector3.zero;
        string newanim = "Idle";

        if (currentPatrol.transform.position.y > transform.position.y)
        {
            if ((currentPatrol.transform.position.y - transform.position.y) > movementRange)
            {
                //SetAnim(animUp);
                newanim = animUp;
                desiredMovement += moveUp;
            }
        }
        if (currentPatrol.transform.position.y < transform.position.y)
        {
            if ((transform.position.y - currentPatrol.transform.position.y) > movementRange)
            {
                //SetAnim(animDown);
                newanim = animDown;
                desiredMovement += moveDown;
            }
        }
        if (currentPatrol.transform.position.x > transform.position.x)
        {
            if ((currentPatrol.transform.position.x - transform.position.x) > movementRange)
            {
                sr.flipX = true;
                //SetAnim(animSide);
                newanim = animSide;
                desiredMovement += moveRight;
            }
        }
        if (currentPatrol.transform.position.x < transform.position.x)
        {
            if ((transform.position.x - currentPatrol.transform.position.x) > movementRange)
            {
                sr.flipX = false;
                //SetAnim(animSide);
                newanim = animSide;
                desiredMovement += moveLeft;
            }

        }

        SetAnim(newanim);

        desiredMovement = (Vector3.Normalize(desiredMovement)) * speed;

        Move(desiredMovement);
    }

    public void Move(Vector3 move)
    {
        rb.MovePosition(new Vector2((transform.position.x + move.x * Time.deltaTime * speed), (transform.position.y + move.y * Time.deltaTime * speed)));
    }

    void GetPatrolPoint()
    {
        var index = Random.Range(0, patrolPoints.Length);
        if(currentPatrol == patrolPoints[index])
        {
            GetPatrolPoint();
        }
        else
        {
            currentPatrol = patrolPoints[index];
        }
            
    }

    void CheckPatrol()
    {
        if(Vector2.Distance(transform.position, currentPatrol.transform.position) < patrolDistance)
        {
            GetPatrolPoint();
        }
    }

    public void SetAnim(string animator)
    {
        newAnimation = animator;

        if (currentAnimation != newAnimation)
        {
            //animator.Play(newAnimation);
            anim.SetBool(newAnimation, true);
            anim.SetBool(currentAnimation, false);
            currentAnimation = newAnimation;
        }
    }

    public void Eating()
    {
        speed = 0;
        eating = true;

        //SetAnim("SlimeEats");
    }
}
