using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] Tank[] m_spawns;
	[SerializeField] float m_spawnSpeed = 5.0f;
	[SerializeField] int m_maxPopulation = 3;
	[SerializeField] int m_spawnAmount = 4;

	bool[] m_isDead = new bool[2]; //limit reached, all units are dead
	bool m_isSpawning = false;
	float m_spawnTimer = 0.0f;
	int m_amount = 0;

	public bool[] Alive { get { return m_isDead; } }

	void Update()
	{
		if (m_isSpawning)
		{
			int population = 0;
			foreach (Tank t in m_spawns)
			{
				if (t.Alive)
				{
					++population;
				}
			}

			if (m_spawnTimer >= m_spawnSpeed)
			{
				if (population < m_maxPopulation)
				{
					foreach (Tank t in m_spawns)
					{
						if (!t.Alive)
						{
							t.Spawn();
							m_spawnTimer = 0.0f;
							++m_amount;
						}
					}
				}
			}
			else
			{
				m_spawnTimer += Time.deltaTime;
			}

			if (m_amount >= m_spawnAmount)
			{
				m_isDead[0] = true;
				SpawnerOff();
			}

			if(population == 0)
			{
				m_isDead[1] = true;
			}
		}
	}

	public void SpawnerOn()
	{
		m_isSpawning =  true;
	}

	public void SpawnerOff()
	{
		m_isSpawning = false;
		m_amount = 0;
	}

}
