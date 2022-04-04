using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DigitalClockAgent : MonoBehaviour {
    private enum DigitalClockState {
        RUN, SHUT_DOWN, RESTART
    }

    private DigitalClockState state = DigitalClockState.RUN;
    private Animator animator;

    private GameObject player = null;

    public string timeString = "00:00";

    private static readonly float SPEED = 0.5f;

    private static readonly float RUN_DISTANCE = 4f;

    [SerializeField] private Text text = null;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update() {
        text.text = timeString;
        
        switch (state) {
            case DigitalClockState.RUN:
                UpdateRun();
                break;
            case DigitalClockState.SHUT_DOWN:
                break;
            case DigitalClockState.RESTART:
                UpdateRestart();
                break;
        }

        UpdateTime();
    }

    void UpdateRun() {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        if (direction.x * transform.localScale.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        
        if (Vector2.Distance(player.transform.position, transform.position) <= RUN_DISTANCE) {
            transform.Translate(-direction * SPEED * Time.deltaTime);
        }

        var dmg = GetComponentInChildren<DigitalDamageReceiver>();
        if (dmg.HP <= 0) {
            animator.SetBool("Shut", true);
            animator.ResetTrigger("Restart");
            state = DigitalClockState.SHUT_DOWN;
        }
    }

    void UpdateRestart() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Reset")) {
            animator.SetTrigger("Restart");
            state = DigitalClockState.RUN;
            var dmg = GetComponentInChildren<DigitalDamageReceiver>();
            dmg.damagable = true;
        }
        else if (!info.IsName("Base Layer.Reset")) {
            animator.SetTrigger("Restart");
            state = DigitalClockState.RUN;
            var dmg = GetComponentInChildren<DigitalDamageReceiver>();
            dmg.damagable = true;
        }
    }

    void UpdateTime() {
        text.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Reset() {
        animator.SetBool("Shut", false);
        state = DigitalClockState.RESTART;
    }
}
