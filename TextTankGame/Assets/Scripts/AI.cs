using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Tank
{
	[SerializeField] GameObject m_target = null;
	[SerializeField] float m_range = 1.0f;
	[SerializeField] float m_updateTime = 3.0f;
	[SerializeField] float m_updateRatio = 3.0f;
	[SerializeField] float m_waitTime = 4.0f;
	[SerializeField] float m_startTime = 2.0f;

	bool m_isFiring = false;
	bool m_wasHit = false;
	bool m_greenLight = true;
	float m_updateTimer = 0.0f;
	float m_updateAngle = 0.0f;
	float m_waitTimer = 0.0f;
	Vector3 m_path = Vector3.forward;

	protected void Update()
	{
		if (Alive)
		{
			if (m_waitTimer >= m_waitTime)
			{
				m_waitTimer = 0.0f;
				m_greenLight = false;
				Stop();
				StartCoroutine(Waiting());
			}
			else if(m_greenLight)
			{
				if (m_speed < m_maxSpeed)
				{
					++m_speed;
				}

				if (InRangeOfTarget())
				{
					Debug.Log("Within range");

					Fire();
					Stop();
				}
				else if (!m_isFiring)
				{
					m_waitTimer += Time.deltaTime;

					if (!m_isMoving)
					{
						StartedMoving();
					}

					float distanceFrom = (m_target.transform.position - transform.position).magnitude;

					Vector3 velocity = Vector3.zero;
					velocity = m_path * m_speed * Time.deltaTime;
					transform.position = transform.position + velocity;

					float distanceNow = (m_target.transform.position - transform.position).magnitude;

					if (m_updateTimer >= m_updateTime)
					{
						if (distanceNow > distanceFrom)
						{
							if (GetAngleFromTarget() < 180)
							{
								m_path = MeanderLeft();
							}
							else
							{
								m_path = MeanderRight();
							}
							m_updateTimer = 0.0f;
						}
					}
					else
					{
						m_updateTimer += Time.deltaTime;
					}
				}

				if (m_health <= 0.0f)
				{
					PlayDieSound();
				}
			}
		}
		else
		{
			Stop();
		}
	}

	IEnumerator Waiting()
	{
		yield return new WaitForSeconds(m_startTime);
		m_greenLight = true;
	}

	public override void Spawn()
	{
		base.Spawn();
		m_updateTimer = m_updateTime;
	}

	Vector3 MeanderLeft()
	{
		Vector3 path = Quaternion.AngleAxis(m_updateAngle, Vector3.up) * m_path;
		m_updateAngle += m_updateRatio;

		return path;
	}

	Vector3 MeanderRight()
	{
		Vector3 path = Quaternion.AngleAxis(m_updateAngle, Vector3.up) * m_path;
		m_updateAngle -= m_updateRatio;

		return path;
	}

	float GetAngleFromTarget()
	{
		Vector3 track = m_target.transform.position - transform.position;
		float angle = Quaternion.Angle(Quaternion.LookRotation(track), Quaternion.LookRotation(m_path));

		return angle;
	}

	bool InRangeOfTarget()
	{
		Vector3 offset = m_target.transform.position - transform.position;
		float distance = offset.magnitude;

		return m_range >= distance;
	}

	public override GameObject Fire()
	{
		GameObject go = null;

		Vector3 offset = m_target.transform.position - transform.position;
		float distance = offset.magnitude;

		if(ShotReady)
		{
			if (!m_isFiring)
			{
				m_shotTimer = 0.0f;

				Debug.Log("Is firing");
				m_isFiring = true;
				PlayGunSound();
				StartCoroutine(TravelTime(distance, offset));

				if (m_wasHit)
				{
					Debug.Log("Target was hit");

					m_wasHit = false;
					go = m_target;

					Tank t = m_target.GetComponent<Tank>();
					if (t)
					{
						t.Hit(m_damage);
					}
				}
			}
		}

		return go;
	}

	IEnumerator TravelTime(float distance, Vector3 targetPosition)
	{
		float time = distance / m_projectileSpeed;

		Debug.Log("Start travel");

		yield return new WaitForSeconds(time);

		Debug.Log("End travel");

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

		m_isFiring = false;
	}
}
