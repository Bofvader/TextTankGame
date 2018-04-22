using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Tank
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] float m_firingRange = 1.0f;
	[SerializeField] float m_maxTrackDistance = 10.0f;
	[SerializeField] float m_minTrackDistance = 3.0f;

	bool m_isFiring = false;
	bool m_wasHit = false;

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
		}
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
