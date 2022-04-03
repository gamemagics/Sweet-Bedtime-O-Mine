using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageReceiver : MonoBehaviour {
    [SerializeField] private int HP;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerAttack") {
            // TODO:
            if (HP <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
