using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
	[SerializeField] Vector3 m_spawnPoint;
	[SerializeField] protected float m_maxSpeed = 5.0f;
	[SerializeField] float m_shotTime = 1.0f;
	[SerializeField] float m_tankSize = 1.0f;
	[SerializeField] float m_hitPoints = 100;
	[SerializeField] float m_damage = 40;
	[SerializeField] float m_projectileSpeed = 40;

	protected float m_speed = 0.0f;
	protected float m_tiltAngle = 0.0f;
	protected float m_turnAngle = 0.0f;

	float m_shotTimer = 0.0f;
	bool m_isAlive = false;

	public bool Alive { get { return m_isAlive; } }
	public bool ShotReady { get { return m_shotTimer >= m_shotTime; } }

	void FixedUpdate()
	{
		if (m_isAlive)
		{
			if (m_shotTimer < m_shotTime)
			{
				m_shotTimer += Time.deltaTime;
			}

			if (m_hitPoints <= 0.0f)
			{
				Died();
			}
		}
	}

	public virtual void Spawn(float level)
	{
		m_isAlive = true;
		transform.position = m_spawnPoint;
	}

	public virtual void Spawn()
	{
		m_isAlive = true;
		transform.position = m_spawnPoint;
	}

	public virtual void Died()
	{
		m_isAlive = false;
		//Play death noise
	}

	public virtual GameObject Fire()
	{
		GameObject hit = null;

		if (ShotReady)
		{
			float distance = ((m_projectileSpeed * m_projectileSpeed) * Mathf.Sin(2 * m_tiltAngle)) / Game.Instance.Gravity;
			GameObject[] gos = Game.Instance.GetObjectsInRange(this.gameObject, distance, "Tank");
			
			foreach(GameObject go in gos)
			{
				Vector3 difference = go.transform.position - transform.position;
				float smallDistance = difference.magnitude;

				if (smallDistance == distance)
				{
					Vector3 north = Vector3.forward * smallDistance;
					Vector3 offset = north - difference;
					float test = offset.magnitude;

					if (test <= m_tankSize)
					{
						hit = go;
						break;
					}
				}
			}
			  
		}

		return hit;
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
		if (t)
		{
			walls += t.m_tankSize;
		}

		return walls * Game.Instance.Scale >= rawDistance;
	}
}
