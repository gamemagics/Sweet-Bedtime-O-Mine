﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    private Animator animator;
    private string currentState;
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

        if (Input.GetKeyDown(KeyCode.K))
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

    public Vector2 GetDirection()
    {
        switch (currentState)
        {
            case "Player_Walk_SideFace":
            case "Player_Attack_Idle_SideFace":
            case "Player_Attack_Walk_SideFace":
            case "Player_Idle_SideFace":
                return new Vector2(-transform.localScale.x, 0);
            case "Player_Walk_Front":
            case "Player_Idle_Front":
            case "Player_Attack_Walk_Front":
            case "Player_Attack_Idle_Front":
                return new Vector2(0, -1);
            case "Player_Walk_Back":
            case "Player_Idle_Back":
            case "Player_Attack_Walk_Back":
            case "Player_Attack_Idle_Back":
                return new Vector2(0, 1);
            default:
                return Vector2.one;
        }
    }

    private void ChangeAnimation()
    {
        if (rb.velocity.x != 0 && !isAttacking)
        {
            ChangeAnimationState("Player_Walk_SideFace");
        }
        else if (rb.velocity.y != 0 && !isAttacking)
        {
            if (rb.velocity.y > 0)
            {
                ChangeAnimationState("Player_Walk_Back");
            }
            else if (rb.velocity.y < 0)
            {
                ChangeAnimationState("Player_Walk_Front");
            }
        }
        else if (rb.velocity.x == 0 && rb.velocity.y == 0 && !isAttacking)
        {
            switch (currentState)
            {
                case "Player_Walk_SideFace":
                case "Player_Attack_Walk_SideFace":
                case "Player_Attack_Idle_SideFace":
                    ChangeAnimationState("Player_Idle_SideFace");
                    break;
                case "Player_Walk_Back":
                case "Player_Attack_Walk_Back":
                case "Player_Attack_Idle_Back":
                    ChangeAnimationState("Player_Idle_Back");
                    break;
                case "Player_Walk_Front":
                case "Player_Attack_Walk_Front":
                case "Player_Attack_Idle_Front":
                    ChangeAnimationState("Player_Idle_Front");
                    break;
                default:
                    break;
            }
        }
        if (isAttacking)
        {
            switch (currentState)
            {
                case "Player_Walk_SideFace":
                    ChangeAnimationState("Player_Attack_Walk_SideFace");
                    break;
                case "Player_Walk_Back":
                    ChangeAnimationState("Player_Attack_Walk_Back");
                    break;
                case "Player_Walk_Front":
                    ChangeAnimationState("Player_Attack_Walk_Front");
                    break;
                case "Player_Idle_SideFace":
                    ChangeAnimationState("Player_Attack_Idle_SideFace");
                    break;
                case "Player_Idle_Back":
                    ChangeAnimationState("Player_Attack_Idle_Back");
                    break;
                case "Player_Idle_Front":
                    ChangeAnimationState("Player_Attack_Idle_Front");
                    break;
                default:
                    break;
            }
            attackDelay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("AttackComplete", attackDelay);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void AttackComplete()
    {
        isAttacking = false;
    }
}