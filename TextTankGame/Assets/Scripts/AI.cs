using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Tank
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] float m_firingRange = 1.0f;
	[SerializeField] float m_updateTime = 3.0f;
	[SerializeField] float m_updateRatio = 3.0f;

	bool m_isFiring = false;
	bool m_wasHit = false;
	float m_updateTimer = 0.0f;
	float m_updateAngle = 0.0f;
	float m_range = 0.0f;
	Vector3 m_path = Vector3.forward;

	protected void Update()
	{
		m_range = m_firingRange * Game.Instance.Scale;

		if (Alive)
		{
			if (m_speed < m_maxSpeed)
			{
				++m_speed;
			}

			if(InRangeOfTarget())
			{
				m_isFiring = true;
				Fire();
			}
			else
			{
				float distanceFrom = (m_target.transform.position - transform.position).magnitude;

				Vector3 velocity = Vector3.zero;
				velocity = m_path * m_speed * Time.deltaTime;
				Debug.Log(velocity.x + ", " + velocity.y + ", " + velocity.z);
				transform.position = transform.position + velocity;

				float distanceNow = (m_target.transform.position - transform.position).magnitude;

				if (m_updateTimer >= m_updateTime)
				{
					if (distanceNow > distanceFrom)
					{
						m_path = Meander();
						m_updateTimer = 0.0f;
					}
				}
				else
				{
					m_updateTimer += Time.deltaTime;
				}
			}
		}
	}

	public override void Spawn()
	{
		base.Spawn();
		m_updateTimer = m_updateTime;
	}

	Vector3 Meander()
	{

		Vector3 path = Quaternion.AngleAxis(m_updateAngle, Vector3.up) * m_path;
		m_updateAngle += m_updateRatio;

		return path;
	}

	bool InRangeOfTarget()
	{
		Vector3 offset = m_target.transform.position - transform.position;
		float distance = offset.magnitude;

		return m_firingRange >= distance;
	}

	public override GameObject Fire()
	{
		GameObject go = null;

		Vector3 offset = m_target.transform.position - transform.position;
		float distance = offset.magnitude;

		if(ShotReady)
		{
			StartCoroutine(TravelTime(distance, offset));

			if(m_wasHit)
			{
				m_wasHit = false;
				go = m_target;

				Tank t = m_target.GetComponent<Tank>();
				if(t)
				{
					t.Hit(m_damage);
				}
			}
		}

		return go;
	}

	IEnumerator TravelTime(float distance, Vector3 targetPosition)
	{
		float time = distance * m_projectileSpeed;

		yield return new WaitForSeconds(time);

		Vector3 actual = m_target.transform.position - transform.position;
		float difference = (actual - targetPosition).magnitude;

		Tank body = m_target.GetComponent<Tank>();
		if(body && difference <= body.Size)
		{
			m_wasHit = true;
		}
		else if(difference == 0)
		{
			m_wasHit = true;
		}
	}
}
