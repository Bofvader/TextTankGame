using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    [SerializeField] Console m_console = null;
    [SerializeField] float m_closeHit = 1.0f;
    [SerializeField] float m_inVicinity = 3.0f;
    [SerializeField] float m_scanRange = 100.0f;

	public override GameObject Fire()
	{
        GameObject result = base.Fire();

        Tank enemy = result.GetComponent<Tank>();
        float distance = (result.transform.position - transform.position).magnitude;

        if (enemy)
        {
            m_console.LogMessage("You fired a shell directly into the opposing tank!");
        }
        else
        {
            if (distance < m_closeHit)
            {
                m_console.LogMessage("Your shot was close, but you only grazed their tank.");

            }
            else if (distance < m_inVicinity)
            {
                m_console.LogMessage("You've got them shacking in fear, but no real damage was done.");
            }
            else
            {
                m_console.LogMessage("Where were you aiming? There's no tank over there!");
            }
        }

        return result;
    }

    public override void Spawn()
    {
        m_console.LogMessage("Get those fingers preped and ready! Open fire!");
        base.Spawn();
    }

    public override void Died()
    {
        m_console.LogMessage("What kind of tank commander are you?!? We're more like swiss cheese than a tank.");
        base.Died();
    }

    public override void Hit(float damage)
    {
        m_console.LogMessage("We've been hit! Return fire before we're finished!");
        base.Hit(damage);
    }

    public override void Collision()
    {
        m_console.LogMessage("You've slammed into something! It must be another tank, shoot them!");
        base.Collision();
    }

    public void Loot(int deadTankNum)
    {
        bool foundTank = false;

        for(int i = 0; i < Game.Instance.m_actors.Length; i++)
        {
            if(deadTankNum == i)
            {
                foundTank = true;

                if (!Game.Instance.m_actors[i].Alive)
                {
                    int num = (int)Random.Range(0.0f, 4.0f);
                    m_console.LogMessage("Tank carcass found. Scavenging now!");

                    if(num == 0)
                    {
                        m_console.LogMessage("You have found a wounded man, taking him hostage");

                    } else if (num == 1)
                    {
                        m_console.LogMessage("You found a few tank shells close to exploding, but you saved them so they should be safe to use.");
                        m_damage += 20;

                    } else if (num == 2)
                    {
                        m_console.LogMessage("You found some tank plating that we could use for ourselves!");
                        m_hitPoints += m_damage;

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

    public void Retreat(float distance, string speed = "normal")
    {
        m_velocity = Vector3.zero;
        
        m_velocity = Vector3.back * m_maxSpeed;
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
        m_velocity = Vector3.zero;

        if(direction.CompareTo("north") == 0)
        {
            Turn(0);
            transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(0.0f, 0.0f, 1.0f));
            m_velocity = Vector3.forward * m_maxSpeed;
            m_travelTime = m_velocity.magnitude / distance;

        } else if (direction.CompareTo("west") == 0)
        {
            Turn(90);
            transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(1.0f, 0.0f, 0.0f));
            m_velocity = Vector3.forward * m_maxSpeed;
            m_travelTime = m_velocity.magnitude / distance;
            
        }
        else if (direction.CompareTo("south") == 0)
        {
            Turn(180);
            transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(0.0f, 0.0f, -1.0f));
            m_velocity = Vector3.forward * m_maxSpeed;
            m_travelTime = m_velocity.magnitude / distance;

        } else if (direction.CompareTo("east") == 0)
        {
            Turn(270);
            transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(-1.0f, 0.0f, 0.0f));
            m_velocity = Vector3.forward * m_maxSpeed;
            m_travelTime = m_velocity.magnitude / distance;

        }
        else
        {
            m_console.LogMessage("Invalid move command. Please input direction (north, south, east, or west) and a number to move");
        }

        if(m_velocity.magnitude > 0.0f)
        {
            if (speed.CompareTo("halftime") == 0)
            {
                m_velocity /= 2.0f;
                m_travelTime *= 2.0f;

            } else if(speed.CompareTo("doubletime") == 0)
            {
                m_velocity *= 2.0f;
                m_travelTime /= 2.0f;

            } else if(speed.CompareTo("tripletime") == 0)
            {
                m_velocity *= 3.0f;
                m_travelTime /= 3.0f;
            }
        }
    }

    public void Advance(float distance)
    {
        m_velocity = Vector3.zero;

        m_velocity = Vector3.forward * m_maxSpeed;
        m_travelTime = m_velocity.magnitude / distance;

    }

    public void Scan()
    {
        foreach(Tank tank in Game.Instance.m_actors)
        {
            string message = "";

            if (this != tank)
            {
                if (tank.Alive)
                {
                    message = "An enemy tank was spotted to the ";
                }
                else
                {
                    message = "Smoldering tank remains have been spotted to the ";
                }

                Vector3 distance = tank.transform.position - transform.position;
                
                if(distance.z > 0) // north
                {
                    if(distance.x > 0)// east
                    {
                        if(distance.z / distance.x >= 2)
                        {
                            message += "north";

                        } else if (distance.x / distance.z >= 2)
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
                    if(distance.x > 0) // east
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

    public void Turn(int angle)
    {
        m_turnAngle = angle;

        m_console.LogMessage("We've turned our tank to " + angle + "degrees");
    }

    public void Angle(int angle)
    {
        m_tiltAngle = angle;

        m_console.LogMessage("We've rotated the tank's barrel to " + angle + "degrees");
    }
}
