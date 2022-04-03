using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDrillAgent : MonoBehaviour {
    private enum ElectricDrillState {
        IDLE, RUSH
    }

    private ElectricDrillState state = ElectricDrillState.IDLE;

    private Animator animator;

    private GameObject player = null;

    private static readonly float COLD_DOWN = 5.0f;
    private static readonly float SPEED = 5.0f;

    private Vector2 direction = Vector2.left;

    void Awake() {
        animator = GetComponent<Animator>();
        animator.SetFloat("Interval", -1);
    }

    void Update() {
        switch (state) {
            case ElectricDrillState.IDLE:
                UpdateIdle();
                break;
            case ElectricDrillState.RUSH:
                UpdateRush();
                break;
        }
    }

    void UpdateIdle() {
        float interval = animator.GetFloat("Interval");
        if (interval >= 0) {
            interval -= Time.deltaTime;
            if (interval <= 0) {
                animator.SetFloat("Interval", -1);
            }
            else {
                animator.SetFloat("Interval", interval);
            }
        }
        else if (player != null) {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            var res = Physics2D.RaycastAll(transform.position, dir);
            bool flag = true;
            if (res != null) {
                for (int i = 0; i < res.Length; ++i) {
                    RaycastHit2D hit = res[i];
                    if (hit.transform.gameObject.tag != "Player" && !hit.transform.gameObject.tag.Contains("Enemy")) {
                        Debug.Log(hit.transform.gameObject.name);
                        flag = false;
                        break;
                    }
                }
            }

            if (flag) {
                direction = (transform.position.x >= player.transform.position.x) ? Vector2.left : Vector2.right;
                animator.SetBool("FoundPlayer", true);
                state = ElectricDrillState.RUSH;
            }
            
        }
    }

    void UpdateRush() {
        transform.Translate(direction * SPEED * Time.deltaTime);
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

    public void Stop() {
        if (state == ElectricDrillState.RUSH) {
            animator.SetFloat("Interval", COLD_DOWN);
            state = ElectricDrillState.IDLE;
        }
    }
}
