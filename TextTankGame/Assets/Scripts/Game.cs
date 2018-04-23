using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : Singleton<Game>
{
	[SerializeField] Spawner[] m_spawners;
	[SerializeField] public Tank[] m_actors;
	[SerializeField] float m_gravityBase = 9.8f;
	[SerializeField] float m_scale = 1.0f;
	[SerializeField] float m_level = 1.0f;

	float m_gravity = 0.0f;

	public float Gravity { get { return m_gravity; } }
	public float Scale { get { return m_scale; } }
	public float Level { get { return m_level; } }

	private void Start()
	{
		m_gravity = m_gravityBase * m_scale;
	}

	void Update()
	{
		CollisionDetection();

		int death = 0;
		foreach(Spawner s in m_spawners)
		{
			if(!s.Alive[0] && !s.Alive[1])
			{
				++death; 
			}
		}

		if(death == m_spawners.Length)
		{

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
			}
		}
	}

	public GameObject[] GetObjectsInRange(GameObject source, float radius, string targetTag)
	{
		List<GameObject> returnGameObjects = new List<GameObject>();

		GameObject[] gameObjects;
		gameObjects = GameObject.FindGameObjectsWithTag(targetTag);

		foreach(GameObject go in gameObjects) 
		{
			float distance = (go.transform.position - source.transform.position).magnitude;
			if(distance <= radius)
			{
				returnGameObjects.Add(go);
			}
		}

		return returnGameObjects.ToArray();
	}

	public void QuiteToMenu()
	{
		foreach (Tank t in m_actors)
		{
			t.Died();
		}

		foreach (Spawner s in m_spawners)
		{
			s.SpawnerOff();
		}
	}

	public void StartGame()
	{
		foreach (Spawner s in m_spawners)
		{
			s.SpawnerOn();
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
