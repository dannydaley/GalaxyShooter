This is the repo for my COMP140 project

Link to controller video - https://www.youtube.com/watch?v=haNFTTyyAVk&ab_channel=DannyDaley

Repo Link - https://gamesgit.falmouth.ac.uk/scm/~dd252935/galaxyshooter.git

My project game is an endless runner where the player must navigate through space avoiding asteroids

The game controller uses two ultrasonic sensors for controlling in-game flight, two potentiometers for 
controlling output power for the in-game lazers with reacting LEDs, and two buttons to fire the left and right lazers.

The housing for my controller is made from polymer clay, I used this because it was readily available, easy to work with 
and very pliable and durable.

Games keyboard controls are arrow keys for movement,
left and right shift for left and right fire,
NumPad 1 = decrease left power output
NumPad 7 = increase left power output
NumPad 3 = decrease right power output
NumPad 9 = increase right power output
esc to pause



the uduino sketch I used was sourced from - https://marcteyssier.com/uduino/projects/ultrasonic-sensor-hc-sr04-on-unity
which is a guide to getting ultrasonic sensors to work, I made my own adjustments as the script was initially designed for one sensor
and resulting in crashes within unity.