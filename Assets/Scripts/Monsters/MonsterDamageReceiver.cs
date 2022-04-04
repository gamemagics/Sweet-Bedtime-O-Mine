using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageReceiver : MonoBehaviour {
    [SerializeField] private int HP = 1;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerAttack") {
            int dmg = collider.gameObject.GetComponent<ProjectileBehavior>().damage;
            HP -= dmg;
            if (HP <= 0) {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
