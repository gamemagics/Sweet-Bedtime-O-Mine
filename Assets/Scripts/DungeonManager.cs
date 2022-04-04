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

    private System.Random random;

    private int remains = 3;

    void Awake() {
        random = new System.Random(System.DateTime.Now.Second);
        instance = this;
    }

    public void ReportDeath() {
        --remains;
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
        GenerateMonsters();
    }

    void GenerateMonsters() {
        var type = (Monstergenerator.MonsterType)random.Next(0, 5);

        if (type == Monstergenerator.MonsterType.DIGITAL_CLOCK) {
            remains = 1;
            GameObject monster = Monstergenerator.Instance.GenerateMonster(type);
            monster.transform.position = Vector3.zero;
            DigitalClockProxy proxy = monster.GetComponent<DigitalClockProxy>();
            
            var color = (DigitalClockProxy.DigitalClockColor)random.Next(0, 3);
            Vector2[] pos = new Vector2[3];
            int[] prev = new int[3];
            for (int i = 0; i < 3; ++i) {
                int posIndex = random.Next(0, generator.availablePosition.Count);
                bool repeat = false;
                do {
                    repeat = false;
                    for (int j = 0; j < i; ++j) {
                        if (prev[j] == posIndex) {
                            repeat = true;
                            break;
                        }
                    }
                }while(repeat);

                prev[i] = posIndex;
                pos[i] = generator.availablePosition[posIndex];
            }

            proxy.InitByColor(color, pos);
        }
        else {
            remains = 3;
            int[] prev = new int[remains];
            
            for (int i = 0; i < remains; ++i) {
                GameObject monster = Monstergenerator.Instance.GenerateMonster(type);
                int posIndex = random.Next(0, generator.availablePosition.Count);
                bool repeat = false;
                do {
                    repeat = false;
                    for (int j = 0; j < i; ++j) {
                        if (prev[j] == posIndex) {
                            repeat = true;
                            break;
                        }
                    }
                }while(repeat);

                prev[i] = posIndex;
                Vector2 pos = generator.availablePosition[posIndex];
                monster.transform.position = new Vector3(pos.x, pos.y, 0);

                if (type == Monstergenerator.MonsterType.TRUMPET) {
                    TrumpetAgent agent = monster.GetComponent<TrumpetAgent>();
                    Vector2[] points = new Vector2[3];
                    for (int j = 0; j < 3; ++j) {
                        posIndex = random.Next(0, generator.availablePosition.Count);
                        points[j] = generator.availablePosition[posIndex];
                    }

                    agent.cruisePoint = points;
                }
                else if (type == Monstergenerator.MonsterType.PENDULUM_CLOCK) {
                    PendulumAgent agent = monster.GetComponent<PendulumAgent>();
                    Vector2[] points = new Vector2[5];
                    for (int j = 0; j < 5; ++j) {
                        posIndex = random.Next(0, generator.availablePosition.Count);
                        points[j] = generator.availablePosition[posIndex];
                    }

                    agent.cruisePoint = points;
                }
            }
        }
    }

    void Start() {
        currentRoom = GameObject.Instantiate<GameObject>(bonusPrefab);
        currentRoom.transform.position = Vector3.zero;
    }

    void Update() {
        if (remains == 0) {
            generator.Pass();
        }
    }
}
