using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] Vector3 m_waypoint;
	[SerializeField] float m_baseSpeed = 1.0f;
	[SerializeField] float m_damage = 1.0f;
	[SerializeField] float m_health = 1.0f;
	[SerializeField] float m_shotTime = 1.0f;
	[SerializeField] float m_length = 1.0f;

	bool isAlive = false;
	bool m_isFiring = false;
	float m_speed = 0.0f;
	float m_shotTimer = 0.0f;

	public float Length { get { return m_length; } }

	private void Start()
	{
		m_speed = m_baseSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		if(isAlive)
		{
			Vector3 direction = m_waypoint - transform.position;
			Vector3 velocity = direction.normalized * m_speed * Time.deltaTime;

			if(m_isFiring)
			{
				m_shotTimer += Time.deltaTime;
			}
		}
	}

	public void Fire()
	{
		if(m_target)
		{
			m_speed = 0.0f;
			m_isFiring = true;
			Vector3 direction = m_target.transform.position - transform.position;

			if(m_shotTimer >= m_shotTime)
			{
				//Bang
			}
		}
	}
}
