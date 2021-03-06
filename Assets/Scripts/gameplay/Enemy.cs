using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private float healthPoint;

    public string Id { get => id; set => id = value; }
    public string Name { get => enemyName; set => enemyName = value; }
    public float HealthPoint { get => healthPoint; set => healthPoint = value; }

    private void Awake()
    {
        EventHandling.EventUtils.AddListener(EventConstants.OnPlayerCorrect, DamageEnemy);
    }

    private void DamageEnemy(EventHandling.EventArgs eventArgs)
    {
        var damage = (float)eventArgs.args[0];
        healthPoint -= damage;
        EventHandling.EventUtils.DispatchEvent(EventConstants.OnEnemyHealthPointUpdate, healthPoint);
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
        EventHandling.EventUtils.RemoveListener(EventConstants.OnPlayerCorrect, DamageEnemy);
    }
}
