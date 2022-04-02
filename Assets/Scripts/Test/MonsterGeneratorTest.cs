using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneratorTest : MonoBehaviour {
    void Start() {
        GameObject clock = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.CLOCK);
        GameObject digitalClock = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.DIGITAL_CLOCK);
        GameObject trumpet = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.TRUMPET);
        GameObject drill = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.ELECTRIC_DRILL);
        GameObject pedulumClock = Monstergenerator.Instance.GenerateMonster(Monstergenerator.MonsterType.PENDULUM_CLOCK);
    }
}
