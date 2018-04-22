using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{

	// Use this for initialization
	void Start()
	{

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

	// Update is called once per frame
	void Update()
	{

	}
}
