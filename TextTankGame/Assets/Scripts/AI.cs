using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Tank
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] Vector3 m_waypoint;
	[SerializeField] float m_firingRange = 1.0f;
	[SerializeField] int m_track = 0;

	bool m_isFiring = false;

	public int Track { get { return m_track; } }

	// Update is called once per frame
	void Update()
	{
		if (m_isAlive)
		{
			if (m_speed < m_maxSpeed) ++m_speed;

			Vector3 direction = m_waypoint - transform.position;
			Vector3 velocity = direction.normalized * m_speed * Time.deltaTime;

			if(InRangeOfTarget())
			{
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

	public override bool Fire()
	{
		Vector3 offset = m_target.transform.position - transform.position;
		float distance = offset.magnitude;



		return false;
	}

	//IEnumerator TravelTime(float distance)
	//{
	//	float time =

	//	yield new WaitForSeconds();

	//	return;
	//}
}
