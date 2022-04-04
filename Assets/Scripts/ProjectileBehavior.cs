﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Projectile hit " + other.name);
        if (other.gameObject.tag == "EnemyDamage" || other.gameObject.tag == "Wall")
            Destroy(gameObject);
    }
}
