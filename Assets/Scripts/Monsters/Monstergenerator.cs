using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstergenerator : MonoBehaviour {
    public enum MonsterType {
        CLOCK = 0, DIGITAL_CLOCK = 1, TRUMPET = 2, ELECTRIC_DRILL = 3, PENDULUM_CLOCK = 4
    }

    [SerializeField]
    private GameObject[] monsterPrefabs = null;

    private static Monstergenerator instance = null;

    void Awake() {
        instance = this;
    }

    public static Monstergenerator Instance {
        get {
            return instance;
        }
    }

    public GameObject GenerateMonster(MonsterType type) {
        GameObject monster = null;
        int index = (int)type;
        if (monsterPrefabs == null || index >= monsterPrefabs.Length) {
            Debug.LogError("No available monster prefab.");
        }
        else {
            monster = GameObject.Instantiate<GameObject>(monsterPrefabs[index]);
        }

        return monster;
    }
}
