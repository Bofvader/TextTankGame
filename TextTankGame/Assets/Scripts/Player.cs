using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank {

    [SerializeField] [Range(0.0f, 1000.0f)] float m_projectileSpeed = 5.0f;
    [SerializeField] [Range(0.0f, 90.0f)] float m_angle = 30.0f;
    float m_gravity = 9.8f;

    //public bool Fire()
    //{
    //    bool hitSomething = false;

    //    float time = (2 * m_projectileSpeed * Mathf.Sin(m_angle)) / m_gravity;
    //    float xDistance = (m_projectileSpeed * Mathf.Sin(m_angle)) * time;

    //    float distance = (Game.Instance.transform.position.x - transform.position.x);
        
    //    if(transform.position.x + distance >= Game.Instance.transform.position.x - (Game.Instance.tankInfo.tankSize / 2) && 
    //        transform.position.x + distance <= Game.Instance.transform.position.x + (Game.Instance.tankInfo.tankSize / 2))
    //    {
    //        Game.Instance.ai.GetComponent<Tank>().hitPoints -= m_tank.damage;
    //        hitSomething = true;
    //    }
    //    return hitSomething;
    //}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
