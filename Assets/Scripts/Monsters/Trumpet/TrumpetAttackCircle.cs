﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetAttackCircle : MonoBehaviour {
    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider2D;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();

        Vector3[] positions = new Vector3[100];
        Vector2[] points = new Vector2[200];
        const float dt = 3.14159265f / 200.0f;
        float theta = 0.0f;
        positions[0] = new Vector3(1.0f, 0.0f, 0);
        points[0] = new Vector2(1.0f, 0.0f);
        for (int i = 1; i < 100; ++i) {
            theta += dt;
            positions[i] = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            points[i] = new Vector3(Mathf.Cos(theta) * (1 + lineRenderer.startWidth / 2), Mathf.Sin(theta) * (1 + lineRenderer.startWidth / 2), 0);
        }
        
        for (int i = 100; i < 200; ++i) {
            points[i] = new Vector3(Mathf.Cos(theta) * (1 - lineRenderer.startWidth / 2), Mathf.Sin(theta) * (1 - lineRenderer.startWidth / 2), 0);
            theta -= dt;
        }

        lineRenderer.SetPositions(positions);
        polygonCollider2D.SetPath(0, points);
    }
}
