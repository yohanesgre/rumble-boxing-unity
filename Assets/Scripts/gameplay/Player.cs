using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private string playerName;
    [SerializeField]
    private float healthPoint;

    public string Id { get => id; set => id = value; }
    public string Name { get => playerName; set => playerName = value; }
    public float HealthPoint { get => healthPoint; set => healthPoint = value; }



    private void Awake()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnEnemyCorrect, DamagePlayer);
    }

    private void DamagePlayer(EventHandling.EventArgs eventArgs)
    {
        var damage = (float)eventArgs.args[0];
        healthPoint -= damage;
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnPlayerHealthPointUpdate, healthPoint);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnEnemyCorrect, DamagePlayer);
    }
}
