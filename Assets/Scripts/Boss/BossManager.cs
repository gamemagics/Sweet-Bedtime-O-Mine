using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {
    private static BossManager instance = null;
    [SerializeField] private Door door;

    public static BossManager Instance {
        get {
            return instance;
        }
    }

    void Awake() {
        instance = this;
    }

    public void Clear() {
        door.enable = true;
        for (int i = 0; i < transform.childCount; ++i) {
            Destroy(transform.GetChild(i));
        }
    }
}
