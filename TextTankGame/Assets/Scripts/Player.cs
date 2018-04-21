using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] [Range(0.0f, 1000.0f)] float m_projectileSpeed = 5.0f;
    [SerializeField] [Range(0.0f, 90.0f)] float m_angle = 30.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
