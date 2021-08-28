using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private Text question1;
    [SerializeField]
    private Text question2;
    [SerializeField]
    private Text question3;
    [SerializeField]
    private Text question4;

    [SerializeField]
    private Button buttonA;
    [SerializeField]
    private Button buttonB;
    [SerializeField]
    private Button buttonC;
    [SerializeField]
    private Button buttonD;

    [SerializeField]
    private Slider sliderEnemyHp;

    [SerializeField]
    private Slider playerSliderHp;

    [SerializeField]
    private Button buttonAttackPlayer;


    private void Awake()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnQuestionUpdate, OnQuestionUpdate);
        EventHandling.EventUtils.AddListener(EventConstants.OnEnemyHealthPointUpdate, OnEnemyHealthPointUpdate);
        EventHandling.EventUtils.AddListener(EventConstants.OnPlayerHealthPointUpdate, OnPlayerHealthPointUpdate);
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonA.onClick.AddListener(() =>
        {
            Debug.Log("A Clicked");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnButtonAnswerClicked, "A");
        });
        buttonB.onClick.AddListener(() =>
        {
            Debug.Log("B Clicked");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnButtonAnswerClicked, "B");
        });
        buttonC.onClick.AddListener(() =>
        {
            Debug.Log("C Clicked");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnButtonAnswerClicked, "C");
        });
        buttonD.onClick.AddListener(() =>
        {
            Debug.Log("D Clicked");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnButtonAnswerClicked, "D");
        });
        buttonAttackPlayer.onClick.AddListener(() =>
        {
            Debug.Log("buttonAttackPlayer Clicked");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnEnemyCorrect, 20);
        });
    }

    private void OnQuestionUpdate(EventHandling.EventArgs eventArgs)
    {
        if (eventArgs.args != null && eventArgs.args.Length > 0)
        {
            var question = eventArgs.args[0] as QuestionModel;
            var index = Convert.ToInt32(eventArgs.args[1]);
            Debug.Log("QuestionUpdate question: " + question.Name + " | " + question.TextColor);
            switch (index)
            {
                case 0:
                    question1.text = question.Name;
                    question1.color = question.TextColor;
                    break;
                case 1:
                    question2.text = question.Name;
                    question2.color = question.TextColor;
                    break;
                case 2:
                    question3.text = question.Name;
                    question3.color = question.TextColor;
                    break;
                case 3:
                    question4.text = question.Name;
                    question4.color = question.TextColor;
                    break;
            }
        }
        else
        {
            question1.color = Color.black;
            question2.color = Color.black;
            question3.color = Color.black;
            question4.color = Color.black;
        }

    }

    private void OnEnemyHealthPointUpdate(EventHandling.EventArgs eventArgs)
    {
        var enemyHp = Convert.ToInt32(eventArgs.args[0]);
        if (enemyHp >= 0)
        {
            sliderEnemyHp.value = (float)enemyHp / 100;
        }
        Debug.Log("OnEnemyHealthPointUpdate enemyHP: " + enemyHp + " | sliderEnemyHp: " + sliderEnemyHp.value);
    }

    private void OnPlayerHealthPointUpdate(EventHandling.EventArgs eventArgs)
    {
        var playerHp = Convert.ToInt32(eventArgs.args[0]);
        if (playerHp >= 0)
        {
            playerSliderHp.value = (float)playerHp / 100;
        }
        Debug.Log("OnPlayerHealthPointUpdate playerHp: " + playerHp + " | playerSliderHp: " + playerSliderHp.value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnQuestionUpdate, OnQuestionUpdate);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnEnemyHealthPointUpdate, OnEnemyHealthPointUpdate);
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();
    }
}
