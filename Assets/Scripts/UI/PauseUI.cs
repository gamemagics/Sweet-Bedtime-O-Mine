using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour {
    [SerializeField] private GameObject BB;
    public void Pause() {
        Time.timeScale = 0;
        BB.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1;
        BB.SetActive(false);
    }
}
