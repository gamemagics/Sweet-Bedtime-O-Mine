using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    private Canvas canvas;
    private Image blackScreen;
    [SerializeField] private Transform player;
    private Vector2 playerCanvasPos;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        blackScreen = GetComponentInChildren<Image>();
    }
    void Start()
    {
        DrawBlackScreen();
        blackScreen.material.SetFloat("_Radius", 1);
    }

    // Update is called once per frame
    void Update()
    {
        // if (blackScreen.material.GetFloat("_Radius") == 1)
        // {
        // }
    }
    private void DrawBlackScreen()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
        var playerScreenPos = Camera.main.WorldToScreenPoint(player.position);

        var canvasRect = canvas.GetComponent<RectTransform>();
        var canvasWidth = canvasRect.rect.width;
        var canvasHeight = canvasRect.rect.height;

        playerCanvasPos = new Vector2
        {
            x = playerScreenPos.x / screenWidth * canvasWidth,
            y = playerScreenPos.y / screenHeight * canvasHeight
        };


        var squareValue = 0f;
        if (canvasWidth > canvasHeight)
        {
            squareValue = canvasWidth;
            playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
        }
        else
        {
            squareValue = canvasHeight;
            playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
        }

        playerCanvasPos /= squareValue;
        var mat = blackScreen.material;

        mat.SetFloat("_CenterX", playerCanvasPos.x);
        mat.SetFloat("_CenterY", playerCanvasPos.y);

        blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius)
    {
        blackScreen.gameObject.SetActive(true);
        var time = 0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            blackScreen.material.SetFloat("_Radius", radius);

            yield return null;
        }
        // yield return new WaitForSeconds(duration);
    }
    public void OpenBlackScreen()
    {
        StartCoroutine(Transition(1f, 0f, 1f));
        Invoke("DisableBlackScreen", 1f);
    }
    public void CloseBlackScreen()
    {
        StartCoroutine(Transition(1f, 1f, 0f));
        Invoke("DisableBlackScreen", 1f);
    }
    private void DisableBlackScreen()
    {
        blackScreen.gameObject.SetActive(false);
    }
}
