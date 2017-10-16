using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// In TippingPoint, a player must respond to squares on either side of a line.
/// The player responds with a left or right arrow press. Pressing the key that corresponds to the wrong side is a fail.
/// Each trial has a defined delay as to when the next square will appear as well as a defined side to determine what side it will show up on.
/// </summary>
public class TippingPoint: GameBase
{
	const string INSTRUCTIONS = "Press <color=cyan>Left Arrow</color> or <color=cyan>Right Arrow</color> as soon as the square appears in the appropriate side.";
	const string FINISHED = "FINISHED!";
	const string RESPONSE_GUESS = "No Guessing!";
	const string RESPONSE_CORRECT = "Good!";
	const string RESPONSE_TIMEOUT = "Missed it!";
	const string RESPONSE_SLOW = "Too Slow!";
	const string RESPONSE_KEY = "Wrong Key!";
	Color RESPONSE_COLOR_GOOD = Color.green;
	Color RESPONSE_COLOR_BAD = Color.red;
	Vector3 leftPosition = new Vector3(-70.0f, 0.0f, 0.0f);
	Vector3 rightPosition = new Vector3(70.0f, 0.0f, 0.0f);
	KeyCode keyPressed; //Would have implemented this as a parameter into one of the AddResult methods with more time. Added here as to not break the Addresult(Trial, float) override.

	/// <summary>
	/// A reference to the UI canvas so we can instantiate the feedback text.
	/// </summary>
	public GameObject uiCanvas;
	/// <summary>
	/// The parent object holding TippingPoint game elements.
	/// </summary>
	public GameObject gameParent;
	/// <summary>
	/// The object that will be displayed briefly to the player.
	/// </summary>
	public GameObject stimulus;
	/// <summary>
	/// The line which stimulus rectangles will cross.
	/// </summary>
	public GameObject line;
	/// <summary>
	/// A prefab for an animated text label that appears when a trial fails/succeeds.
	/// </summary>
	public GameObject feedbackTextPrefab;
	/// <summary>
	/// The instructions text label.
	/// </summary>
	public Text instructionsText;


	/// <summary>
	/// Called when the game session has started.
	/// </summary>
	public override GameBase StartSession(TextAsset sessionFile)
	{
		base.StartSession(sessionFile);

		instructionsText.text = INSTRUCTIONS;
		gameParent.SetActive(true); //Enable the game's parent so that the elements show up

		StartCoroutine(RunTrials(SessionData));

		return this;
	}


	/// <summary>
	/// Iterates through all the trials, and calls the appropriate Start/End/Finished events.
	/// </summary>
	protected virtual IEnumerator RunTrials(SessionData data)
	{
		foreach (Trial t in data.trials)
		{
			StartTrial(t);
			yield return StartCoroutine(DisplayStimulus((TippingPointTrial)t));
			EndTrial(t);
		}
		stimulus.SetActive(false); //Make sure to turn off stimulus
		FinishedSession();
		yield break;
	}


	/// <summary>
	/// Displays the Stimulus on the left or right of a line for a specified duration.
	/// During that duration the player needs to respond as quickly as possible.
	/// </summary>
	protected virtual IEnumerator DisplayStimulus(TippingPointTrial t)
	{
		GameObject stim = stimulus;
		stim.SetActive(false);

		//Left position
		if(t.side == "l")
		{
			stim.GetComponent<RectTransform>().localPosition = leftPosition;
		}
		else //Right position
		{
			stim.GetComponent<RectTransform>().localPosition = rightPosition;
		}

		yield return new WaitForSeconds(t.delay);

		StartInput();
		stim.SetActive(true);

		yield return new WaitForSeconds(t.duration);

		EndInput();

		yield break;
	}


	/// <summary>
	/// Called when the game session is finished.
	/// e.g. All session trials have been completed.
	/// </summary>
	protected override void FinishedSession()
	{
		base.FinishedSession();
		instructionsText.text = FINISHED;
	}


	/// <summary>
	/// Called when the player makes a response during a Trial.
	/// StartInput needs to be called for this to execute, or override the function.
	/// </summary>
	public override void PlayerResponded(KeyCode key, float time)
	{
		if (!listenForInput)
		{
			return;
		}
		base.PlayerResponded(key, time);
		EndInput();
		keyPressed = key;
		AddResult(CurrentTrial, time);
	}


	/// <summary>
	/// Adds a result to the SessionData for the given trial.
	/// </summary>
	protected override void AddResult(Trial t, float time)
	{
		TippingPointTrialResult r = new TippingPointTrialResult(t);
		r.responseTime = time;
		if (time == 0)
		{
			// No response.
			DisplayFeedback(RESPONSE_TIMEOUT, RESPONSE_COLOR_BAD);
			GUILog.Log("Fail! No response!");
		}
		else
		{
			if (IsGuessResponse(time))
			{
				// Responded before the guess limit, aka guessed.
				//Check if it was also the wrong key.
				if(IsCorrectKey((TippingPointTrial)t))
				{
					r.keyCorrect = true;
				}
				DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
				GUILog.Log("Fail! Guess response! responseTime = {0}", time);
			}
			else if (IsValidResponse(time))
			{
				//Check if correct key was pressed
				if(IsCorrectKey((TippingPointTrial)t))
				{
					// Responded correctly.
					DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
					r.success = true;
					r.keyCorrect = true;
					r.accuracy = GetAccuracy(t, time);
					GUILog.Log("Success! responseTime = {0}", time);
				}
				else
				{
					//With wrong key.
					DisplayFeedback(RESPONSE_KEY, RESPONSE_COLOR_BAD);
					GUILog.Log("Fail! Wrong Key Pressed! key pressed = " + keyPressed, time);
				}
			}
			else
			{
				// Responded too slow.
				DisplayFeedback(RESPONSE_SLOW, RESPONSE_COLOR_BAD);
				GUILog.Log("Fail! Slow response! responseTime = {0}", time);
			}
		}
		sessionData.results.Add(r);
	}


	/// <summary>
	/// Display visual feedback on whether the trial has been responded to correctly or incorrectly.
	/// </summary>
	private void DisplayFeedback(string text, Color color)
	{
		GameObject g = Instantiate(feedbackTextPrefab);
		g.transform.SetParent(uiCanvas.transform);
		g.transform.localPosition = feedbackTextPrefab.transform.localPosition;
		Text t = g.GetComponent<Text>();
		t.text = text;
		t.color = color;
	}


	/// <summary>
	/// Returns the players response accuracy.
	/// The perfect accuracy would be 1, most inaccuracy is 0.
	/// </summary>
	protected float GetAccuracy(Trial t, float time)
	{
		TippingPointData data = sessionData.gameData as TippingPointData;
		bool hasResponseTimeLimit =  data.ResponseTimeLimit > 0;

		float rTime = time - data.GuessTimeLimit;
		float totalTimeWindow = hasResponseTimeLimit ? 
			data.ResponseTimeLimit : (t as TippingPointTrial).duration;

		return 1f - (rTime / (totalTimeWindow - data.GuessTimeLimit));
	}


	/// <summary>
	/// Returns True if the given response time is considered a guess.
	/// </summary>
	protected bool IsGuessResponse(float time)
	{
		TippingPointData data = sessionData.gameData as TippingPointData;
		return data.GuessTimeLimit > 0 && time < data.GuessTimeLimit;
	}


	/// <summary>
	/// Returns True if the given response time is considered valid.
	/// </summary>
	protected bool IsValidResponse(float time)
	{
		TippingPointData data = sessionData.gameData as TippingPointData;
		return data.ResponseTimeLimit <= 0 || time < data.ResponseTimeLimit;
	}

	protected bool IsCorrectKey(TippingPointTrial t)
	{
		//Check if proper arrow was hit with proper side
		if(t.side == "l" && keyPressed == KeyCode.LeftArrow)
		{
			return true;
		}
		
		if(t.side == "r" && keyPressed == KeyCode.RightArrow)
		{
			return true;
		}

		return false;
	}
}
