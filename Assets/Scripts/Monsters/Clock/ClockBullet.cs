﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBullet : MonoBehaviour
{
    public Vector2 direction;

    private static readonly float speed = 1f;
    private static readonly int DAMAGE = 2;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.tag.Contains("Enemy"))
        {
            if (collider.tag == "Player")
            {
                var pb = collider.gameObject.GetComponent<PlayerBehavior>();
                pb.TakeDamage(DAMAGE);
            }

            Destroy(gameObject);
        }
    }
}
