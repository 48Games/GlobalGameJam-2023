using System;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public float transitionTime;
    public float fullTime;

    RectTransform square;
    State status = State.NONE;
    private float currentTime = 0;
    private float currentFullTime = 0;
    private Action callback;

    void Start()
    {
        square = GetComponent<RectTransform>();
        ResetTransition();
    }

    void Update()
    {
        switch(status)
        {
            case State.BEGIN:
                ResetTransition();
                ResetValues();
                status = State.RUNNING_LEFT;
                break;
            case State.RUNNING_LEFT:
                currentTime += Time.deltaTime;
                if(currentTime >= transitionTime)
                {
                    currentTime = transitionTime;
                    status = State.FULL;
                    callback();
                }
                square.sizeDelta = new Vector2((GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.width * 2) * (currentTime/transitionTime)
                    , GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.height);
                break;
            case State.FULL:
                square.anchoredPosition = new Vector2(-GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.width / 2, 0);
                currentFullTime += Time.deltaTime;
                if(currentFullTime >= fullTime)
                {
                    status = State.RUNNING_RIGHT;
                    currentFullTime = 0;
                }
                break;
            case State.RUNNING_RIGHT:
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    currentTime = 0;
                    status = State.NONE;
                }
                square.sizeDelta = new Vector2((GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.width * 2) * (currentTime / transitionTime)
                    , GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.height);
                break;
        }
    }

    private void ResetValues()
    {
        currentTime = 0;
    }

    private void ResetTransition()
    {
        square.anchoredPosition = new Vector2(GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.width / 2, 0);
        square.sizeDelta = new Vector2(0, GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.height);
    }

    public void DoTransition(Action Callback)
    {
        if(status == State.NONE)
        {
            callback = Callback;
            status = State.BEGIN;
        }   
    }
}

enum State
{
    NONE,
    BEGIN,
    RUNNING_LEFT,
    FULL,
    RUNNING_RIGHT
}
