using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    private Animator animator;
    private string currentState;
    private string previousState;
    private bool isAttackPressed;
    private bool isAttacking;
    [SerializeField]
    private float attackDelay = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttackPressed = true;
        }

    }

    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (isAttackPressed)
        {
            isAttackPressed = false;
            if (!isAttacking)
            {
                isAttacking = true;
            }
        }
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (movement.x != 0)
        {
            Debug.Log("Player_Walk_SideFace");
            ChangeAnimationState("Player_Walk_SideFace");
        }
        else if (movement.y != 0)
        {
            if (movement.y > 0)
            {
                Debug.
                ChangeAnimationState("Player_Walk_Back");
            }
            else if (movement.y < 0)
            {
                ChangeAnimationState("Player_Walk_Front");
            }
        }
        else if (movement.x == 0 && movement.y == 0)
        {
            if (previousState == "Player_Walk_SideFace")
            {
                ChangeAnimationState("Player_Idle_SideFace");
            }
            else if (previousState == "Player_Walk_Back")
            {
                ChangeAnimationState("Player_Idle_Back");
            }
            else if (previousState == "Player_Walk_Front")
            {
                ChangeAnimationState("Player_Idle_Front");
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        previousState = currentState;
        animator.Play(newState);
        currentState = newState;
    }
}