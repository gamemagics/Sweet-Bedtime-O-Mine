using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour {
    [SerializeField] private BonusData[] bonusData;
    private System.Random random;
    private GameObject player;

    private SpriteRenderer spriteRenderer;

    private BonusData current;

    private bool done = false;

    void Awake() {
        random = new System.Random((int)Time.time);
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        int index = random.Next(0, bonusData.Length);
        current = bonusData[index];
        spriteRenderer.sprite = current.sprite;
    }

    void Update() {
        if (done) {
            DestroyImmediate(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // TODO:
        TMPro.TMP_Text text = GameObject.FindGameObjectWithTag("Hint").GetComponent<TMPro.TMP_Text>();
        text.text = current.text;
        done = true;
    }
}
