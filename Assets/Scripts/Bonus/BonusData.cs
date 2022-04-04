using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bonus", menuName = "ScriptableObjects/Bonus Data", order = 1)]
public class BonusData : ScriptableObject {
    public Sprite sprite;
    public string text;
    public string prop;
    public int delta;
}
