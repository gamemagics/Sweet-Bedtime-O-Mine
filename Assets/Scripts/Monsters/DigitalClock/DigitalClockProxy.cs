using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalClockProxy : MonoBehaviour {
    public enum DigitalClockColor {
        RED = 0, YELLOW = 1, BLUE = 2
    }

    [SerializeField] private GameObject[] digitalClockPrefabs = null;

    private List<GameObject> clocks = new List<GameObject>();
    private List<int> order = new List<int>();
    private static readonly int MAX_HP = 5;

    public void InitByColor(DigitalClockColor color, Vector2[] positions) {
        for (int i = 0; i < positions.Length; ++i) {
            GameObject clock = GameObject.Instantiate<GameObject>(digitalClockPrefabs[(int)color]);
            clock.transform.position = positions[i];
            clock.transform.parent = transform;
            clocks.Add(clock);

            var agent = clock.GetComponent<DigitalClockAgent>();
            agent.timeString = "0" + (i + 1).ToString() + ":00";

            var receiver = clock.GetComponentInChildren<DigitalDamageReceiver>();
            receiver.index = i;
            receiver.OnShutDown.AddListener(Shut);
        }
    }

    public void Shut(int index) {
        order.Add(index);
        if (order.Count == clocks.Count) {
            bool flag = true;
            for (int i = 1; i < order.Count; ++i) {
                if (order[i] < order[i - 1]) {
                    flag = false;
                    break;
                }
            }

            if (flag) {
                // TODO:
                Destroy(gameObject);
            }
            else {
                order.Clear();
                foreach (GameObject clock in clocks) {
                    var agent = clock.GetComponent<DigitalClockAgent>();
                    agent.Reset();

                    var receiver = clock.GetComponentInChildren<DigitalDamageReceiver>();
                    receiver.HP = MAX_HP;
                }
            }
        }
    }
}
