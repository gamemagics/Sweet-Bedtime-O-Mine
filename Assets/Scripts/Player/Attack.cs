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
    public float shootRate = 0.3f;
    private float nextShoot;
    public float attackRate = 0.5f;
    private float nextAttack;

    public float attackRange;
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

            List<GameObject> enemy = GetClosestEnemy();
            if (enemy != null)
            {
                foreach (GameObject enemyObj in enemy)
                {
                    var receiver = enemyObj.GetComponentInChildren<MonsterDamageReceiver>();
                    if (receiver != null)
                    {
                        // AudioManager.Instance.AttackAudio();
                        Debug.Log("Hit enemy: " + enemyObj.name);
                        receiver.TakeDamage(damage * 2);
                    }
                    else
                    {
                        var digitalReceiver = enemyObj.GetComponentInChildren<DigitalDamageReceiver>();
                        if (digitalReceiver != null)
                        {
                            // AudioManager.Instance.AttackAudio();
                            Debug.Log("Hit enemy: " + enemyObj.name);
                            digitalReceiver.TakeDamage(damage * 2);
                        }
                    }
                }
            }
        }
    }
    private List<GameObject> GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemySearch");
        if (enemies == null)
        {
            return null;
        }
        List<GameObject> closest = new List<GameObject>();
        Vector3 position = new Vector3(GetAttackPos().x, GetAttackPos().y, 0);
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) > attackRange)
            {
                continue;
            }
            closest.Add(enemy);
        }
        return closest;
    }
    private Vector2 GetAttackPos()
    {
        Vector2 direction = this.GetComponent<PlayerBehavior>().GetDirection();
        if (direction.x == 1 && direction.y == 0)
        {
            return new Vector2(transform.position.x + GetPosition().x / 2, transform.position.y);
        }
        else if (direction.x == -1 && direction.y == 0)
        {
            return new Vector2(transform.position.x - GetPosition().x / 2, transform.position.y);
        }
        else if (direction.x == 0 && direction.y == 1)
        {
            return new Vector2(transform.position.x, transform.position.y + GetPosition().y / 2);
        }
        else if (direction.x == 0 && direction.y == -1)
        {
            return new Vector2(transform.position.x, transform.position.y - GetPosition().y / 2);
        }
        else
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
    }
    private void Shoot()
    {
        Vector2 direction = this.GetComponent<PlayerBehavior>().GetDirection();
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        projectile = Instantiate(projectilePrefab, GetAttackPos(), rotation);
        // AudioManager.Instance.ShootAudio();

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
        Vector2 direction = GetComponent<PlayerBehavior>().GetDirection();
        if (direction.y != 1)
        {
            attackEffect = Instantiate(attackEffectPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            attackEffect.GetComponent<SpriteRenderer>().sortingOrder = 11;
        }
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
}
