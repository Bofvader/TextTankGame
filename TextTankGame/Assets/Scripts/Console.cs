using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{
	[SerializeField] Text m_log = null;
	[SerializeField] Text m_input = null;

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			string input = m_input.text;

			if(input.Equals("Scan"))
			{
				//call player.scan
			}
			else
			{
				Regex aim = new Regex();

			}
		}
	}
}
