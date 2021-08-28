using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionModel
{
    [SerializeField]
    private string questionName;
    [SerializeField]
    private Color textColor;

    public QuestionModel(
        string name,
        Color textColor)
    {
        this.questionName = name;
        this.textColor = textColor;
    }

    public QuestionModel(
       string name)
    {
        this.questionName = name;
        this.textColor = Color.black;
    }

    public QuestionModel()
    {
        this.questionName = "A";
        this.textColor = Color.black;
    }

    public string Name { get => questionName; set => questionName = value; }
    public Color TextColor { get => textColor; set => textColor = value; }
}
