using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Toast;
    public Text Timer;
    public RawImage Reticle;
    public float TimeRemaining;

    private float toastTimer;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if ((toastTimer -= Time.deltaTime) <= 0) Toast.text = "";

        if ((TimeRemaining -= Time.deltaTime) <= 0) Timer.text = "";
        else
        {
            int min = Mathf.FloorToInt(TimeRemaining / 60);
            int sec = Mathf.FloorToInt(TimeRemaining % 60);
            Timer.text = min + ":" + (sec < 10 ? "0" : "") + sec + "s";
        }
    }

    public void DisplayToast(string message, float time = 2)
    {
        Toast.text = message;
        toastTimer = time;
    }

    public void DisplayMessage(string message)
    {
        DisplayToast(message, Mathf.Infinity);
    }
}
