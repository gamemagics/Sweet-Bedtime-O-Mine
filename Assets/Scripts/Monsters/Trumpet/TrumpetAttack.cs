using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetAttack : MonoBehaviour {
    private static readonly float ATTACK_INTERVAL = 1.0f;
    private float timer = ATTACK_INTERVAL;

    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (timer < ATTACK_INTERVAL) {
            timer += Time.deltaTime;
            if (timer >= ATTACK_INTERVAL) {
                animator.SetTrigger("Ready");
            }
        }
    }

    public void Attack(Vector2 direction) {
        if (timer < ATTACK_INTERVAL) {
            return;
        }
        
        // TODO:
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Attack")) {
            animator.ResetTrigger("Ready");
            timer = 0.0f;
        }
    }
}
