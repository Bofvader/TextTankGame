using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour
{
	[SerializeField] Vector3 m_spawnPoint;
	[SerializeField] protected float m_maxSpeed = 5.0f;
	[SerializeField] float m_shotTime = 1.0f;
    [SerializeField] float m_tankSize = 1.0f;
    [SerializeField] float m_hitPoints = 100;
    [SerializeField] float m_damage = 40;
	public bool ShotReady { get { return m_shotTimer >= m_shotTime; } }

	protected float m_speed = 0.0f;
	protected float m_shotTimer = 0.0f;
	protected float m_tiltAngle = 0.0f;
	protected float m_turnAngle = 0.0f;

    [SyncVar] public bool m_isAlive = false;

	void FixedUpdate()
    {
        if (isLocalPlayer)
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

	public virtual bool Fire()
	{
		bool hit = false;

		if(ShotReady)
		{

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
		if(t)
		{
			walls += t.m_tankSize;
		}

		return walls * Game.Instance.Scale >= rawDistance;
	}
}
