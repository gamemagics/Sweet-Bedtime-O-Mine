using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterDamageReceiver : MonoBehaviour
{

    [SerializeField] private int maxHP = 1;
    [SerializeField] private int HP = 1;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private GameObject effect;
    [SerializeField] private Camera cam;
    // private float dazedTime;
    // public float startDazedTime;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        // if(dazedTime<=0){

        // }
        if (isBoss)
        {
            var animator = transform.parent.gameObject.GetComponent<Animator>();
            animator.SetFloat("Process", (float)HP / maxHP);
            // Debug.Log("Process:" + (float)HP / maxHP);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerAttack")
        {
            int dmg = collider.gameObject.GetComponent<ProjectileBehavior>().damage;
            TakeDamage(dmg);
        }
    }

    public void TakeDamage(int dmg)
    {
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        AudioManager.Instance.MonsterHurtAudio();
        HP -= dmg;
        Invoke("ResetColor", 0.1f);
        if (HP <= 0)
        {
            cam.transform.DOShakePosition(0.2f, 0.3f, 20, 90, false, true);
            Instantiate(effect, transform.parent.position, Quaternion.identity);
            if (isBoss)
            {
                BossManager.Instance.Clear();
            }
            else
            {
                DungeonManager.Instance.ReportDeath();
            }
            Destroy(transform.parent.gameObject);
        }
    }

    private void ResetColor()
    {
        GetComponentInParent<SpriteRenderer>().color = Color.white;
    }
}
