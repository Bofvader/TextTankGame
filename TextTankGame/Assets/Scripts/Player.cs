using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    [SerializeField] Console m_console = null;
    [SerializeField] float m_closeHit = 1.0f;
    [SerializeField] float m_inVicinity = 3.0f;
    [SerializeField] float m_scanRange = 100.0f;

	protected float m_travelTime = 0.0f;

	protected void Update()
	{
		if (Alive)
		{
			if (m_travelTime > 0)
			{
				transform.position += (m_velocity * Time.deltaTime);
				m_travelTime -= Time.deltaTime;
			}
			else
			{
				m_velocity = Vector3.zero;
				Stop();
			}
		}
		else
		{
			m_travelTime = 0.0f;
			Stop();
		}
	}

	public void BringUpMenu()
	{
		m_console.LogMessage("Round over");
		m_console.Quit();
		m_console.Quit();
		StartCoroutine(m_console.Loading());
	}

	public override GameObject Fire()
	{
		GameObject result = base.Fire();

		if (result)
		{ 
			Tank enemy = result.GetComponent<Tank>();

			if (enemy)
			{
				if (enemy != this)
				{
					float travel = Mathf.Pow(m_projectileSpeed, 2);
					float launch = Mathf.Deg2Rad * m_tiltAngle;

					travel *= Mathf.Sin(2 * launch);
					travel /= Game.Instance.Gravity;

					int limit = (int)travel;

					Vector3 path = Vector3.forward * limit;
					path = Quaternion.Euler(0.0f, m_turnAngle, 0.0f) * path;

					int distance = (int)((result.transform.position - path).magnitude - enemy.Size);

					if (distance <= (int)m_closeHit && distance > 0)
					{
						m_console.LogMessage("--Your shot was close, but you only grazed their tank.");

					}
					else if (distance <= (int)m_inVicinity && distance > m_closeHit)
					{
						m_console.LogMessage("--You've got them shacking in fear, but no real damage was done.");
					}
					else
					{
						m_console.LogMessage("--Direct hit, do it again.");
					}
				}
				else
				{
					m_console.LogMessage("--Still reloading sir.");
				}
			}
        }
		else
		{
			m_console.LogMessage("--You hit no one. Who are you aiming at?");
		}

        return result;
    }

    public override void Spawn()
    {
        m_console.LogMessage("-Attention! Time for work, destroy enemy armor.");
        base.Spawn();
    }

    public override void Died(bool actualDeath)
    {
		if (actualDeath)
		{
			m_console.LogMessage("-Happy commander? We're more like swiss cheese than a tank.");
		}
        base.Died();
    }

    public override void Hit(float damage)
    {
        m_console.LogMessage("-We've been hit! Take them down quickly");
        CameraController.Instance.Shake(m_screenShake);
        base.Hit(damage);
    }

    public override void Collision()
    {
        m_console.LogMessage("-You've slammed into something! You're lucky we don't have any new holes!");
        CameraController.Instance.Shake(m_screenShake);
        base.Collision();
    }

    public void Loot(int deadTankNum)
    {
        bool foundTank = false;

        for (int i = 0; i < Game.Instance.m_actors.Length; i++)
        {
            if (deadTankNum == i)
            {
                foundTank = true;

                if (!Game.Instance.m_actors[i].Alive)
                {
                    int num = (int)Random.Range(0.0f, 4.0f);
                    m_console.LogMessage("Tank carcass found. Scavenging now!");

                    if (num == 0)
                    {
                        m_console.LogMessage("You have found a wounded man, taking him hostage");

                    }
                    else if (num == 1)
                    {
                        m_console.LogMessage("You found a few tank shells close to exploding, but you saved them so they should be safe to use.");
                        m_damage += 20;

                    }
                    else if (num == 2)
                    {
                        m_console.LogMessage("You found some tank plating that we could use for ourselves!");
						m_health += m_damage;

                    }
                    else
                    {
                        m_console.LogMessage("Anything useful has already exploded or burnt to a crisp");
                    }
                }
                else
                {
                    m_console.LogMessage("Tank number " + deadTankNum + " is still kicking. Take them down so we can take their stuff!");
                }
            }
        }

        if (!foundTank)
        {
            m_console.LogMessage("No tank with the number " + deadTankNum + " is out there. Search for a number from zero to " + (Game.Instance.m_actors.Length - 1));
        }
    }

	public void Advance(float distance, string speed = "normal")
	{
		m_velocity = Vector3.zero;

		m_velocity = Vector3.forward * m_maxSpeed;
		Quaternion face = Quaternion.Euler(0.0f, m_turnAngle, 0.0f);

		m_velocity = face * m_velocity;
		m_travelTime = m_velocity.magnitude / distance;

		if (m_velocity.magnitude > 0.0f)
		{
			if (speed.CompareTo("halftime") == 0)
			{
				m_velocity /= 2.0f;
				m_travelTime *= 2.0f;

			}
			else if (speed.CompareTo("doubletime") == 0)
			{
				m_velocity *= 2.0f;
				m_travelTime /= 2.0f;
			}
			else if (speed.CompareTo("tripletime") == 0)
			{
				m_velocity *= 3.0f;
				m_travelTime /= 3.0f;
			}
		}
	}

	public void Retreat(float distance, string speed = "normal")
    {
        m_velocity = Vector3.zero;

        m_velocity = Vector3.back * m_maxSpeed;
		Quaternion face = Quaternion.Euler(0.0f, m_turnAngle, 0.0f);

		m_velocity = face * m_velocity;
		m_travelTime = m_velocity.magnitude / distance;

        if (m_velocity.magnitude > 0.0f)
        {
            if (speed.CompareTo("halftime") == 0)
            {
                m_velocity /= 2.0f;
                m_travelTime *= 2.0f;

            }
            else if (speed.CompareTo("doubletime") == 0)
            {
                m_velocity *= 2.0f;
                m_travelTime /= 2.0f;
            }
            else if (speed.CompareTo("tripletime") == 0)
            {
                m_velocity *= 3.0f;
                m_travelTime /= 3.0f;
            }
        }
    }

    public void Move(string direction, float distance, string speed = "normal")
    {
       Vector3 velocity = Vector3.zero;

        if (direction.CompareTo("north") == 0)
        {
			velocity = Vector3.forward; 
        }
        else if (direction.CompareTo("west") == 0)
        {
			velocity = Vector3.left;
		}
		else if (direction.CompareTo("south") == 0)
        {
			velocity = Vector3.back;
		}
		else if (direction.CompareTo("east") == 0)
        {
			velocity = Vector3.right;
		}

		velocity = velocity * m_maxSpeed;
		m_travelTime = velocity.magnitude * distance;

		if (velocity.magnitude > 0.0f)
		{
			if (speed.CompareTo("halftime") == 0)
			{
				velocity /= 2.0f;
				m_travelTime *= 2.0f;

			}
			else if (speed.CompareTo("doubletime") == 0)
			{
				velocity *= 2.0f;
				m_travelTime /= 2.0f;

			}
			else if (speed.CompareTo("tripletime") == 0)
			{
				velocity *= 3.0f;
				m_travelTime /= 3.0f;
			}

			StartedMoving();
			m_velocity = velocity;
        }
    }

    public void Scan()
    {
		GameObject[] tanks = Game.Instance.GetObjectsInRange(gameObject, m_scanRange, "Tank");

		string message = "--";

		if (tanks.Length > 0)
		{
			foreach (GameObject go in tanks)
			{
				Tank tank = go.GetComponent<Tank>();


				if (this != tank)
				{
					if (tank.Alive)
					{
						message = "An enemy tank was spotted to the ";
					}
					else
					{
						message = "Smoldering tank remains located at ";
					}

					Vector3 distance = tank.transform.position - transform.position;

					if (distance.z > 0) // north
					{
						if (distance.x > 0)// east
						{
							if (distance.z / distance.x >= 2)
							{
								message += "north";

							}
							else if (distance.x / distance.z >= 2)
							{
								message += "east";
							}
							else
							{
								message += "northeast";
							}
						}
						else // west
						{
							if (Mathf.Abs(distance.z / distance.x) >= 2)
							{
								message += "north";
							}
							else if (Mathf.Abs(distance.x / distance.z) >= 2)
							{
								message += "west";
							}
							else
							{
								message += "northwest";
							}
						}
					}
					else // south
					{
						if (distance.x > 0) // east
						{
							if (Mathf.Abs(distance.z / distance.x) >= 2)
							{
								message += "south";
							}
							else if (Mathf.Abs(distance.x / distance.z) >= 2)
							{
								message += "east";
							}
							else
							{
								message += "southeast";
							}
						}
						else // west
						{
							if (Mathf.Abs(distance.z / distance.x) >= 2)
							{
								message += "south";
							}
							else if (Mathf.Abs(distance.x / distance.z) >= 2)
							{
								message += "west";
							}
							else
							{
								message += "southwest";
							}
						}
					}
				}
			}
		}
		else
		{
			message += "There's nothing sir.";
		}

		m_console.LogMessage(message);
    }

    public void Turn(int angle)
    {
        m_turnAngle = angle;

        m_console.LogMessage("--We've turned our tank to " + angle + " degrees");
    }

    public void Angle(int angle)
    {
        m_tiltAngle = angle;

        m_console.LogMessage("--We've angled the barrel " + angle + " degrees");
    }
}
