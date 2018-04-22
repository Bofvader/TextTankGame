using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    [SerializeField] Console m_console = null;
    [SerializeField] float m_closeHit = 1.0f;
    [SerializeField] float m_inVicinity = 3.0f;

	public override GameObject Fire()
	{
        GameObject result = base.Fire();

        Tank enemy = result.GetComponent<Tank>();
        float distance = (result.transform.position - transform.position).magnitude;

        if (enemy)
        {
            Console.Msg = "You fired a shell directly into the opposing tank!";
            m_console.LogMessage();
        }
        else
        {
            if (distance < m_closeHit)
            {
                Console.Msg = "Your shot was close, but you only grazed their tank.";

            }
            else if (distance < m_inVicinity)
            {
                Console.Msg = "You've got them shacking in fear, but no real damage was done".;
            }
            else
            {
                Console.Msg = "Where were you aiming? There's no tank over there!";
            }
            m_console.LogMessage();
        }

        return result;
    }

    public override void Spawn()
    {
        Console.Msg = "Get those fingers preped and ready! An enemy tank has appeared!";
        m_console.LogMessage();
        base.Spawn();
    }

    public override void Died()
    {
        Console.Msg = "What kind of tank commander are you?!? We're more like swiss cheese than a tank.";
        m_console.LogMessage();
        base.Died();
    }

    public override void Hit(float damage)
    {
        Console.Msg = "We've been hit! Take them down quickly";
        m_console.LogMessage();
        base.Hit(damage);
    }

    public override void Collision()
    {
        Console.Msg = "You've slammed into something! You're lucky we don't have any new holes!";
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

    }

    public void Turn(int angle)
    {

    }

    public void Angle(int angle)
    {

    }
    //loot, move, retreat, scan, turn, angle, 
}
