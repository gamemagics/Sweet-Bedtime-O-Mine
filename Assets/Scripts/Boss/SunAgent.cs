using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAgent : MonoBehaviour {
    private GameObject player = null;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        // TODO:
    }
}
