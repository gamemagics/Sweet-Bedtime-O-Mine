using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDrillCollider : MonoBehaviour
{
    private static readonly int DAMAGE = 4;

    void OnTriggerEnter2D(Collider2D collider)
    {
        transform.parent.gameObject.GetComponent<ElectricDrillAgent>().Stop();

        if (!collider.tag.Contains("Enemy") && !collider.tag.Contains("Attack"))
        {
            if (collider.tag == "Player")
            {
                var pb = collider.gameObject.GetComponent<PlayerBehavior>();
                pb.TakeDamage(DAMAGE);
            }
        }
    }
}
