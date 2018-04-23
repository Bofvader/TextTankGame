using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : Singleton<Game>
{
	[SerializeField] Spawner[] m_spawners;
	[SerializeField] public Tank[] m_actors;
	[SerializeField] float m_gravity = 9.8f;
	[SerializeField] float m_level = 1.0f;
	[SerializeField] float m_endingTime = 2.0f;

	bool m_gameStarted = false;
	bool m_gameEnded = false;

	public float Gravity { get { return m_gravity; } }
	public float Level { get { return m_level; } }
	public bool Started { get { return m_gameStarted; } }

	void Update()
	{
		if (m_gameStarted)
		{
			CollisionDetection();

			int death = 0;
			foreach (Spawner s in m_spawners)
			{
				if (!s.Alive)
				{
					++death;
				}
			}

			if (death == m_spawners.Length - 1 && !m_gameEnded)
			{
				m_gameEnded = true;
				StartCoroutine(EndGame());
			}
		}
	}
	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(m_endingTime);
		QuiteToMenu();
		foreach (Tank t in m_actors)
		{
			if (t.GetType() == typeof(Player))
			{
				(t as Player).BringUpMenu();
			}
		}

	}

	void CollisionDetection()
	{
		int i = 0;
		foreach (Tank t in m_actors)
		{
			int j = 0;
			foreach (Tank o in m_actors)
			{
				if (i != j && t.Colliding(o.gameObject))
				{
					t.Collision();
					o.Collision();
				}

				++j;
			}
			++i;
		}
	}

	public GameObject[] GetObjectsInRange(GameObject source, float radius, string targetTag)
	{
		List<GameObject> returnGameObjects = new List<GameObject>();

		GameObject[] gameObjects;
		gameObjects = GameObject.FindGameObjectsWithTag(targetTag);

		foreach (GameObject go in gameObjects)
		{
			float distance = (go.transform.position - source.transform.position).magnitude;
			if (distance <= radius)
			{
				returnGameObjects.Add(go);
			}
		}

		return returnGameObjects.ToArray();
	}

	public void QuiteToMenu()
	{
		m_gameStarted = false;

		foreach (Tank t in m_actors)
		{
			t.Died(false);
		}

		foreach (Spawner s in m_spawners)
		{
			s.SpawnerOff();
			s.Reset();
		}
	}

	public void StartGame()
	{
		foreach (Spawner s in m_spawners)
		{
			s.SpawnerOn();
		}

		m_gameStarted = true;
		m_gameEnded = false;
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
