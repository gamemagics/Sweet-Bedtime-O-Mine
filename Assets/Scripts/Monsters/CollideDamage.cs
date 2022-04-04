using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDamage : MonoBehaviour
{
    private static readonly int DAMAGE = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            var pb = collider.gameObject.GetComponent<PlayerBehavior>();
            pb.TakeDamage(DAMAGE);
        }
    }
}