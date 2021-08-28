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
    private int healthPoint;

    public string Id { get => id; set => id = value; }
    public string Name { get => playerName; set => playerName = value; }
    public int HealthPoint { get => healthPoint; set => healthPoint = value; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
