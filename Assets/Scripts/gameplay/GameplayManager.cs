using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ListOfQuestion;
    private int currentQuestionPointerIndex = 0;

    private void Awake()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnButtonAnswerClicked, OnButtonAnswerClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GamePlayManager Dispatch Question Init | " + ListOfQuestion.Length);
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionInit, ListOfQuestion);
    }

    private void OnButtonAnswerClicked(EventHandling.EventArgs eventArgs)
    {
        if (currentQuestionPointerIndex < ListOfQuestion.Length)
        {
            var question = ListOfQuestion[currentQuestionPointerIndex].GetComponent<Question>();
            var arg = eventArgs.args[0] as string;
            Debug.Log("OnButtonAnswerClicked Question: " + question.Name + "| arg " + arg);
            if (arg == question.Name)
            {

                EventHandling.EventUtils.DispatchEvent(EventConstants.OnAnswerCorrect, question);
                currentQuestionPointerIndex++;

            }
            else
            {
                EventHandling.EventUtils.DispatchEvent(EventConstants.OnAnswerFalse, question);
                currentQuestionPointerIndex = 0;
            }
        }
        else
        {
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnPlayerCorrect, 20);
            //currentQuestionPointerIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnButtonAnswerClicked, OnButtonAnswerClicked);
    }
}
