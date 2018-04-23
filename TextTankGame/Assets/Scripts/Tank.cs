using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
	[SerializeField] AudioSource m_gun = null;
	[SerializeField] AudioClip m_moveSound = null;
	[SerializeField] AudioClip m_deadSound = null;
	[SerializeField] Vector3 m_spawnPoint;
	[SerializeField] protected float m_maxSpeed = 5.0f;
	[SerializeField] protected float m_projectileSpeed = 40;
	[SerializeField] protected float m_damage = 40;
	[SerializeField] protected float m_health = 100;
	[SerializeField] float m_shotTime = 1.0f;
	[SerializeField] float m_size = 1.0f;
	[SerializeField] float m_errorMargin = 5.0f;
	[SerializeField] [Range(0.0f, 10.0f)] protected float m_screenShake = .5f;

	protected float m_speed = 0.0f;
	protected float m_tiltAngle = 1.0f;
	protected float m_turnAngle = 0.0f;
	protected Vector3 m_velocity = Vector3.zero;
	protected bool m_isMoving = false;

	float m_shotTimer = 0.0f;
	bool m_isAlive = false;

	AudioSource m_audio = null;

	public bool Alive { get { return m_isAlive; } }
	public bool ShotReady { get { return m_shotTimer >= m_shotTime; } }
	public float Size { get { return m_size; } }
	public float Health { get { return m_health; } }

	private void Start()
	{
		m_audio = gameObject.GetComponent<AudioSource>();
	}

	protected void StartedMoving()
	{
		m_isMoving = true;

		if (m_audio)
		{
			m_audio.clip = m_moveSound;
			m_audio.loop = true;
			m_audio.Play();
		}
	}

	protected void Stop()
	{
		m_isMoving = false;

		if (m_audio)
		{
			m_audio.loop = false;
			m_audio.Stop();
		}
	}

	void FixedUpdate()
	{
		if (Alive)
		{
			if (m_shotTimer < m_shotTime)
			{
				m_shotTimer += Time.deltaTime;
			}

			if (m_health <= 0.0f)
			{
				Died();
				PlayDieSound();
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
		Stop();
	}

	public virtual GameObject Fire()
	{
		GameObject hit = gameObject;

		if (ShotReady)
		{
			PlayGunSound();
			m_shotTimer = 0.0f;
			hit = null;

			float distance = Mathf.Pow(m_projectileSpeed, 2);
			float launch = Mathf.Deg2Rad * m_tiltAngle;

			distance *= Mathf.Sin(2 * launch);
			distance /= Game.Instance.Gravity;

			GameObject[] gos = Game.Instance.GetObjectsInRange(this.gameObject, distance, "Tank");

			foreach (GameObject go in gos)
			{
				if (go != gameObject)
				{

					Vector3 point = go.transform.position - transform.position;
					float difference = point.magnitude;

					float high = difference;
					float low = difference;
					Tank t = go.GetComponent<Tank>();
					if (t)
					{
						high += t.Size;
						low -= t.Size;
					}

					if (distance >= low && distance <= high)
					{
						Vector3 north = Vector3.forward * difference;
						north = Quaternion.Euler(0.0f, m_turnAngle, 0.0f) * north;

						float offset = (point - north).magnitude;
						float test = m_errorMargin;

						if (t)
						{
							test += t.Size;
						}

						if (offset <= test)
						{
							hit = go;
							if (offset <= test - m_errorMargin && t)
							{
								t.Hit(m_damage);
							}
							break;
						}
					}
				}
			}

		}

		return hit;
	}

	public virtual void Hit(float damage)
	{
		m_health -= damage;
	}

	public virtual void Collision()
	{
		m_speed = 0.0f;
	}

	public bool Colliding(GameObject other)
	{
		float rawDistance = (other.transform.position - transform.position).magnitude;

		float walls = Size;
		Tank t = other.GetComponent<Tank>();
		if (t)
		{
			walls += t.Size;
		}

		return walls >= rawDistance;
	}

	public void PlayGunSound()
	{
		if (m_gun)
		{
			m_gun.Play();
		}
	}

	public void PlayMoveSound()
	{
		if (m_audio)
		{
			m_audio.clip = m_moveSound;
			m_audio.Play();
		}
	}

	public void PlayDieSound()
	{
		if (m_audio)
		{
			m_audio.clip = m_deadSound;
			m_audio.Play();
		}
	}
}
