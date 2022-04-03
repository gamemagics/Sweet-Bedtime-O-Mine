using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClockAgent : MonoBehaviour {
    private enum ClockState {
        WALK, ATTACK
    }

    [SerializeField] private GameObject bulletPrefab = null;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject player = null;

    private ClockState state = ClockState.WALK;

    private static readonly float WALK_TIME = 3.0f;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator.SetFloat("Interval", WALK_TIME);
    }

    void Update() {
        switch (state) {
            case ClockState.WALK:
                UpdateWalk();
                break;
            case ClockState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    void UpdateWalk() {
        float time = animator.GetFloat("Interval");
        time -= Time.deltaTime;

        if (time <= 0.0f) {
            agent.SetDestination(transform.position);
            animator.SetFloat("Interval", -1);
            animator.ResetTrigger("Reset");
            state = ClockState.ATTACK;
        }
        else {
            animator.SetFloat("Interval", time);
            agent.SetDestination(player.transform.position);
        }

        if (agent.velocity.x * transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void UpdateAttack() {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        if (direction.x * transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Attack")) {
            GameObject bullet = GameObject.Instantiate<GameObject>(bulletPrefab);
            bullet.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
            ClockBullet clockBullet = bullet.GetComponent<ClockBullet>();
            clockBullet.direction = direction;

            bullet.name = "Bullet";
            bullet.tag = "EnemyAttack";

            animator.SetFloat("Interval", WALK_TIME);
            animator.SetTrigger("Reset");
            state = ClockState.WALK;
        }
    }
}
