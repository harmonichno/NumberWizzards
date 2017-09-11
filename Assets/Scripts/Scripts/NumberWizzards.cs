using UnityEngine;
using System.Collections;
using System;

public class NumberWizzards : MonoBehaviour 
{	
	Boolean isStartedGame;
	Boolean isLowestNumberChoosed;

	Boolean isHighestNumberChoosed;
	Boolean isHighestNumberTextShown;

	Boolean isGuessCountChoosed;
	Boolean isGuessCountTextShown;

	int max;
	int min;
	int guess;
	int guessCount;

	string inputText; 	// input text from console
	string numberType; // highest or lowest number ?
	
	float maxTime;		// Can get input only every 1/2 sec. 
	float currentTime;
	
	void Start () 
	{
		StartGame();
	}
	
	void StartGame() 
	{
	    isStartedGame = false;
		isLowestNumberChoosed = false;

		isHighestNumberChoosed = false;
		isHighestNumberTextShown = false;

		isGuessCountChoosed = false;
		isGuessCountTextShown = false;

		guessCount = 5;

		// Can get input only every 1/10 sec. 
		maxTime = 0.1f;
		// How passed time since last input ?
		currentTime = 0.0f; 
																																			
		print ("======================================");
		print ("Welcome to Number Wizzards ! ");
		print ("======================================");
		print ("Pick a number in your head, bit do not tell me it");	
		print ("Up arrow = higher, down = lower, return = equal");
		print ("Type lowest and highest number ! ");

		PrintCurrentNumber();
	}
	
	void Update () 
	{
		if(isStartedGame) 
		{
			bool isInput = false;
			if(Input.GetKeyDown(KeyCode.UpArrow)) 
			{
				min = guess;
				isInput = true;
				guessCount--;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow)) 
			{
				max = guess;
				isInput = true;
				guessCount--;
			}
			else if (Input.GetKeyDown(KeyCode.Return)) 
			{
				print ("I won !");
				RestartGame();
			}	

			if(guessCount == 0)
			{
				print ("I lose !");
				RestartGame();
			}
			else if(isInput)
			{
				NextGuess();
			}
		} 
		else 
		{
			ChoiceLowestNumber();
			if(isLowestNumberChoosed) 
			{
				if(!isHighestNumberTextShown) 
				{
					PrintCurrentNumber();
					isHighestNumberTextShown = true;
				}

				ChoiceHighestNumber();
			}

			if(isLowestNumberChoosed && isHighestNumberChoosed && !isGuessCountChoosed) 
			{
				if(!isGuessCountTextShown)
				{
					print("Choose guess try's count: ");
					isGuessCountTextShown = true;
				}

				ChoiceGuessNumber();
				if(isGuessCountChoosed)
				{
					isStartedGame = true;
					NextGuess();
				}
			}
		}
	}
	
	// --------------------------
	// Game functions
	// --------------------------
	void RestartGame()
	{
		isStartedGame = false;
		isLowestNumberChoosed = false;
		
		isHighestNumberChoosed = false;
		isHighestNumberTextShown = false;
		
		isGuessCountChoosed = false;
		isGuessCountTextShown = false;
		
		guessCount = 5;
		
		// Can get input only every 1/10 sec. 
		maxTime = 0.1f;
		// How passed time since last input ?
		currentTime = 0.0f; 
		
		print ("======================================");
		print ("===============RESTART================");
		print ("======================================");
		print ("Pick a number in your head, bit do not tell me it");	
		print ("Type lowest and highest number ! ");
		
		PrintCurrentNumber();
	}

	void PrintCurrentNumber() 
	{
		if (!isLowestNumberChoosed || !isHighestNumberChoosed) 
		{
			numberType = isLowestNumberChoosed ? "highest" : "lowest";
			print ("Current " + numberType + " number: " + inputText);
		} 
		else 
		{
			print ("Current guess count number: " + inputText);
		}
	}

	void NextGuess() 
	{
		guess = (max + min) / 2;
		print ("Higher or lower that " + guess);			
		print ("Up arrow = higher, down = lower, return = equal");
	}
	
	void ChoiceLowestNumber() 
	{
		// If have not chosen yet
		if(!isLowestNumberChoosed) 
		{
			// Read chars from keyboard
			if(ReadChars()) 
			{
				int value;
				// Convert from string to int
				if(int.TryParse(inputText, out value)) 
				{
					if(value > 0)
					{
						min = value;
						print ("The lowest number you can pisk is " + min);
						isLowestNumberChoosed = true;
					}
					else
					{
						print ("Choice another lowest number, please.");
						print ("Reason: give me only positive integer numbers !");
					}
				}
				else 
				{
					print ("Choice another lowest number, please !");	
					print ("Reason: give me only Integer numbers !");				
				}
				inputText = "";
			}
		}
	}
	
	void ChoiceHighestNumber() 
	{
		// If have not chosen yet 
		if(!isHighestNumberChoosed) 
		{
			// Try to parse entered number
			if(ReadChars()) 
			{
				int value;
				// Convert from string to int
				if(int.TryParse(inputText, out value)) 
				{
					if(value > min && value > 0) 
					{
						max = value;
						print ("The highest number you can pisk is " + max);
						isHighestNumberChoosed = true;
						max += 1; // divison's result is rounded to the lower part, hence we can get 999 instead 1000.  
					}
					else 
					{
						print ("Choice another highest number, please.");
						print ("Reason: max value is lower than min value AND only positive integers !");
					}
				}
				else 
				{
					print ("Choice another highest number, please.");		
					print ("Reason: give me only Integer numbers !");
				}
				inputText = "";
			}
		}	
	}

	void ChoiceGuessNumber() 
	{
		// If have not chosen yet 
		if(!isGuessCountChoosed) 
		{
			// Try to parse entered number
			if(ReadChars()) 
			{
				int value;
				// Convert from string to int
				if(int.TryParse(inputText, out value)) 
				{			
					if(value > 0) 
					{
						guessCount = value;
						print ("PC can try to guess: " + guessCount);
						isGuessCountChoosed = true;
					}
					else 
					{
						print ("Choice another guess count number, please.");
						print ("Reason: give me only positive integer numbers !");
					}
				}
				else 
				{
					print ("Choice another guess count number, please.");		
					print ("Reason: give me only Integer numbers !");
				}
				inputText = "";
			}
		}	
	}

	// ----------------------------
	// Utiltiy functions
	// ----------------------------
	
	Boolean ReadChars() 
	{
		bool result = false;
		currentTime += Time.deltaTime;

		if(currentTime >= maxTime) 
		{
			foreach(char c in Input.inputString) 
			{
				// Check that the key is not a backspace
				if(c != "\b"[0]) 
				{ 
					// If return was pressed
					if ((c == "\n"[0] || c == "\r"[0])) 
					{ 
						// Player has entered number => return true
						result = true;
					}
					else 
					{
						// If anything else was pressed
						inputText += c;   
					}
					PrintCurrentNumber();
					currentTime = 0.0f;
				}
			}
		}
		return result;
	}
}