# Unity Exercise 

Clone this Github project and expand it by adding a new game to it.

The project is made in Unity 5.3

It implements a very simple game called React, which briefly displays a stimulus (rectangle) and the player has to respond as quickly as possible.

The codebase is structured so that more complex games can be built on top of it easily and its behavior can be controlled using an xml parameter file (which we call the session file).
See the 'Overview' section below for more details on the structure of the project.


Once you're familiar with the game and how the code is organized, you will be implementing one new game in this project.
In this game you should be able to:

- Specify in the session file whether the stimulus (rectangle) position per trial is random or predefined.
  - The predefined position should be defined inside the session file on a per trial basis. 
  - While the random position should be generated based off a defined range.
- Specify in the session file whether the stimulus should sometimes appear red, in which case the player should NOT respond in order to get a correct response.
- Save all the new game parameters (position, isRed, etc...) when creating a session log file at the end of a game.
- Log important events such as the result of each Trial to a trace file by utilizing the GUILog functionality that the project has, instead of Unity's Debug.Log


# Things to keep in mind

- Treat this exercise as a real world scenario where we ask you to add a new game to our existing project.
- The original React game should remain unchanged.
- The new code should maintain the formatting conventions of the original code.


# Project Overview

- **Stimulus** - An event that a player has to respond to.
- **Session** - A session refers to an entire playthrough of a game.
- **Trial** - A trial is when a player has to respond to a stimulus, which becomes marked as a success or failure depending on the player's response.
- **TrialResult** - A result contains data for how the player responded during a Trial.
- **Session File** - A session file contains all the Trials that will be played during a session, as well as any additional variables that allow us to control and customize the game.
- **Session Log** - An xml log file generated at the end of a session, contains all the attributes defined in the source Session file as well as all the Trial results that were generated during the game session.
- **Trace Log** - A text log file generated using GUILog for debugging and analytical purposes. GUILog requires a SaveLog() function to be called at the end of a session in order for the log to be saved.
- **GameController** - Tracks all the possible game options and selects a defined game to be played at the start of the application.
- **InputController** - Checks for player input and sends an event to the Active game that may be assigned.
- **GUILog** - A trace file logging solution, similar to Unity's Debug.Log, except this one creates a unique log file in the application's starting location.
- **GameBase** - The base class for all games.
- **GameData** - A base class, used for storing game specific data.
- **GameType** - Used to distinguish to which game a Session file belongs to.


# Submission

For your submission, extend this README documenting the rules of the new game, how the code works, how scoring works in the new game, and any other interesting or useful things you can think of for us to take into consideration. Then zip the git repository and send it to us.

# The Game: Tipping Point by William Montgomery
Tipping Point is also a quick reaction game, but instead has the player deciding what side of a line the stimulant is on.
Player's hit the left or right arrow to pick the side as the square appears.

# How It Works
The code works by using a side attribute that can be chosen in the xml.
That determines the postion of the square and will also be visible in log files.
Also a KeyCorrect method was written since multiple keys would now be valid as an answer, but only one was actually correct.

# Things I Would Have Added and Other Notes
Due to the following: 
* Midterm Season in full swing
* A shortened deadline by a week because of another offer I'm considering
* Not hearing back for a few days about whether or not I could submit some other Unity projects instead of this one
* Coming down with a cold

I learned how to use this codebase and wrote this game in less than a day.
You can view when I forked the original project and when my commits where to see that this is true.
I would've preferred to not be in that situation since I had other exciting ideas about what to create for this project, but I still at least have something to show.
Things I would've liked to do:
* Add the possibilty for stimulant squares to be placed on sides for all arrow keys.
* Add occasional red squares that would have asked the player to hit the opposite side.
* Add sounds for correct and incorrect choices.
I believe I still made a game that perfectly qualifies for all of the requirements above, but I would've liked to do more with more time.
