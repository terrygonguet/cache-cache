using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State { MessageHider, RoundHider, MessageSearcher, RoundSearcher, EndRound }

public class Manager : MonoBehaviour
{
    public GameObject Cacheur;
    public GameObject Chercheur;
    public Text Text;
    public int NbHiders = 1;

    private State state;
    private int currentHider;
    private float timeRemaining;

    void Start()
    {
        currentHider = 1;
        timeRemaining = 0;
        state = State.MessageHider;
        Text.text = "Tour du cacheur n°" + currentHider;
        Cacheur.transform.position = new Vector3();
        Chercheur.transform.position = new Vector3();
        Cacheur.SetActive(false);
        Chercheur.SetActive(false);
    }

    void Update()
    {
        bool gotoNext = false;
        if (state == State.MessageHider || state == State.MessageSearcher)
        {
            gotoNext = Input.GetButton("Jump");
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            gotoNext = timeRemaining <= 0;
        }

        if (gotoNext)
        {
            switch (state)
            {
                case State.MessageHider:
                    // spawn & start timer
                    Text.text = "30 secondes pour te cacher!";
                    timeRemaining = 30;
                    state = State.RoundHider;
                    Cacheur.SetActive(true);
                    break;
                case State.RoundHider:
                    // next hider or searcher round
                    if (++currentHider > NbHiders)
                    {
                        Text.text = "Tour du chercheur";
                        Hider hider = Cacheur.GetComponent<Hider>();
                        hider.enabled = false;
                        state = State.MessageSearcher;
                    }
                    else
                    {
                        // TODO
                    }
                    break;
                case State.MessageSearcher:
                    // spawn & start timer
                    Text.text = "1min pour trouver le cacheur!";
                    Chercheur.SetActive(true);
                    timeRemaining = 60;
                    state = State.RoundSearcher;
                    break;
                case State.RoundSearcher:
                    // end round
                    Text.text = "Game Over";
                    Searcher searcher = Chercheur.GetComponent<Searcher>();
                    searcher.enabled = false;
                    timeRemaining = 10;
                    state = State.EndRound;
                    break;
                case State.EndRound:
                    // reset
                    Start();
                    break;
            }
        }
    }
}
