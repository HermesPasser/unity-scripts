# Novelist
A very simple precedual visual novel engine  

See the demo [bizarre novel test](https://github.com/HermesPasser/unity-scripts/releases/download/1/unity-scripts.zip).

### About choice system

This is a very simple visual novel system and all choices are boolen viables based, there is not cumulative values and they reset each time the choices are called. If your game needs have long-term consequences choices then modify this system or use another.

### Documentation

**nameText** *» A string[]*  
Set a text for the name ladel.  
Set blank for a empty text.  
e.g: `nameText Name here.`  

**labelText** *» A string[]*  
Set a text for the dialog ladel.   
Set blank for a empty text.  
e.g: `labelText Your text here.` 

**backgroundImage** » A number of images[] 
Set a background image.  
Set nullValue for remove a image and define with background.  
e.g: `backgroundImage 0` 

**characterImage** *»  A number of images[], A string of character position (left, middle, right)*  
Show and set a image for the left character.  
Set nullValue for remove and hide a image.  
e.g: `characterImage 0 middle`  

**soundMusic** *» A number of sounds[], a boolean Play In Loop*  
Set a sound music.  
Set nullValue for stop and remove.  
e.g: `soundMusic 0 true`  

**soundSFX** *» A number of sounds[], a boolean Play In Loop*  
Set a sound effect.  
Set nullValue for stop and remove.  
e.g: `soundSFX 0 false`  

**Interface** *» a boolean Is Enabled*  
Enable and disable the labels of interface.  
Set nullValue for stop and remove.  
e.g: `Interface true`  

**answerText** *» A int index of choices[] (default 1-3), a string choice text*  
Set a text for an answer.  
e.g: `answer 1 false`  

**callChoice** *» A int of choice numbers to show (default 1-3)*  
Show the choice list.  
e.g: `callChoice 3`  

**ifChoiceThenLoadScene** *» A int index of choices[] (default 1-3), a string of scene name*  
Loads a scene if the condition is true. Use if you want to make the system choices with the results in different scenes. Recommended for games that have many choices.  
See "About choice system".  
e.g: `ifChoiceThenLoadScene 1 2`  

**ifChoiceThenJump** *» A int index of choices[] (default 1-3), a int index of storyboard[]*  
Skips to the script index if the index that has been selected is equal to the indicece defined in this statement.  
Use if you want to make the whole system of choices consequences on a storyboard or for smaller choices.
See "About choice system".  
e.g:
				[5]ifChoiceThenJump 1 7
				[6]ifChoiceThenJump 2 9
				
				This will execute if line 5 is true
				[7]labelText Tell something
				[8]endGame
				
				This executes if line 6 is true
				[9]labelText Tell other thing
				[8]endGame


**endGame**
Finish the game.  
Currently unfunctional.  
e.g: `endGame`
