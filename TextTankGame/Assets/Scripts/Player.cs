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
        m_console.LogMessage("Get those fingers preped and ready! An enemy tank has appeared!");
        base.Spawn();
    }

    public override void Died()
    {
        m_console.LogMessage("What kind of tank commander are you?!? We're more like swiss cheese than a tank.");
        base.Died();
    }

    public override void Hit(float damage)
    {
        m_console.LogMessage("We've been hit! Take them down quickly");
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

    public void Retreat(int distance)
    {

    }

    public void Retreat(int distance, string speed)
    {

    }

    public void Move(string direction, int distance)
    {
        
    }

    public void Move(string direction, int distance, string speed)
    {
        
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
                        if (distance.z / distance.x >= 2)
                        {
                            // just north
                        }
                        else if (distance.x / distance.z >= 2)
                        {
                            // just west
                        }
                        else
                        {
                            //north west
                        }
                    }                    
                }
                else // south
                {
                    if(distance.x > 0) // east
                    {
                        if (distance.z / distance.x >= 2)
                        {
                            // just south
                        }
                        else if (distance.x / distance.z >= 2)
                        {
                            // just east
                        }
                        else
                        {
                            //south east
                        }
                    }
                    else // west
                    {

                    }
                }
            }
        }
    }

    public void Turn(int angle)
    {

    }

    public void Angle(int angle)
    {

    }
    //loot, move, retreat, scan, turn, angle, 
}
