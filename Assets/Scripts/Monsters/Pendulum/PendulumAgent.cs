using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PendulumAgent : MonoBehaviour
{
    private enum PendulumState
    {
        IDLE, WALK, ATTACK
    }

    private NavMeshAgent agent;
    private Animator animator;

    private PendulumState state = PendulumState.IDLE;

    private System.Random random;

    private static readonly float WAIT_TIME = 3.0f;

    private static readonly float ATTACK_RANGE = 3f;

    public Vector2[] cruisePoint = new Vector2[5];
    private int previousPoint = -1;

    [SerializeField] private GameObject bombPrefab = null;

    private Vector2 attackPoint = Vector2.zero;
    private GameObject player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        random = new System.Random(System.DateTime.Now.Second);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator.SetFloat("Wait", WAIT_TIME);
    }

    void Update()
    {
        switch (state)
        {
            case PendulumState.IDLE:
                UpdateIdle();
                break;
            case PendulumState.WALK:
                UpdateWalk();
                break;
            case PendulumState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        float wait = animator.GetFloat("Wait");
        if (wait > 0)
        {
            wait -= Time.deltaTime;
            animator.SetFloat("Wait", wait);
        }
        else
        {
            int index = random.Next(0, cruisePoint.Length);
            if (index == previousPoint)
            {
                index = (index + 1) % cruisePoint.Length;
            }

            previousPoint = index;
            Vector2 dest = cruisePoint[index];
            agent.SetDestination(new Vector3(dest.x, dest.y, transform.position.z));

            animator.SetFloat("Wait", -1);

            state = PendulumState.WALK;
        }
    }

    void UpdateWalk()
    {
        Vector2 dis = agent.destination - transform.position;
        if (Vector2.Distance(agent.destination, transform.position) < 0.1f)
        {
            animator.SetTrigger("Fire");
            agent.SetDestination(transform.position);
            state = PendulumState.ATTACK;
            attackPoint = new Vector2(random.Next(-1, 1), random.Next(-1, 1));
        }

        if (agent.velocity.x * transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void UpdateAttack()
    {
        if (attackPoint.x * transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Attack"))
        {
            Vector2 p = attackPoint;
            p.Normalize();
            p *= ATTACK_RANGE * (float)random.NextDouble();

            GameObject bomb = GameObject.Instantiate<GameObject>(bombPrefab);
            Vector2 direction = player.transform.position - transform.position;
            bomb.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            AudioManager.Instance.ThrowAudio();
            bomb.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            bomb.GetComponentInChildren<Explore>().target = new Vector2(transform.position.x + p.x, transform.position.y + p.y);
            bomb.name = "bomb";
            bomb.tag = "EnemyAttack";

            animator.SetFloat("Wait", WAIT_TIME);
            animator.ResetTrigger("Fire");
            state = PendulumState.IDLE;
        }
    }
}
