using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDrillCollider : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider) {
        transform.parent.gameObject.GetComponent<ElectricDrillAgent>().Stop();
    }
}
