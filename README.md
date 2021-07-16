# StreamLabsLoLWatcher

This app is used **for people who stream League of Legends with StreamLabs OBS** : it displays a very ugly popup to remember to switch scenes in StreamLabs OBS, so that you don't stream your client but the game. You make the popup disappear instantly by using your scene shortcut.

The code is opensource : [you can read it here](https://github.com/HowTommy/StreamLabsLoLWatcher/)

![ui](https://github.com/HowTommy/StreamLabsLoLWatcher/blob/main/1.png?raw=true)

![ui2](https://github.com/HowTommy/StreamLabsLoLWatcher/blob/main/2.png?raw=true)

You need to setup keyboard shortcuts for your scenes to use it.

The release of the v0.1 is here : [Release v0.1](https://github.com/HowTommy/StreamLabsLoLWatcher/blob/main/Releases/setup.exe?raw=true)

Few notes :
* It works only when StreamLabs is up
* Only CTRL key modifier seems to work
* The focus is automatically set on the popup, you just have to type your shortcut to make it disappear
* There is a 5 seconds timer refresh for the check of the window
* If streamlabs is not launched, it only checks every 1 min if you launch it

For the process, you must pick "League of Legends" with the game launched. The others are RIOT anti-cheat or the client.

It's a tiny tiny app, created in 1 hour, just to have a safety net for when I stream.

I hope that StreamLabs will someday implement the automatic switch of scenes.

Enjoy!
