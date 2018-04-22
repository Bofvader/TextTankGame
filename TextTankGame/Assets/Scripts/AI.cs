using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Tank
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] float m_firingRange = 1.0f;
	[SerializeField] float m_updateTime = 3.0f;

	bool m_isFiring = false;
	bool m_wasHit = false;
	float m_updateTimer = 0.0f;
	Vector3 m_path = Vector3.zero;

	void Update()
	{
		if (Alive)
		{
			if (m_speed < m_maxSpeed) ++m_speed;

			if(InRangeOfTarget())
			{
				m_isFiring = true;
				Fire();
			}
			else
			{

				float distanceFrom = (m_target.transform.position - transform.position).magnitude;

				Debug.Log(distanceFrom + " " + m_path.x + " " + m_path.y + " " + m_path.z);

				Vector3 velocity = m_path;
				velocity = velocity.normalized * m_speed * Time.deltaTime;
				transform.position = transform.position + velocity;

				float distanceNow = (m_target.transform.position - transform.position).magnitude;

				if(distanceFrom < distanceNow && m_updateTimer >= m_updateTime)
				{
					m_path = Meander();
					m_updateTimer = 0.0f;
				}

				if(m_updateTimer < m_updateTime)
				{
					m_updateTimer += Time.deltaTime;
				}
			}
		}
	}

	Vector3 Meander()
	{
		Vector2 start = Random.insideUnitCircle;
		Vector3 path = Vector3.zero;
		path.x = start.x;
		path.y = Game.Instance.Level;
		path.z = start.y;

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
