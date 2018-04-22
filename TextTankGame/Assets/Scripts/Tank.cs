using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
	[SerializeField] protected float m_maxSpeed = 5.0f;
	[SerializeField] protected float m_shotTime = 1.0f;

	protected float m_speed = 0.0f;
	float m_shotTimer = 0.0f;

    public float m_tankSize = 1.0f;
    public float m_hitPoints = 100;
    public float m_damage = 40;
	public bool m_isAlive = false;

	public bool ShotReady { get { return m_shotTimer >= m_shotTime; } }

	void FixedUpdate()
    {
		if (m_isAlive)
		{
			if (m_shotTimer < m_shotTime)
			{
				m_shotTimer += Time.deltaTime;
			}

			if(m_hitPoints <= 0.0f)
			{
				Died();
			}
		}
	}

	protected virtual void Died()
	{
		m_isAlive = false;
		//Play death noise
	}

	public virtual bool Fire()
	{

		return false;
	}

	public virtual void Hit(float damage)
	{
		m_hitPoints -= damage;
	}

	public virtual void Collision()
	{
		m_speed = 0.0f;
	}

	public bool Colliding(GameObject other)
	{
		float rawDistance = (other.transform.position - transform.position).magnitude * Game.Instance.Scale;

		float walls = m_tankSize;
		Tank t = other.GetComponent<Tank>();
		if(t)
		{
			walls += t.m_tankSize;
		}

		return walls * Game.Instance.Scale >= rawDistance;
	}
}
