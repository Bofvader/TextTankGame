using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{
	[SerializeField] Text m_log = null;
	[SerializeField] Text m_input = null;

	string m_aimString = @"^(?isx)Angle(?<amount>{|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})$";
	string m_turnString = @"^(?isx)turn(?<amount>{|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})$";
	string m_moveString = @"^(?isx)move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?$";

	// Update is called once per frame
	void Update()
	{
		Debug.Log("Updating");
		if(Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.text;
			m_input.text.Remove(0, input.Length);

			if(input.Equals("Scan"))
			{
				//call player.scan
				Debug.Log("Scanning");
			}
			else if(input.Equals("Fire"))
			{
				//call player.fire
				Debug.Log("Firing");
			}
			else
			{
				Match match = Regex.Match(input, m_aimString);
				if(match.Success)
				{
					//call player.angle(match.Groups[1]);
				}

				match = Regex.Match(input, m_turnString);
				if (match.Success)
				{
					//call player.turn(match.Groups[1]);
				}

				match = Regex.Match(input, m_moveString);
				if(match.Success)
				{
					if (match.Groups.Count == 5)
					{
						//call player.move(match.Groups[1], match.Groups[2], match.Groups[3]);
					}
					else
					{
						//call player.move(match.Groups[1], match.Groups[2]);
					}
				}

			}
		}
	}
}
