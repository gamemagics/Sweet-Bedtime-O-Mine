using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject attackEffectPrefab;
    public int damage = 1;
    public float projectileForce = 10f;
    public float shootRate = 0.1f;
    private float nextShoot;
    public float attackRate = 0.5f;
    private float nextAttack;

    public Transform attackPos;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private GameObject projectile;
    private GameObject attackEffect;
    [SerializeField] private Sprite attackSprite;

    void Update()
    {
        if (Input.GetKey(KeyCode.J) && Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            Shoot();
        }
        if (Input.GetKey(KeyCode.K) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            Invoke("ShowEffect", 0.1f);
            Invoke("ChangeSprite", 0.2f);
            Invoke("DestroyEffect", 0.3f);

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in enemiesToDamage)
            {
                enemy.GetComponentInChildren<MonsterDamageReceiver>().TakeDamage(damage);
            }
        }
    }
    private void Shoot()
    {
        Vector2 direction = this.GetComponent<PlayerBehavior>().GetDirection();
        if (direction.x == 1 && direction.y == 0)
        {
            projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x + GetPosition().x / 2, transform.position.y), Quaternion.identity);
        }
        else if (direction.x == -1 && direction.y == 0)
        {
            projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x - GetPosition().x / 2, transform.position.y), Quaternion.identity);
        }
        else if (direction.x == 0 && direction.y == 1)
        {
            projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x, transform.position.y + GetPosition().y / 2), Quaternion.identity);
        }
        else if (direction.x == 0 && direction.y == -1)
        {
            projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x, transform.position.y - GetPosition().y / 2), Quaternion.identity);
        }
        if (projectile != null)
        {
            projectile.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).From(Vector3.zero);
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
    private void ShowEffect()
    {
        attackEffect = Instantiate(attackEffectPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
    private void ChangeSprite()
    {
        if (attackEffect != null)
            attackEffect.GetComponent<SpriteRenderer>().sprite = attackSprite;
    }
    private void DestroyEffect()
    {
        if (attackEffect != null)
            Destroy(attackEffect);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
