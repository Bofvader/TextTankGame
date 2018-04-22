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
        m_console.LogMessage("You've slammed into something! You're lucky we don't have any new holes!");
        base.Collision();
    }

    public void Loot(int deadTankNum)
    {

    }

    //public void Retreat(int distance)
    //{

    //}

    public void Retreat(int distance, string speed = "normal")
    {

    }

    //public void Move(string direction, int distance)
    //{
        
    //}

    public void Move(string direction, int distance, string speed = "normal")
    {
        if(direction.CompareTo("north") == 0)
        {
            Turn(0);
            transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f));

        } else if (direction.CompareTo("west") == 0)
        {
            Turn(90);

        } else if (direction.CompareTo("south") == 0)
        {
            Turn(180);

        } else if (direction.CompareTo("east") == 0)
        {
            Turn(270);

        }
        else
        {
            m_console.LogMessage("Invalid move command. Please input direction (north, south, east, or west) and a number to move");
        }
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
    }

    public void Angle(int angle)
    {
        m_tiltAngle = angle;
    }
}
