using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour {
    [SerializeField]
    private Transform _Player;
    [SerializeField]
    private float _Health;
    [SerializeField]
    private float _Speed;
    private float _HealthModifier =1;
    private float _SpeedModifier =1;
    private NavMeshAgent _Agent;
    
	// Use this for initialization
	void Start () {
        _Health = 100f;
        _Speed = 5f;
        _Health *= _HealthModifier;
        _Speed *= _SpeedModifier;
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.speed = _Speed;
        
	}
	
	// Update is called once per frame
	void Update () {
        _Agent.destination = _Player.position;
        
        
        
	}

    public void Modifie(float modifier)
    {
        _HealthModifier = modifier;
        _SpeedModifier = modifier;
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            _Agent.isStopped = true;
        }
    }
}
