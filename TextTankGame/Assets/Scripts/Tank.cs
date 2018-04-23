using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
	[SerializeField] Vector3 m_spawnPoint;
	[SerializeField] protected float m_maxSpeedBase = 5.0f;
	[SerializeField] protected float m_hitPoints = 100;
	[SerializeField] protected float m_projectileSpeed = 40;
	[SerializeField] protected float m_damage = 40;
	[SerializeField] float m_shotTime = 1.0f;
	[SerializeField] float m_tankSize = 1.0f;
	[SerializeField] float m_baseMargin = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] protected float m_screenShake = .5f;

	protected float m_speed = 0.0f;
	protected float m_maxSpeed = 0.0f;
	protected float m_tiltAngle = 0.0f;
	protected float m_turnAngle = 0.0f;
    protected Vector3 m_velocity = Vector3.zero;

	float m_shotTimer = 0.0f;
	bool m_isAlive = false;

	float m_size = 0.0f;
	float m_errorMargin = 0.0f;

	public bool Alive { get { return m_isAlive; } }
	public bool ShotReady { get { return m_shotTimer >= m_shotTime; } }
	public float Size { get { return m_size; } }

    void FixedUpdate()
	{
		m_size = m_tankSize * Game.Instance.Scale;
		m_errorMargin = m_baseMargin * Game.Instance.Scale;
		m_maxSpeed = m_maxSpeedBase * Game.Instance.Scale;

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
				Vector3 point = go.transform.position - transform.position;
				float difference = point.magnitude;

				if (difference == distance)
				{
					Vector3 north = Vector3.forward * difference;
					north = Quaternion.Euler(0.0f, m_turnAngle, 0.0f) * north;

					float offset = (point - north).magnitude;
					float test = m_errorMargin;
					Tank t = go.GetComponent<Tank>();
					if(t)
					{
						test += t.Size;
					}

					if(offset <= test)
					{
						hit = go;
						if (offset <= test - m_errorMargin && t) t.Hit(m_damage);
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
