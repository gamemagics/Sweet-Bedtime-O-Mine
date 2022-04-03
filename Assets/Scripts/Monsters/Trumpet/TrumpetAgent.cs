﻿using UnityEngine;
using UnityEngine.AI;

public class TrumpetAgent : MonoBehaviour {
    private enum TrumpetState {
        IDLE, WALK, ATTACK
    }

    private NavMeshAgent agent;
    private Animator animator;

    private TrumpetAttack trumpetAttack;

    private GameObject player = null;

    private TrumpetState state = TrumpetState.IDLE;

    private System.Random random;

    private static readonly float WAIT_TIME = 3.0f;

    public Vector2[] cruisePoint;

    private int previousPoint = -1;

    void Awake() {
        random = new System.Random((int)Time.time);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        trumpetAttack = GetComponent<TrumpetAttack>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator.SetFloat("Wait", WAIT_TIME);
    }

    void Update() {
        switch (state) {
            case TrumpetState.IDLE:
                UpdateIdle();
                break;
            case TrumpetState.WALK:
                UpdateWalk();
                break;
            case TrumpetState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle() {
        float wait = animator.GetFloat("Wait");
        if (wait > 0 && player == null) {
            wait -= Time.deltaTime;
            animator.SetFloat("Wait", wait);
        }
        else if (player != null) {
            animator.SetBool("FoundPlayer", true);
            state = TrumpetState.ATTACK;
        }
        else {
            int index = random.Next(0, cruisePoint.Length);
            if (index == previousPoint) {
                index = (index + 1) % cruisePoint.Length;
            }

            previousPoint = index;
            Vector2 dest = cruisePoint[index];
            agent.SetDestination(new Vector3(dest.x, dest.y, transform.position.z));

            animator.ResetTrigger("Stop");
            animator.SetFloat("Wait", -1);
            
            state = TrumpetState.WALK;
        }
    }

    void UpdateWalk() {
        Vector2 dis = agent.destination - transform.position; 

        if (Vector2.Distance(agent.destination, transform.position) < 0.1f || player != null) {
            animator.SetFloat("Wait", WAIT_TIME);
            animator.SetTrigger("Stop");
            agent.SetDestination(transform.position);
            state = TrumpetState.IDLE;
        }

        if (agent.velocity.x * transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void UpdateAttack() {
        if (player == null) {
            animator.SetBool("FoundPlayer", false);
            animator.SetFloat("Wait", WAIT_TIME);
            state = TrumpetState.IDLE;
        }
        else {
            Vector2 direction = player.transform.position - transform.position;
            if (direction.x * transform.localScale.x > 0) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            direction.Normalize();
            trumpetAttack.Attack(direction);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            player = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            player = null;
        }
    }
}
