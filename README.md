Since you mentioned enjoying Don't Drink and Dive — and because it’s a game I’m particularly proud of — I’ve included the scripts for the harpoon and rock-dragging mechanics, as well as the one controlling the first boss (McBandito Pike).

All of these were written during the one-week jam. I’ve only cleaned up unused code, and while I know some areas could be better structured or optimized, I chose to leave the code as-is to give you a realistic sense of how I work under pressure. It’s very much “make it work, make it bug-free, move on” — the kind of approach necessary in a jam but which I understand needs refinement in a studio setting. I’ve added a few comments to highlight what I’d improve in a more formal context.

I’ve also included the GameManager singleton from The Secret Scoffer of Saffron Walden — a personal favourite that placed 6th out of 521 in GitHub’s GameOff. It shows how I approach saving/loading (in this case via PlayerPrefs, which was sufficient for the jam, though I’d use JSON or a more robust method in production).

In short, while this may read as “jam code,” I’m fully aware of the best practices I’m bypassing here. My jam approach prioritizes polish, stability, and rapid feature delivery. In a studio context, my focus would be clean, modular, and maintainable code from the outset.

Looking forward to hearing your thoughts!
but I assure you that I know the rules of best practice that I am breaking here. 
When writing this code it was for my eyes only, and I always code in game jams in such a way that the game is bug free, 
and plays cleanly and fluidly, and if the code is fully functional in that sense then I leave it as is. 
I felt it would be dishonest of me to make it 'studio friendly' before sending it over for your review. 
If employed by Grendel, my first priority would be for the code to be readable, modular and clear as well as bug free and fully functional.
