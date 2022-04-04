using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBullet : MonoBehaviour {
    public Vector2 direction;

    private static readonly float speed = 1f;

    void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "EnemySearch" && collider.tag != "EnemyDamage") {
            // TODO:
            Destroy(gameObject);
        }
    }
}
