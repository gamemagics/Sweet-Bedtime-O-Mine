using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int damage = 1;
    public float projectileForce = 10f;
    public float attackRate = 0.1f;
    private float nextAttack;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            DoAttack();
        }

    }
    private void DoAttack()
    {
        Vector2 direction = this.GetComponent<PlayerBehavior>().GetDirection();
        if (direction.x == 1 && direction.y == 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x + GetPosition().x / 2, transform.position.y), Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectile.GetComponent<ProjectileBehavior>().damage = damage;
        }
        else if (direction.x == -1 && direction.y == 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x - GetPosition().x / 2, transform.position.y), Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectile.GetComponent<ProjectileBehavior>().damage = damage;
        }
        else if (direction.x == 0 && direction.y == 1)
        {
            GameObject projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x, transform.position.y + GetPosition().y / 2), Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectile.GetComponent<ProjectileBehavior>().damage = damage;
        }
        else if (direction.x == 0 && direction.y == -1)
        {
            GameObject projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x, transform.position.y - GetPosition().y / 2), Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectile.GetComponent<ProjectileBehavior>().damage = damage;
        }
    }
    private Vector2 GetPosition()
    {
        Vector3 size = GetComponent<Collider2D>().bounds.size;
        float length = size.x;
        float width = size.y;
        return new Vector2(length, width);
    }
}
