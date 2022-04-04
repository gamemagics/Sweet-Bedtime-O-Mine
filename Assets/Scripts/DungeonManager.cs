using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {
    [SerializeField] private int[] layerRoomCount;
    [SerializeField] private GameObject[] bossPrefab;

    [SerializeField] private RandomDungeonGenerator generator;

    [SerializeField] private GameObject bonusPrefab;

    private GameObject currentRoom = null;

    private static DungeonManager instance = null;

    private bool isBoss = false;
    private int roomNunber = 0;
    private int layer = 0;

    [SerializeField] private Grid grid;

    void Awake() {
        instance = this;
    }

    public static DungeonManager Instance {
        get {
            return instance;
        }
    }

    public void GoNextRoom() {
        grid.gameObject.SetActive(false);
        if (currentRoom != null) {
            DestroyImmediate(currentRoom);
            currentRoom = null;
        }
        
        if (isBoss) {
            ++layer;
            if (layer == layerRoomCount.Length) {
                // TODO:
            }
            else {
                currentRoom = GameObject.Instantiate<GameObject>(bonusPrefab);
                currentRoom.transform.position = Vector3.zero;
            }
        }
        else if (roomNunber == layerRoomCount[layer]) {
            // BOSS
        }
        else {
            ++roomNunber;
            generator.Generate();
            grid.gameObject.SetActive(true);
        }
    }

    void Start() {
        currentRoom = GameObject.Instantiate<GameObject>(bonusPrefab);
        currentRoom.transform.position = Vector3.zero;
    }
}
