using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] Tank[] m_spawns;
	[SerializeField] float m_spawnSpeed = 5.0f;
	[SerializeField] int m_maxPopulation = 3;
	[SerializeField] int m_spawnAmount = 4;

	public bool m_isAlive = true;
	bool m_isSpawning = false;
	float m_spawnTimer = 0.0f;
	int m_amount = 0;

	public bool Alive { get { return m_isAlive; } }

	void Update()
	{
		if (Game.Instance.Started)
		{

			int population = 0;
			foreach (Tank t in m_spawns)
			{
				if (t.Alive)
				{
					++population;
				}
			}

			if (!m_isSpawning && population == 0)
			{
				m_isAlive = false;
			}

			if (m_isSpawning)
			{
				if (m_amount < m_spawnAmount)
				{
					if (population < m_maxPopulation)
					{
						if (m_spawnTimer >= m_spawnSpeed)
						{
							foreach (Tank t in m_spawns)
							{
								if (!t.Alive)
								{
									t.Spawn();
									++m_amount;
									break;
								}
							}
						}
						else
						{
							m_spawnTimer += Time.deltaTime;
						}
					}
				}
				else
				{
					SpawnerOff();
				}
			}
		}
	}

	public void Reset()
	{
		m_isAlive = true;
		m_isSpawning = false;
		m_spawnTimer = 0.0f;
		m_amount = 0;
	}

	public void SpawnerOn()
	{
		m_isSpawning = true;
	}

	public void SpawnerOff()
	{
		m_isSpawning = false;
		m_amount = 0;
	}

}
