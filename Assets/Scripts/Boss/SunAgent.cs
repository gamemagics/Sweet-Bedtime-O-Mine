using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAgent : MonoBehaviour {
    private Animator animator = null;
    private GameObject player = null;
    private static readonly float INTERVAL = 1.5f;
    private float timer = 0.0f;

    [SerializeField] private GameObject fireBallPrefab = null;
    [SerializeField] private GameObject flamePrefab = null;

    private System.Random random = null;

    void Awake() {
        random = new System.Random((int)Time.time);
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
                for (int i = 0; i < 10; ++i) {
                    AttackFireBall();
                }
                
            }
            else {
                for (int i = 0; i < 10; ++i) {
                    AttackFlame();    
                }
            }
        }
    }

    void AttackFireBall() {
        GameObject ball = GameObject.Instantiate<GameObject>(fireBallPrefab);
        ball.tag = "EnemyAttack";
        ball.name = "ball";
        ball.transform.position = new Vector3(random.Next(-3, 3), random.Next(-3, 3), transform.position.z);
    }

    void AttackFlame() {
        GameObject flame = GameObject.Instantiate<GameObject>(flamePrefab);
        flame.tag = "EnemyAttack";
        flame.name = "flame";
        flame.transform.position = new Vector3(random.Next(-3, 3), random.Next(-3, 3), transform.position.z);
    }
}
