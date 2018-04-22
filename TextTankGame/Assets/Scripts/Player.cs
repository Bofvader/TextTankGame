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

		string msg = "";


		if (enemy)
        {
            m_console.LogMessage("You fired a shell directly into the opposing tank!");
        }
        else
        {
            if (distance < m_closeHit)
            {
				msg = "Your shot was close, but you only grazed their tank.";

            }
            else if (distance < m_inVicinity)
            {
				msg = "You've got them shacking in fear, but no real damage was done";
            }
            else
            {
               msg = "Where were you aiming? There's no tank over there!";
            }
            m_console.LogMessage(msg);
        }

        return result;
    }

    public override void Spawn()
    {
        base.Spawn();
        m_console.LogMessage("Attention crew! Time to get to work.");
    }

    public override void Died()
    {
        base.Died();
        m_console.LogMessage("What kind of tank commander are you?!? We're more like swiss cheese than a tank.");
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        m_console.LogMessage("We've been hit! Take them down quickly");
    }

    public override void Collision()
    {
        base.Collision();
       m_console.LogMessage("You've slammed into something! You're lucky we don't have any new holes!");
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
