using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerBehavior : MonoBehaviour
{
    public int moveSpeed = 5;
    public Rigidbody2D rb;
    Vector2 movement;
    private Animator animator;
    private string currentState;
    private bool isAttackPressed;
    private bool isAttacking;
    [SerializeField]
    private float attackDelay = 0.3f;

    public static readonly int MAX_HP = 10;
    public int HP = MAX_HP;
    public int defendence = 0;
    public int damage = 2;

    [SerializeField] private RectTransform HPBar;

    [SerializeField] private Attack attackObj;
    private static readonly float INIT_X = 280f;
    private bool isInvincible = false;
    [SerializeField] private float hurtForce = 0.5f;
    [SerializeField] private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            EndUI.isHappy = false;
            SceneManager.LoadScene(2);
        }

        HPBar.transform.localPosition = new Vector3((float)HP / MAX_HP * INIT_X,
             HPBar.transform.localPosition.y, HPBar.transform.localPosition.z);
        attackObj.damage = Mathf.Max(damage / 2, 1);
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

    public void TakeDamage(int damage)
    {
        DoTakeDamage(damage);
    }
    public void TakeDamage(int damage, Vector3 position)
    {
        DoTakeDamage(damage);

        Vector2 direction = transform.position - position;
        Vector2 pos = transform.position;

        // rb.velocity = rb.velocity + direction * hurtForce;
        transform.DOMove(rb.position + direction * hurtForce, 0.1f);
        // (pos + direction * hurtForce);
    }
    public void DoTakeDamage(int damage)
    {
        if (isInvincible) return;
        isInvincible = true;
        Blink();

        cam.transform.DOShakePosition(0.2f, 0.1f, 20, 90, false, true);
        HP -= Mathf.Max(1, damage - defendence);
        Invoke("ResetInvincible", 1f);
    }
    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void ResetInvincible()
    {
        isInvincible = false;
    }
    private void Blink()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Sequence seq = DOTween.Sequence();
        seq.Append(sprite.DOColor(new Color32(0, 0, 0, 0), 0.1f));
        seq.Append(sprite.DOColor(Color.white, 0.1f));
        seq.SetLoops(5);
    }
}