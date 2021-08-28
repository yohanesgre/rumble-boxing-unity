using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventConstants
{
    #region EVENT_UI
    public const string OnButtonAnswerClicked = "ON_BUTTON_ANSWER_CLICKED";
    #endregion

    #region EVENT_GAMEPLAY
    public const string OnQuestionInit = "ON_QUESTION_INIT";
    public const string OnQuestionUpdate = "ON_QUESTION_UPDATE";
    #endregion

    #region EVENT_QUESTION
    public const string OnAnswerCorrect = "ON_ANSWER_CORRECT";
    public const string OnAnswerFalse = "ON_ANSWER_FALSE";
    #endregion
}
