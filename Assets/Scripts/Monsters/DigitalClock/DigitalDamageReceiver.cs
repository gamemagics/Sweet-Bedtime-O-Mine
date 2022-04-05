using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitEvent : UnityEvent<int> {}

public class DigitalDamageReceiver : MonoBehaviour {
    public int HP;

    public HitEvent OnShutDown = new HitEvent();

    public int index = 0;

    public bool damagable = true;

    void OnTriggerEnter2D(Collider2D collider) {
        if (damagable && collider.tag == "PlayerAttack") {
            AudioManager.instance.MonsterHurtAudio();
            int dmg = collider.gameObject.GetComponent<ProjectileBehavior>().damage;
            HP -= dmg;
            if (HP <= 0) {
                damagable = false;
                OnShutDown.Invoke(index);
            }
        }
    }
}
