using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeUI : MonoBehaviour {
    [SerializeField] private SpriteRenderer render1 = null;
    [SerializeField] private SpriteRenderer render2 = null;

    private static readonly float INTERVAL = 0.5f;
    private float timer = 0.0f;

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= INTERVAL) {
            timer -= INTERVAL;
            float t = render1.color.a;
            render1.color = new Color(render1.color.r, render1.color.g, render1.color.b, render2.color.a);
            render2.color = new Color(render2.color.r, render2.color.g, render2.color.b, t);
        }
    }
}
