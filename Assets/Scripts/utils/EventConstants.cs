using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventConstants
{
    #region EVENT_UI
    public const string OnButtonAnswerClicked = "ON_BUTTON_ANSWER_CLICKED";
    public const string OnEnemyHealthPointUpdate = "ON_ENEMY_HEALTH_POINT_UPDATE";
    public const string OnPlayerHealthPointUpdate = "ON_Player_HEALTH_POINT_UPDATE";
    #endregion

    #region EVENT_GAMEPLAY
    public const string OnQuestionInit = "ON_QUESTION_INIT";
    public const string OnQuestionUpdate = "ON_QUESTION_UPDATE";
    #endregion

    #region EVENT_QUESTION
    public const string OnAnswerCorrect = "ON_ANSWER_CORRECT";
    public const string OnAnswerFalse = "ON_ANSWER_FALSE";
    #endregion

    #region EVENT_ENEMY
    public const string OnEnemyCorrect = "ON_ENEMY_CORRECT";
    #endregion

    #region EVENT_PLAYER
    public const string OnPlayerCorrect = "ON_PLAYER_CORRECT";
    #endregion
}
