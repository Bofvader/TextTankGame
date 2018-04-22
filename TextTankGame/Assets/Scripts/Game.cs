using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
	[SerializeField] Spawner[] m_spawners;
	[SerializeField] Tank[] m_actors;
	[SerializeField] float m_gravity = 9.8f;
	[SerializeField] float m_scale = 1.0f;

	public float Gravity { get { return m_gravity; } }
	public float Scale { get { return m_scale; } }

	void Update()
	{
		CollisionDetection();
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

	public void QuiteToMenu()
	{
		foreach(Tank t in m_actors)
		{
			t.Died();
		}

		foreach(Spawner s in m_spawners)
		{
			s.SpawnerOff();
		}
	}

	public void StartGame()
	{
		foreach(Spawner s in m_spawners)
		{
			s.SpawnerOn();
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
