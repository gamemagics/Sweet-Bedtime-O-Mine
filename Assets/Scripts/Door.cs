using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool flag = false;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            flag = true;
        }
    }

    void Update() {
        if (flag) {
            DungeonManager.Instance.GoNextRoom();
        }
    }
}
