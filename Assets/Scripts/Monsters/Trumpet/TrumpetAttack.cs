using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetAttack : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;

    private static readonly float ATTACK_INTERVAL = 2.0f;
    private float timer = ATTACK_INTERVAL;

    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (timer < ATTACK_INTERVAL) {
            timer += Time.deltaTime;
            if (timer >= ATTACK_INTERVAL) {
                animator.SetTrigger("Ready");
            }
        }
    }

    public void Attack(Vector2 direction) {
        if (timer < ATTACK_INTERVAL) {
            return;
        }
        
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("Base Layer.Attack")) {
            animator.ResetTrigger("Ready");
            timer = 0.0f;
            float[] scales = new float[] {0.75f, 0.5f, 0.25f};
            Vector2 originDir = new Vector2(Mathf.Sqrt(2) / 2.0f, Mathf.Sqrt(2) / 2.0f);

            GameObject[] bullets = new GameObject[3];
            for (int i = 0; i < 3; ++i) {
                bullets[i] = GameObject.Instantiate<GameObject>(bulletPrefab);
                float theta = Mathf.Acos(Vector2.Dot(originDir, direction));
                if (originDir.x * direction.y - originDir.y * direction.x < 0) {
                    theta = -theta;
                }

                bullets[i].transform.Rotate(new Vector3(0, 0, theta * 180 / Mathf.PI));

                bullets[i].transform.position = new Vector3(transform.position.x + direction.x * scales[i],
                 transform.position.y + direction.y * scales[i], transform.position.z);
                bullets[i].transform.localScale = new Vector3(scales[i], scales[i], scales[i]);
                bullets[i].name = "Bullet";
                bullets[i].tag = "EnemyAttack";

                TrumpetAttackCircle tac = bullets[i].GetComponent<TrumpetAttackCircle>();
                tac.speed *= scales[i];
                tac.direction = direction;
            }
        }
    }
}
