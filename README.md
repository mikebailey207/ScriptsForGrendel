Since you mentioned enjoying Don't Drink and Dive — and because it’s a game I’m particularly proud of — I’ve included the scripts for the harpoon and rock-dragging mechanics, as well as the one controlling the first boss (McBandito Pike).

These were all written during the week-long jam, and I haven’t changed anything since submission, aside from removing unused code. 
I know there are parts that could be structured or optimized better, and I’ve deleted comments from the jam and added comments specific to what I would change if coding in a studio setting, 
but I thought it would be more honest to share them as they are to show how I work under pressure. 
It is very much 'it works and it is bug free so on to the next feature' kind of coding, that would be neater and better optimized in a studio setting.

I have also added the GameManager singleton from The Secret Scoffer of Saffron Walden. 
My personal favourite of the games I've made (that finished 6th out of 521 entries in the GitHub GameOff). 
This is just to show that I use singletons, which I obviously reference from other scripts, and also how I (in this case) handle saving and loading. 
Here I am using PlayerPrefs as it was sufficient for the purposes of the jam. 
I am aware that it would be more robust to use something like json in a more formal setting to store and load this info. 
Again, this would be far better optimized if not in a game jam setting where I was rushing to add as many features as possible in a short amount of time, 
and I have just left it as it was when the jam was submitted. 

In summary then, I am aware that this may scream 'jam code', 
but I assure you that I know the rules of best practice that I am breaking here. 
When writing this code it was for my eyes only, and I always code in game jams in such a way that the game is bug free, 
and plays cleanly and fluidly, and if the code is fully functional in that sense then I leave it as is. 
I felt it would be dishonest of me to make it 'studio friendly' before sending it over for your review. 
If employed by Grendel, my first priority would be for the code to be readable, modular and clear as well as bug free and fully functional.
