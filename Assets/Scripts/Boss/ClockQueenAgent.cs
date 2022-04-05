using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClockQueenAgent : MonoBehaviour
{
    private enum ClockQueenState
    {
        IDLE, ATTACK
    }

    private NavMeshAgent agent;
    private Animator animator;

    private ClockQueenState state = ClockQueenState.IDLE;

    private System.Random random;

    private static readonly float WAIT_TIME = 3.0f;

    public Vector2[] cruisePoint;

    private int previousPoint = -1;

    [SerializeField] private GameObject bulletPrefab = null;

    void Awake()
    {
        random = new System.Random((int)Time.time);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        int index = random.Next(0, cruisePoint.Length);
        previousPoint = index;
        Vector2 dest = cruisePoint[index];
        agent.SetDestination(new Vector3(dest.x, dest.y, transform.position.z));
    }

    void Update()
    {
        switch (state)
        {
            case ClockQueenState.IDLE:
                UpdateIdle();
                break;
            case ClockQueenState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        Vector2 dis = agent.destination - transform.position;

        if (Vector2.Distance(agent.destination, transform.position) < 0.01f)
        {
            animator.SetBool("Attack", true);
            agent.SetDestination(transform.position);
            state = ClockQueenState.ATTACK;
        }

        if (agent.velocity.x * transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void UpdateAttack()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Attack1"))
        {
            for (int i = 0; i < 3; ++i) {
                Vector2 p = new Vector2(random.Next(-1, 1), random.Next(-1, 1));
                p.Normalize();
                p *= 2.5f * (float)random.NextDouble();

                GameObject child = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.CLOCK);
                child.transform.position = new Vector3(transform.position.x + p.x, transform.position.y + p.y, transform.position.z);
                child.transform.parent = BossManager.Instance.transform;
            }

            FinishAttack();
        }
        else if (info.IsName("Base Layer.Attack2"))
        {
            float theta = 0.0f;
            for (int i = 0; i < 18; ++i)
            {
                Vector2 p = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                GameObject bullet = GameObject.Instantiate<GameObject>(bulletPrefab);
                bullet.transform.position = new Vector3(transform.position.x + p.x, transform.position.y + p.y, transform.position.z);
                ClockBullet clockBullet = bullet.GetComponent<ClockBullet>();
                clockBullet.direction = p;

                bullet.name = "Bullet";
                bullet.tag = "EnemyAttack";

                theta += 2 * Mathf.PI / 18.0f;
            }

            FinishAttack();
        }
    }

    void FinishAttack()
    {
        int index = random.Next(0, cruisePoint.Length);
        if (index == previousPoint)
        {
            index = (index + 1) % cruisePoint.Length;
        }

        previousPoint = index;
        Vector2 dest = cruisePoint[index];
        agent.SetDestination(new Vector3(dest.x, dest.y, transform.position.z));

        animator.SetBool("Attack", false);
        state = ClockQueenState.IDLE;
    }
}
