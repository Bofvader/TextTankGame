using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{

	[SerializeField] Tank[] m_actors;
	[SerializeField] float m_gravity = 9.8f;
	[SerializeField] float m_scale = 1.0f;

	public float Gravity { get { return m_gravity; } }
	public float Scale { get { return m_scale; } }

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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

	}
}
