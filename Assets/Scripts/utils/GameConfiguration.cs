using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    menuName = GameConstants.CreateAssetMenu_GameConfiguration)]
public class GameConfiguration : ScriptableObject
{
    //  Properties ------------------------------------
    public string SceneNameMainMenu { get { return _sceneNameMainMenu; } }
    public string SceneNameGameplay { get { return _sceneNameGameplay; } }

    // Gameplay - Local Player
    //public int StartingGold { get { return _startingGold; } }
    //public int MaxGoldCount { get { return _maxGoldCount; } }
    //public float GoldPerSecond { get { return _goldPerSecond; } }

    // Audio
    //public bool IsAudioEnabled { get { return _isAudioEnabled; } }
    //public float AudioVolume { get { return _audioVolume; } }

    //  Fields ----------------------------------------
    [Header("Scenes")]
    [SerializeField] private string _sceneNameMainMenu = "MainMenu";
    [SerializeField] private string _sceneNameGameplay = "Gameplay";

    //[Header("Gameplay - Local Player")]
    /// <summary>
    /// Starting gold count.
    /// </summary>
    //[Range(1, 3)]
    //[SerializeField] private int _startingGold = 3;

    /// <summary>
    /// Maximum gold a user can have at a time.
    /// </summary>
    //[Range(3, 10)]
    //[SerializeField] private int _maxGoldCount = 10;

    /// <summary>
    /// Gold income per second.
    /// </summary>
    //[Range(0.1f, 2f)]
    //[SerializeField] private float _goldPerSecond = 0.5f;

    //[Header("Audio")]
    //[SerializeField] private bool _isAudioEnabled = true;

    //[Range(0, 1f)]
    //[SerializeField] float _audioVolume = 1;
}