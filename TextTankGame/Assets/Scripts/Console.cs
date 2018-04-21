using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{
	[SerializeField] Text m_log = null;
	[SerializeField] InputField m_input = null;

	string m_fireString = @"^(?isx)fire&";
	string m_scanString = @"^(?isx)scan&";
	string m_aimString = @"^(?isx)Angle(?<amount>{|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})$";
	string m_turnString = @"^(?isx)turn(?<amount>{|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})$";
	string m_moveString = @"^(?isx)move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?$";

	public static string Msg { get; set; }

	// Update is called once per frame
	void Update()
	{
		Debug.Log("Updating");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.textComponent.text;
			input = input.Trim();

			Match match = Regex.Match(input, m_scanString);
			if (match.Success)
			{
				input += "\nScanning...";
				//call player.scan
				//input += Msg;
				Debug.Log("Scanning");
			}
			else
			{
				match = Regex.Match(input, m_fireString);
				if (match.Success)
				{
					input += "\nFiring...";
					//call player.fire
					//input += Msg;
					Debug.Log("Firing");
				}
				else
				{
					match = Regex.Match(input, m_aimString);
					if (match.Success)
					{
						//call player.angle(match.Groups[1]);
						input += "...";
					}
					else
					{
						match = Regex.Match(input, m_turnString);
						if (match.Success)
						{
							//call player.turn(match.Groups[1]);
							input += "...";
						}
						else
						{
							match = Regex.Match(input, m_moveString);
							if (match.Success)
							{
								if (match.Groups.Count == 5)
								{
									//call player.move(match.Groups[1], match.Groups[2], match.Groups[3]);
									//input += Msg;
								}
								else
								{
									//call player.move(match.Groups[1], match.Groups[2]);
									//input += Msg;
								}
							}
							else
							{
								input += ", order not recongnixed";
							}
						}
					}
				}

				m_log.text += "\n" + input;
				m_input.textComponent.text = "";
			}
		}
	}
}
