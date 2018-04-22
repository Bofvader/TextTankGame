using System;
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
	[SerializeField] AudioClip m_blip = null;
	[SerializeField] AudioClip m_return = null;
	[SerializeField] int m_maxLineCount = 10;
	[SerializeField] float m_loadTime = 1.0f;

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
		"loot([0-9])", "check([0-9])", "scavenge([0-9])", //loot
		"([0-9])" //menu controls
	};

	string m_prefix = "^(?isx)";
	string m_suffix = "$";

	string[] m_displayLog;

	int m_selectedMenu = 0; //0 - main, 1 - game
	bool m_quiting = false;
	AudioSource m_audio;

	public static string Msg { get; set; }

	private void Start()
	{
		m_input.ActivateInputField();
		m_audio = gameObject.GetComponent<AudioSource>();
		m_displayLog = new string[m_maxLineCount];

		ClearConsole();

		AddToLog("0 - Singleplayer");
		AddToLog("1 - Multiplayer (Not Implimented yet)");
		AddToLog("2 - Exit");
	}

	void Update()
	{
		switch (m_selectedMenu)
		{
			case 0:
				UpdateMenu();
				break;
			case 1:
				UpdateGame();
				break;
		}
	}

	void UpdateMenu()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.textComponent.text;

			m_input.Select();
			m_input.text = "";
			m_input.ActivateInputField();

			input = input.Trim();
			input = input.Replace(" ", "");
			input = input.Replace("'", "");
			input = input.Replace("?", "");

			Match match;
			string test = m_prefix;
			test += m_wordList[m_wordList.Length - 1] + m_suffix;

			match = Regex.Match(input, test);
			if (match.Success)
			{
				string raw = match.Value;
				int selection = Int32.Parse(raw);
				switch(selection)
				{
					case 0: //Singlplayer
						StartCoroutine(Loading(1));
						break;
					case 1: //Multiplayer
						SetUpLan();
						StartCoroutine(Loading(1));
						break;
					case 2: //Exit
						ClearConsole();
						AddToLog("Goodbye");
						Game.Instance.ExitGame();
						break;
				}
			}
		}

	}

	private void SetUpLan()
	{

	}

	private IEnumerator Loading(int next)
	{
		yield return new WaitForSeconds(0.5f);

		ClearConsole();

		AddToLog("Loading...");

		yield return new WaitForSeconds(m_loadTime);
		m_selectedMenu = next;

		ClearConsole();

		switch(m_selectedMenu)
		{
			case 0:
				AddToLog("0 - Singleplayer");
				AddToLog("1 - Multiplayer (Not Implimented yet)");
				AddToLog("2 - Exit");
				break;
			case 1:
				AddToLog("Would you like to play a game?");
				Game.Instance.StartGame();
				break;
		}
	}

	void UpdateGame()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.textComponent.text;
			AddToLog(input);

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
			foreach (string body in m_wordList)
			{
				string test = m_prefix;
				test += body + m_suffix;

				match = Regex.Match(input, test);
				if (match.Success)
				{
					matched = true;
					if (index > 4)
					{
						m_quiting = false;
					}

					switch (index)
					{
						case 0: //quit
						case 1: //leave
						case 2: //fullretreat
						case 3: //surrender
							Quit();
							break;
						case 4: //fire
						case 5: //shoot
							AddToLog("Firing...");
                            m_player.Fire();
							//call player.fire();
							break;
						case 6: //bang
						case 7: //pow
						case 8: //pewpew
							AddToLog("Firing...were the sounds necessary?");
                            m_player.Fire();
							//call player.fire();
							break;
						case 9: //scan
						case 10: //search
						case 11: //lookaround
						case 12: //whatdoyousee
						case 13: //whatsaroundus
						case 14: //tellmethesituation
						case 15: //whatsthesituation
							AddToLog("Scanning...");
                            m_player.Scan();
							//call player.scan();
							break;
						case 16: //aim
							AddToLog("...");
                            m_player.Angle(int.Parse(match.Groups[1].Value));
							//call player.angle(match.groups[1]);
							break;
						case 17: //turn
							AddToLog("...");
                            m_player.Turn(int.Parse(match.Groups[1].Value));
							//call player.turn(match.groups[1]);
							break;
						case 18: //aimalt
							AddToLog("Aye sir");
                            m_player.Angle(int.Parse(match.Groups[1].Value));
							//call player.angle(match.groups[1]);
							break;
						case 19: //turnalt
							AddToLog("Aye sir");
                            m_player.Turn(int.Parse(match.Groups[1].Value));
							//call player.turn(match.groups[1]);
							break;
						case 20: //move
						case 21: //advance
							AddToLog("Moving " + match.Groups[1]);
                            m_player.Move(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
							//call player.move(match.group[1], match.group[2]);
							break;
						case 22: //retreat
							AddToLog("Retreating...");
                            m_player.Retreat(int.Parse(match.Groups[1].Value));
							//call player.retreat(match.Groups[1]);
							break;
						case 23: //retreatalt
							AddToLog("Retreating..." + match.Groups[2]);
                            m_player.Retreat(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
							//call player.retreat(match.Groups[1], match.Groups[2]);
							break;
						case 24: //movealt
							AddToLog("Moving " + match.Groups[1] + " " + match.Groups[3]);
                            m_player.Move(match.Groups[1].Value, int.Parse(match.Groups[2].Value), match.Groups[3].Value);
							//call player.move(match.Groups[1], match.Groups[2], match.Groups[3]);
							break;
						case 25: //loot
						case 26: //check
						case 27: //scavenge
							AddToLog("Scavenging...");
                            m_player.Loot(int.Parse(match.Groups[1].Value));
							//call player.loot(match.Groups[1]);
							break;
						default:
							AddToLog("I'm sorry, I don't know what to do.");
							break;
					}
				}

				++index;
			}

			if (!matched)
			{
				AddToLog("Order not recognized.");
			}
		}

	}

	private void FixedUpdate()
	{
		PrintToConsole();
	}

	void ClearConsole()
	{
		for (int i = 0; i < m_maxLineCount; i++)
		{
			m_displayLog[i] = " ";
		}
	}

	void Quit()
	{
		if (m_quiting)
		{
			AddToLog("Got it, the conflict is over.");
			Game.Instance.QuiteToMenu();
			StartCoroutine(Loading(0));
		}
		else
		{
			m_quiting = true;
			AddToLog("Are we really leaving?");
		}
	}

	public void LogMessage()
	{
		AddToLog(Msg);
	}

	void AddToLog(string addition)
	{
		string[] other = m_displayLog;
		for (int i = 0; i < other.Length - 1; i++)
		{
			m_displayLog[i] = other[i + 1];
		}

		m_displayLog[m_maxLineCount - 1] = addition;
	}

	void PrintToConsole()
	{
		string log = "";

		bool start = true;
		foreach (string line in m_displayLog)
		{
			if (start)
			{
				log += line;
				start = false;
			}
			else
			{
				log += "\n" + line;
			}
		}

		m_log.text = log;
	}

	public void PlaBlipSound()
	{
		if (m_audio)
		{
			m_audio.clip = m_blip;
			m_audio.Play();
		}
	}

	public void PlayReturnSound()
	{
		if (m_audio)
		{
			m_audio.clip = m_return;
			m_audio.Play();
		}
	}
}
