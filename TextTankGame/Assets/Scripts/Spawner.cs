using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] Tank[] m_spawns;
	[SerializeField] float m_spawnSpeed = 5.0f;
	[SerializeField] int m_maxPopulation = 3;
	[SerializeField] int m_spawnAmount = 4;

	bool m_isSpawning = false;
	float m_spawnTimer = 0.0f;
	int m_amount = 0;

	void Update()
	{
		if (m_isSpawning)
		{
			int m_population = 0;
			foreach (Tank t in m_spawns)
			{

				if (t.Alive)
				{
					++m_population;
				}
			}

			if (m_spawnTimer >= m_spawnSpeed)
			{
				if (m_population < m_maxPopulation)
				{
					foreach (Tank t in m_spawns)
					{
						if (!t.Alive)
						{
							Debug.Log("Spawned " + t.name);
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
				SpawnerOff();
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
