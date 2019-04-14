using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State { PreSouris, RoundSouris, PreChat, RoundChat, Fin }

public class Manager : MonoBehaviour
{
    public GameObject PrefabChat;
    public GameObject PrefabSouris;
    public UIManager UIManager;
    public Camera DefaultCamera;

    public int NbSouris = 1;
    public int HideTime = 30;
    public int BaseSearchTime = 60;
    public int BonusSearchTime = 30;

    private State state = State.PreSouris;
    private int currentSouris = 0;
    private float timeRemaining = 0;
    private List<GameObject> souris = new List<GameObject>();
    private GameObject chat;

    void Start()
    {
        currentSouris = 0;
        UIManager.TimeRemaining = timeRemaining = 0;
        state = State.PreSouris;

        foreach(GameObject obj in souris)
            Destroy(obj);
        if (chat) Destroy(chat);

        souris.Clear();
        chat = null;

        UIManager.DisplayMessage("Tour de la Souris n°" + (currentSouris + 1) + " !");
        DefaultCamera.enabled = true;
        UIManager.Reticle.enabled = false;
    }

    void Update()
    {
        bool next = false;
        if (state == State.PreChat || state == State.PreSouris)
        {
            next = Input.GetButton("Jump");
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            next = timeRemaining <= 0;
        }

        if (next)
        {
            switch (state)
            {
                case State.PreSouris:
                    DefaultCamera.enabled = false;
                    souris.Add(Instantiate(PrefabSouris));
                    souris[currentSouris].transform.position = Vector3.up * 1.2f;
                    UIManager.TimeRemaining = timeRemaining = HideTime;
                    UIManager.DisplayToast(HideTime + " secondes pour te cacher !");
                    state = State.RoundSouris;
                    break;

                case State.RoundSouris:
                    Souris compoSouris = souris[currentSouris].GetComponent<Souris>();
                    compoSouris.enabled = false;
                    DefaultCamera.enabled = true;

                    if (++currentSouris >= NbSouris)
                    {
                        UIManager.DisplayMessage("Tour du Chat !");
                        state = State.PreChat;
                    }
                    else
                    {
                        UIManager.DisplayMessage("Tour de la Souris n°" + (currentSouris + 1) + " !");
                        state = State.PreSouris;
                    }
                    break;

                case State.PreChat:
                    DefaultCamera.enabled = false;
                    chat = Instantiate(PrefabChat);
                    chat.transform.position = Vector3.up * 1.2f;
                    UIManager.TimeRemaining = timeRemaining = BaseSearchTime + (NbSouris - 1) * BonusSearchTime;
                    UIManager.DisplayToast(timeRemaining + " secondes pour trouver " + (NbSouris > 1 ? "les" : "la") + " Souris !");
                    UIManager.Reticle.enabled = true;
                    state = State.RoundChat;
                    break;

                case State.RoundChat:
                    Chat compoChat = chat.GetComponent<Chat>();
                    compoChat.enabled = false;
                    UIManager.Reticle.enabled = false;
                    timeRemaining = 5;
                    UIManager.DisplayMessage("Game Over !");
                    state = State.Fin;
                    break;

                case State.Fin:
                    Start();
                    break;
            }
        }
    }
}
