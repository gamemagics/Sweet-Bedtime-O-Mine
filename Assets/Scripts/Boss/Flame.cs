using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {
    private Animator animator = null;
    private bool flag = false;

    private static readonly float FIRE_TIME = 2.0f;
    private float timer = 0.0f;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Ready")) {
            flag = true;
            animator.SetTrigger("Hit");
        }
        else if (info.IsName("Base Layer.Fire")) {
            timer += Time.deltaTime;
            if (timer >= FIRE_TIME) {
                DestroyImmediate(gameObject);
            }
        }
    }
}
