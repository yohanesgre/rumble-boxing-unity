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
    public const string OnQuestionsInit = "ON_QUESTION_INIT";
    public const string OnQuestionUpdate = "ON_QUESTION_UPDATE";
    public const string OnGameInitialized = "ON_GAME_INITIALIZED";
    public const string OnGameRoundUpdate = "ON_GAME_ROUND_UPDATE";
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
    public const string OnPlayerLocalReady = "ON_PLAYER_LOCAL_READY";
    #endregion
}
