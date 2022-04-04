using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageReceiver : MonoBehaviour {
    [SerializeField] private int HP = 1;
    [SerializeField] private bool isBoss = false;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerAttack") {
            int dmg = collider.gameObject.GetComponent<ProjectileBehavior>().damage;
            HP -= dmg;
            if (HP <= 0) {
                if (isBoss) {
                    
                }
                else {
                    DungeonManager.Instance.ReportDeath();
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
