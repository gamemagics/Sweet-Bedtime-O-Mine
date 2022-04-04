using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;

    private FireBallDamage damage = null;

    [SerializeField] private GameObject fall = null;
    [SerializeField] private GameObject crash = null;

    void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = fall.GetComponent<SpriteRenderer>();
        damage = crash.GetComponent<FireBallDamage>();
    }

    void Update() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Crash")) {
            DestroyImmediate(gameObject);
        }
        else if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Fall")) {
            animator.SetTrigger("Hit");
            spriteRenderer.color = new Color(0, 0, 0, 0);
            damage.Damage();
        }
    }
}
