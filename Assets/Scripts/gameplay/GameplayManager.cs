using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ListOfQuestion;
    private int currentQuestionPointerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnButtonAnswerClicked, OnButtonAnswerClicked);
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
