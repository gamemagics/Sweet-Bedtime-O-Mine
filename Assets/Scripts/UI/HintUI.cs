using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    void Start()
    {
        HideHint();
    }
    void Update()
    {

    }

    private void DoHideHint()
    {
        GetComponent<TMPro.TMP_Text>().text = "";
    }

    public void ShowHint(string text)
    {
        GetComponent<TMPro.TMP_Text>().text = text;
    }
    public void HideHint()
    {
        Invoke("DoHideHint", 5f);
    }
}
