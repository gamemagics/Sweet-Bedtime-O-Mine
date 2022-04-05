using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [SerializeField] private BonusData[] bonusData;
    private System.Random random;
    private GameObject player;

    private SpriteRenderer spriteRenderer;

    private BonusData current;

    private bool done = false;

    void Awake()
    {
        random = new System.Random(System.DateTime.Now.Second);
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int index = random.Next(0, bonusData.Length);
        current = bonusData[index];
        spriteRenderer.sprite = current.sprite;
    }

    void Update()
    {
        if (done)
        {
            DestroyImmediate(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var pb = player.GetComponent<PlayerBehavior>();
        if (current.prop == "HP")
        {
            pb.HP = Mathf.Min(pb.HP + current.delta, PlayerBehavior.MAX_HP);
        }
        else if (current.prop == "Speed")
        {
            pb.moveSpeed += current.delta;
        }
        else if (current.prop == "Damage")
        {
            pb.damage += current.delta;
        }
        else
        {
            pb.defendence += current.delta;
        }
        HintUI hint = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintUI>();
        hint.ShowHint(current.text);
        hint.HideHint();
        done = true;
    }
}
