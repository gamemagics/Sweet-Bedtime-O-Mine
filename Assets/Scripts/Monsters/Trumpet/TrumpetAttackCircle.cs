using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetAttackCircle : MonoBehaviour {
    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider2D;

    public float speed = 0.08f;
    public Vector2 direction = Vector2.zero;

    private static readonly int DAMAGE = 3;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();

        Vector3[] positions = new Vector3[20];
        Vector2[] points = new Vector2[40];
        const float dt = Mathf.PI / 40.0f;
        float theta = 0.0f;
        positions[0] = new Vector3(1.0f, 0.0f, 0);
        points[0] = new Vector2(1.0f, 0.0f);
        for (int i = 1; i < 20; ++i) {
            theta += dt;
            positions[i] = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            points[i] = new Vector3(Mathf.Cos(theta) * (1 + lineRenderer.startWidth / 2), Mathf.Sin(theta) * (1 + lineRenderer.startWidth / 2), 0);
        }
        
        for (int i = 20; i < 40; ++i) {
            points[i] = new Vector3(Mathf.Cos(theta) * (1 - lineRenderer.startWidth / 2), Mathf.Sin(theta) * (1 - lineRenderer.startWidth / 2), 0);
            theta -= dt;
        }

        lineRenderer.SetPositions(positions);
        polygonCollider2D.SetPath(0, points);
    }

    void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
        float s = transform.localScale.x * 1.05f;
        transform.localScale = new Vector3(s, s, s);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.tag.Contains("Enemy") && !collider.tag.Contains("Attack")) {
            if (collider.tag == "Player") {
                var pb = collider.gameObject.GetComponent<PlayerBehavior>();
                pb.HP -= Mathf.Max(1, pb.defendence - DAMAGE);
            }
            
            Destroy(gameObject);
        }
    }
}
