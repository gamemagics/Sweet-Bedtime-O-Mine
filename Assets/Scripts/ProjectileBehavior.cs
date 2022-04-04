using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
        }
        if (other.gameObject.tag != "Player")
            Destroy(gameObject);
    }
}
