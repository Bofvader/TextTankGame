using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{
	[SerializeField] Text m_log = null;
	[SerializeField] InputField m_input = null;
	[SerializeField] Player m_player = null;

	string[] m_wordList =
	{
		"quit", "leave", "fullretreat", "surrender", //quit
		"fire", "shoot", //fire
		"bang", "pow", "pewpew", //fireAlt
		"scan", "search", "lookaround", "whatdoyousee", "whatsaaroundus","tellmethesituation", "whatsthesituation", //scan
		"angle({|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})", "turn({|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})", //aim
		"elevation({|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})", "direction({|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})", //aimAlt
		"move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})", "advance({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})", //move
		"retreat({|0?[1-9]|[1-9][0-9]|})", //retreating
		"retreat({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?", //retreatalt
		"move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?", //moveAlt
		"loot([0-9])", "check([0-9])", "scavenging([0-9])" //loot
	};

	string m_prefix = "^(?isx)";
	string m_suffix = "$";

	bool m_quiting = false;

	public static string Msg { get; set; }

	private void Start()
	{
		m_input.ActivateInputField();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.textComponent.text;
			m_log.text += "\n->" + input;
			m_input.Select();
			m_input.text = "";
			m_input.ActivateInputField();

			input = input.Trim();
			input = input.Replace(" ", "");
			input = input.Replace("'", "");
			input = input.Replace("?", "");

			Match match;

			int index = 0;
			bool matched = false;
			foreach(string body in m_wordList)
			{
				string test = m_prefix;
				test += body + m_suffix;

				match = Regex.Match(input, test);
				if(match.Success)
				{
					Debug.Log(test);

					matched = true;
					if(index > 2)
					{
						m_quiting = false;
					}

					switch(index)
					{
						case 0:
						case 1:
						case 2:
						case 3:
							Quit();
							break;
						case 4:
						case 5:
							m_log.text += "\nFiring...";
							//call player.fire();
							break;
						case 6:
						case 7:
						case 8:
							m_log.text += "\nFiring...were the sounds necessary?";
							//call player.fire();
							break;
						case 9:
						case 10:
						case 11:
						case 12:
						case 13:
						case 14:
						case 15:
							m_log.text += "\nScanning...";
							//call player.scan();
							break;
						case 16:
							m_log.text += "\n...";
							//call player.angle(match.groups[1]);
							break;
						case 17:
							m_log.text += "\n...";
							//call player.turn(match.groups[1]);
							break;
						case 18:
							m_log.text += "\nAye sir";
							//call player.angle(match.groups[1]);
							break;
						case 19:
							m_log.text += "\nAye sir";
							//call player.turn(match.groups[1]);
							break;
						case 20:
						case 21:
							m_log.text += "\nMoving " + match.Groups[1];
							//call player.move(match.group[1], match.group[2]);
							break;
						case 22:
							m_log.text += "\nRetreating...";
							//call player.retreat(match.Groups[1]);
							break;
						case 23:
							m_log.text += "\nRetreating..." + match.Groups[2];
							//call player.retreat(match.Groups[1], match.Groups[2]);
							break;
						case 24:
							m_log.text += "\nMoving " + match.Groups[1] + " " + match.Groups[3];
							//call player.move(match.Groups[1], match.Groups[2], match.Groups[3]);
							break;
						case 25:
						case 26:
						case 27:
							m_log.text += "\nScavenging...";
							//call player.loot(match.Groups[1]);
							break;
						default:
							m_log.text += "\nI'm sorry, I don't know what to do.";
							break;
					}
				}

				index++;
			}

			if(!matched)
			{
				m_log.text += "\nOrder not recognized.";
			}
		}
	}

	void Quit()
	{
		if(m_quiting)
		{
			m_log.text += "\nGot it, the conflict is over.";
			//Quit game;
		}
		else
		{
			m_quiting = true;
			m_log.text += "\nAre we really leaving?";
		}
	}

	public void LogMessage()
	{
		m_log.text += "\n" + Msg;
	}
}
