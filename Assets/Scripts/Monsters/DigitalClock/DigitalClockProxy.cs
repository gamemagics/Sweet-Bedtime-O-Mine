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

    public void InitByColor(DigitalClockColor color, Vector2[] positions) {
        // TODO:
    }
}
