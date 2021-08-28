using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    [SerializeField]
    private string questionName;
    [SerializeField]
    private float red, green, blue, alpha;
    [SerializeField]
    private Color textColor;

    public Question(
        string name,
        float red = 0,
        float green = 0,
        float blue = 0,
        float alpha = 1)
    {
        this.questionName = name;
        this.red = red;
        this.green = green;
        this.blue = blue;
        this.alpha = alpha;
    }

    public Question(
        string name,
        Color textColor)
    {
        this.questionName = name;
        this.TextColor = textColor;
    }

    public string Name { get => questionName; set => questionName = value; }
    public Color TextColor { get => textColor; set => textColor = value; }

    private void Start()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnAnswerCorrect, OnAnswerCorrect);
        EventHandling.EventUtils.AddListener(EventConstants.OnAnswerFalse, OnAnswerFalse);
    }

    private void OnAnswerCorrect(EventHandling.EventArgs eventArgs)
    {
        var question = eventArgs.args[0] as Question;
        if (question.Name == questionName)
        {
            TextColor = Color.green;
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionUpdate, this);
        }
    }

    private void OnAnswerFalse(EventHandling.EventArgs eventArgs)
    {
        var question = eventArgs.args[0] as Question;
        TextColor = Color.black;
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionUpdate, this);
    }

    private void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnAnswerCorrect, OnAnswerCorrect);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnAnswerFalse, OnAnswerFalse);
    }
}
