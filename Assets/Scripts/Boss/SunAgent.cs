using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAgent : MonoBehaviour {
    private Animator animator = null;
    private GameObject player = null;
    private static readonly float INTERVAL = 1.0f;
    private float timer = 0.0f;

    [SerializeField] private GameObject fireBallPrefab = null;
    [SerializeField] private GameObject[] flamePrefab = null;

    void Awake() {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (timer < INTERVAL) {
            timer += Time.deltaTime;
        }
        else {
            timer -= INTERVAL;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Base Layer.Attack1")) {
                AttackFireBall();
            }
            else {
                AttackFlame();
            }
        }
    }

    void AttackFireBall() {

    }

    void AttackFlame() {

    }
}
