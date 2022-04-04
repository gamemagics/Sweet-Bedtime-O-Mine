using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float damage;
    public float projectileForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 direction = transform.position.normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            //projectile.GetComponent<>
        }
    }
}
