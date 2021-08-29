using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : Singleton<QuestionManager>
{
    [SerializeField]
    private List<QuestionModel[]> listOfQuestionModel = new List<QuestionModel[]>();
    public List<QuestionModel[]> ListOfQuestionModel { get => listOfQuestionModel; set => listOfQuestionModel = value; }
    [SerializeField]
    private List<GameObject> listOfQuestionGameObject = new List<GameObject>();
    public List<GameObject> ListOfQuestionGameObject { get => listOfQuestionGameObject; set => listOfQuestionGameObject = value; }


    [SerializeField]
    private int currentQuestionPointerIndex = 0;
    [SerializeField]
    private int currentRound = 0;

    [Obsolete]
    protected override void Awake()
    {
        base.Awake();
        EventHandling.EventUtils.AddListener(EventConstants.OnQuestionsInit, OnQuestionsInit);
        EventHandling.EventUtils.AddListener(EventConstants.OnGameRoundUpdate, OnGameRoundUpdate);
        EventHandling.EventUtils.AddListener(EventConstants.OnButtonAnswerClicked, OnButtonAnswerClicked);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    [Obsolete]
    private void OnQuestionsInit(EventHandling.EventArgs eventArgs)
    {
        var rounds = Convert.ToInt32(eventArgs.args[0]);
        for (int i = 0; i < rounds; i++)
        {
            var setOfQuestion = new QuestionModel[4];
            for (int j = 0; j < 4; j++)
            {
                var question = new QuestionModel(ValueUtils.GetRandomQuestionChar().ToString());
                setOfQuestion[j] = question;
            }
            ListOfQuestionModel.Add(setOfQuestion);
        }
        SetQuestionView(0);
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnGameInitialized);
    }

    private void OnGameRoundUpdate(EventHandling.EventArgs eventArgs)
    {
        currentRound = Convert.ToInt32(eventArgs.args[0]);
        SetQuestionView(currentRound);
    }

    private void OnButtonAnswerClicked(EventHandling.EventArgs eventArgs)
    {
        if (currentQuestionPointerIndex < listOfQuestionModel[currentRound].Length)
        {
            var question = listOfQuestionModel[currentRound][currentQuestionPointerIndex];
            var answer = eventArgs.args[0] as string;
            Debug.Log("OnButtonAnswerClicked | Question: " + question.Name + " | Answer: " + answer);
            if (answer == question.Name)
            {
                Debug.Log("OnButtonAnswerClicked correct");
                EventHandling.EventUtils.DispatchEvent(EventConstants.OnAnswerCorrect, question, currentQuestionPointerIndex, currentRound);
                if (currentQuestionPointerIndex == 3)
                {
                    EventHandling.EventUtils.DispatchEvent(EventConstants.OnPlayerCorrect, 20);
                    EventHandling.EventUtils.DispatchEvent(EventConstants.OnGameRoundUpdate, currentRound + 1);
                }
                else
                {
                    currentQuestionPointerIndex++;
                }
            }
            else
            {
                Debug.Log("OnButtonAnswerClicked false");
                EventHandling.EventUtils.DispatchEvent(EventConstants.OnAnswerFalse);
                currentQuestionPointerIndex = 0;
            }
        }
    }

    private void SetQuestionView(int round)
    {
        currentQuestionPointerIndex = 0;
        Debug.LogFormat("SetQuestionView: listOfQuestionModel {0}", listOfQuestionModel[0].Length);
        for (int i = 0; i < listOfQuestionGameObject.Count; i++)
        {
            var _gameObject = listOfQuestionGameObject[i];
            var _question = _gameObject.GetComponent<Question>();
            var _questionModel = listOfQuestionModel[round][i];
            _question.QuestionModel = _questionModel;
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionUpdate, _question.QuestionModel, i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Obsolete]
    protected override void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnButtonAnswerClicked, OnButtonAnswerClicked);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnQuestionsInit, OnQuestionsInit);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnGameRoundUpdate, OnGameRoundUpdate);
        base.OnDestroy();
    }
}
