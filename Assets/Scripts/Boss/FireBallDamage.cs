using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamage : MonoBehaviour {
    private bool flag = false;

    public void Damage() {
        flag = true;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (flag && collider.gameObject.tag == "Player") {
            // TODO:
        }
    }
}
