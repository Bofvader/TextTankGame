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
		"scan", "search", "radar", "boopboop", "lookaround", "whatdoyousee", "whatsaroundus","tellmethesituation", "whatsthesituation", //scan
		"angle({|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})", "turn({|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})", //aim
		"elevation({|0?[1-9]|[1-9][0-9]|1[0-7][0-9]|})", "direction({|0?[1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-9][0-9]|3[0-5][0-9]|})", //aimAlt
        "move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})", //move
        "advance({|0?[1-9]|[1-9][0-9]|})", "forward({|0?[1-9]|[1-9][0-9]|})",//advance
		"retreat({|0?[1-9]|[1-9][0-9]|})", //retreating
		"retreat({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?", //retreatalt
		"move({|north|east|south|west|})({|0?[1-9]|[1-9][0-9]|})({|doubletime|tripletime|halftime|})?", //moveAlt
		"loot([0-9])", "check([0-9])", "scavenge([0-9])", //loot
        "help", "whatdoido", "howdoidothis", "what", "whatisgoingon", "how", "question", //help
		"changeweapons", "cartridgechange", "loaddifferentrounds", //weapon change
		"allofthedirections", "howtotanks", "helpme", "whysixpedals", "whyonlyfourpedals", //panic
		"fuckyou", "fuckoff",  "youbitch", "fuckingbitch", "suchanasshole",  //cussing
		"volley", "volleyfire", "butthatschruch", "exitvehicle", "useobject", "greetings", "hi", //easter eggs
		"([0-9])" //menu controls
	};

	string m_prefix = "^(?isx)";
	string m_suffix = "$";

	string[] m_displayLog;

	int m_selectedMenu = 0; //0 - main, 1 - game
	bool m_quiting = false;
	AudioSource m_audio;

	private void Start()
	{
		m_input.ActivateInputField();
		m_audio = gameObject.GetComponent<AudioSource>();
		m_displayLog = new string[m_maxLineCount];

		ClearConsole();
		AddToLog("Tactical Texting");
		for(int i=1;i<m_maxLineCount-2;i++)
		{
			AddToLog("");
		}
		AddToLog("0 - Play");
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

    private void SetUpLan()
    {

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
					//case 2: //Multiplayer
					//	SetUpLan();
					//	break;
					case 1: //Exit
						ClearConsole();
						AddToLog("Goodbye");
						Game.Instance.ExitGame();
						break;
				}
			}
		}
	}

	private IEnumerator Loading(int next)
	{
		yield return new WaitForSeconds(0.5f);

		ClearConsole();

		AddToLog("Loading...(These words will break bones)");

		yield return new WaitForSeconds(m_loadTime);
		m_selectedMenu = next;

		ClearConsole();

		switch(m_selectedMenu)
		{
			case 0:
				AddToLog(">>0 - Play");
				AddToLog(">>1 - Exit");
				break;
			case 1:
				AddToLog(">>Operation start<<");
				Game.Instance.StartGame();
				break;
		}
	}

	void UpdateGame()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string input = m_input.textComponent.text;
			AddToLog(">" + input);

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
							if (m_player.Alive)
							{
								AddToLog("-Firing...");
								m_player.Fire();
							}
							break;
						case 6: //bang
						case 7: //pow
						case 8: //pewpew
							if (m_player.Alive)
							{
								AddToLog("-Firing...were the sounds necessary?");
								m_player.Fire();
							}
							break;
						case 9: //scan
						case 10: //search
                        case 11: //radar
                        case 12: //boopboop
						case 13: //lookaround
						case 14: //whatdoyousee
						case 15: //whatsaroundus
						case 16: //tellmethesituation
						case 17: //whatsthesituation
							if (m_player.Alive)
							{
								AddToLog("-Scanning...");
								m_player.Scan();
							}
							break;
						case 18: //aim
							if (m_player.Alive)
							{
								AddToLog("-Adjusting...");
								m_player.Angle(int.Parse(match.Groups[1].Value));
							}
							break;
						case 19: //turn
							if (m_player.Alive)
							{
								AddToLog("-Shifting...");
								m_player.Turn(int.Parse(match.Groups[1].Value));
							}
							break;
						case 20: //aimalt
							if (m_player.Alive)
							{
								AddToLog("-Aye sir");
								m_player.Angle(int.Parse(match.Groups[1].Value));
							}
							break;
						case 21: //turnalt
							if (m_player.Alive)
							{
								AddToLog("-Right away sir");
								m_player.Turn(int.Parse(match.Groups[1].Value));
							}
							break;
						case 22: //move
                            if (m_player.Alive)
                            {
                                AddToLog("-Moving " + match.Groups[1]);
                                m_player.Move(match.Groups[1].Value, float.Parse(match.Groups[2].Value));
                            }
                            break;
                        case 23: //advance							
                        case 24: //forward - change
                            if (m_player.Alive)
                            { 
                                AddToLog("-Advancing");
                                m_player.Advance(int.Parse(match.Groups[1].Value), match.Groups[1].Value);
                            }
                            break;
                        case 25: //retreat
							if (m_player.Alive)
							{
                                AddToLog("-Retreating...");
                                m_player.Retreat(float.Parse(match.Groups[1].Value));
                            }
							break;
						case 26: //retreatalt
							if (m_player.Alive)
							{
                                AddToLog("-Retreating..." + match.Groups[2]);
                                m_player.Retreat(float.Parse(match.Groups[1].Value), match.Groups[2].Value);
                            }
							break;
						case 27: //movealt
							if (m_player.Alive)
							{
                                AddToLog("-Moving " + match.Groups[1] + " " + match.Groups[3]);
                                m_player.Move(match.Groups[1].Value, float.Parse(match.Groups[2].Value), match.Groups[3].Value);
                            }
							break;
						case 28: //loot
						case 29: //check
						case 30: //scavenge
							if (m_player.Alive)
							{
                                AddToLog("-Scavenging...");
                                m_player.Loot(int.Parse(match.Groups[1].Value));
							}
							break;
                        case 31: //help
                        case 32: //whatdoido
                        case 33: //howdoidothis
                        case 34: //what
                        case 35: //whatisgoingon
                        case 36: //how
                        case 37: //question 
                            AddToLog("-Basic commands:");
                            AddToLog("fire: fire gun at current angle");
                            AddToLog("scan: search for other tanks");
                            AddToLog("angle ###: move barrel to given angle");
                            AddToLog("turn ###: turn tank to given degree");
                            AddToLog("move (direction) ###: move north, south, east, or west as far as you give it");
                            AddToLog("retreat ###: move backwards ### away");
                            AddToLog("loot #: scavenge the remains of another tank using its tank number");
                            AddToLog("quit: quit game");
                            break;
						case 38: //change weapons
						case 39: //cartridge change
						case 40: //loaddifferentrounds
							AddToLog("-Budget cuts have resricted on weapon variety.");
							break;
						case 41: //allofthedirections
						case 42: //howtotanks
						case 43: //helpme
						case 44: //whysixpedals
						case 45: //whyonlyfourpedals
							AddToLog("-Woah, calm down there. Focus on the mission.");
							break;
						case 46: //fu
						case 47: //fo
						case 48: //ub
							AddToLog("-Professionalism please...");
							break;
						case 49: //fb
						case 50: //saa
							AddToLog("-Well, that's not very nice...");
							break;
						case 51: //volley
						case 52: //volleyfire
							AddToLog("-Pew pew pew...pew pew");
							break;
						case 53: //butthatschruch
							AddToLog("-Friendly fire on, target locked, firing main cannon.");
							break;
						case 54: //exitvehicle
							AddToLog("-I can't let you do that Dave...");
							break;
						case 55: //useobject
							AddToLog("-...You are in a tank, sir. You have only this tank.");
							break;
						case 56: //greetings
						case 57: //hi
							AddToLog("-um, hello? I'm not sure how to respond to that");
							break;
						default:
							AddToLog("-I'm sorry, I don't know what to do.");
							break;
					}
				}

				++index;
			}

			if (!matched)
			{
				AddToLog("-Order not recognized.");
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
			AddToLog("-Got it, the conflict is over.");
			Game.Instance.QuiteToMenu();
			StartCoroutine(Loading(0));
		}
		else
		{
			m_quiting = true;
			AddToLog("-Are we really leaving?");
		}
	}

	public void LogMessage(string msg)
	{
		AddToLog(msg);
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
