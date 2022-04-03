using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explore : MonoBehaviour {
    private GameObject player = null;

    private Animator animator = null;

    void Awake() {
        animator = transform.parent.gameObject.GetComponent<Animator>();
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

    void Update() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Explore")) {
            Debug.Log(1);
            DestroyImmediate(transform.parent.gameObject);
        }
        else if (info.normalizedTime >= 0.5f && info.IsName("Base Layer.Explore") && player != null) {
            
        }
    }
}
