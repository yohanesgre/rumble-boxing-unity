using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    [SerializeField]
    private QuestionModel questionModel;

    public QuestionModel QuestionModel { get => questionModel; set => questionModel = value; }

    public Question()
    {
        this.QuestionModel = new QuestionModel();
    }

    private void Awake()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnAnswerCorrect, OnAnswerCorrect);
        EventHandling.EventUtils.AddListener(EventConstants.OnAnswerFalse, OnAnswerFalse);
    }

    private void Start()
    {

    }

    private void OnAnswerCorrect(EventHandling.EventArgs eventArgs)
    {
        var questionModel = eventArgs.args[0] as QuestionModel;
        var index = Convert.ToInt32(eventArgs.args[1]);
        var currentRound = Convert.ToInt32(eventArgs.args[2]);
        //Debug.LogFormat("questionModel.Name: {0} | this.QuestionModel.Name: {1}", questionModel.Name, this.QuestionModel.Name);
        if (questionModel.Name == this.QuestionModel.Name)
        {
            this.QuestionModel.TextColor = Color.green;
            Debug.LogFormat("QuestionCorrectTextColor: {0}", this.QuestionModel.TextColor);
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionUpdate, this.QuestionModel, index);
        }
    }

    private void OnAnswerFalse(EventHandling.EventArgs eventArgs)
    {
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionUpdate);
    }

    private void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnAnswerCorrect, OnAnswerCorrect);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnAnswerFalse, OnAnswerFalse);
    }
}

