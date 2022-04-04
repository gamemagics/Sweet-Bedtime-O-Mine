using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonManager : MonoBehaviour {
    [SerializeField] private int[] layerRoomCount;
    [SerializeField] private GameObject[] bossPrefab;

    [SerializeField] private RandomDungeonGenerator generator;

    [SerializeField] private GameObject bonusPrefab;

    [SerializeField] private NavMeshSurface2d surface2D;

    private GameObject currentRoom = null;

    private static DungeonManager instance = null;

    private bool isBoss = false;
    private int roomNunber = 0;
    private int layer = 0;

    [SerializeField] private Grid grid;

    [SerializeField] private GameObject player;

    void Awake() {
        instance = this;
    }

    public static DungeonManager Instance {
        get {
            return instance;
        }
    }

    public void GoNextRoom() {
        if (currentRoom != null) {
            DestroyImmediate(currentRoom);
            currentRoom = null;
        }
        
        if (isBoss) {
            ++layer;
            roomNunber = 0;
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
            grid.gameObject.SetActive(false);
        }
        else {
            if (roomNunber == 0) {
                grid.gameObject.SetActive(true);
            }
            ++roomNunber;
            
            generator.Generate();
        }

        player.transform.position = Vector2.zero;
        surface2D.BuildNavMesh();
    }

    void Start() {
        currentRoom = GameObject.Instantiate<GameObject>(bonusPrefab);
        currentRoom.transform.position = Vector3.zero;
    }
}
