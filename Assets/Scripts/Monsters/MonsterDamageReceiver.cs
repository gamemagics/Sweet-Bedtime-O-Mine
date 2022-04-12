using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class MonsterDamageReceiver : MonoBehaviour
{
    [SerializeField] private int maxHP = 1;
    [SerializeField] private int HP = 1;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private GameObject effect;
    [SerializeField] private Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }
    void Start()
    {

    }
    void Update()
    {
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
            if (isBoss)
            {
                StartCoroutine(PlayBossDeath());
            }
            else
            {
                float size = cam.orthographicSize;
                cam.DOOrthoSize(size * 1.2f, 0.1f).onComplete = () =>
                {
                    cam.DOOrthoSize(size, 0.1f);
                };
                Instantiate(effect, transform.parent.position, Quaternion.identity);
                DungeonManager.Instance.ReportDeath();
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void ResetColor()
    {
        GetComponentInParent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator PlayBossDeath()
    {
        HintUI hint = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintUI>();
        hint.ShowHint("Yay! You made it!");

        var boss = GetComponentInParent<NavMeshAgent>();
        if (boss != null)
            boss.transform.DOShakePosition(1f, 1f, 20, 90, false, true).onComplete = () =>
            // boss.transform.DOShakePosition(1f, 1f, 20, 90, false, true);
        {
            boss.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(effect, transform.parent.position, Quaternion.identity);
        };

        yield return new WaitForSeconds(2f);
        EndUI.isHappy = true;
        SceneManager.LoadScene(2);
    }
}
