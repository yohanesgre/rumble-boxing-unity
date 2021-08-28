using System.Collections;
using UnityEngine;


public class GameConfigurationManager : Singleton<GameConfigurationManager>
{
    //  Properties ------------------------------------
    public GameConfiguration GameConfiguration { get { return gameConfiguration; } }

    //  Fields ----------------------------------------
    [SerializeField] private GameConfiguration gameConfiguration = null;
}
