using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour {
    public static bool isHappy = true;
    [SerializeField] private SpriteRenderer renderer1;
    [SerializeField] private SpriteRenderer renderer2;
    [SerializeField] private SpriteRenderer renderer3;
    [SerializeField] private SpriteRenderer renderer4;

    [SerializeField] private AudioClip happyAudio;
    [SerializeField] private AudioClip badAudio;

    [SerializeField] private AudioSource audioSource;

    private static readonly float INTERVAL = 0.5f;
    private float timer = 0.0f;

    private SpriteRenderer img1, img2;

    void Awake() {
        if (isHappy) {
            renderer1.color = new Color(1, 1, 1, 1);
            img1 = renderer1; img2 = renderer2;
            audioSource.clip = happyAudio;
        }
        else {
            renderer3.color = new Color(1, 1, 1, 1);
            img1 = renderer3; img2 = renderer4;
            audioSource.clip = badAudio;
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

    void Start() {
        audioSource.Play();
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= INTERVAL) {
            timer -= INTERVAL;
            float t = img1.color.a;
            img1.color = new Color(1, 1, 1, img2.color.a);
            img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, t);
        }
    }
}
