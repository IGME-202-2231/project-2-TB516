# Project _NAME_

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Thomas Berrios
-   Section: 3

## Simulation Design

 - The simulation will be of planes and jets flying around in a battle.

### Controls

-   _List all of the actions the player can have in your simulation_
      - The player input would be launching a flak puff into the fight. It would be controlled by the mouse. If a plane/jet is hit by a flack puff, it would be destroyed.

## Fighter

   - Wander/Flock around scene. When an enemy plane is in range, start seeking it to attack.

### Formation

   - Idle fly around scene. Stays near bomber flock but can wander around more freely.

#### Steering Behaviors

- _List all behaviors used by this state_
   - Wander, Flock with other allied planes
- Obstacles - Avoids flack puffs
- Seperation - None
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   - When the plane is out of combat range of other enemy planes, this state is entered.
   - When this agent gets in range of enemy planes, it will exit Search
   
### Combat

**Objective:** Seek closest enemy plane to destroy it

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - Flack puffs, enemy projectiles
- Seperation - Seperates from all enemy planes except its target
   
#### State Transistions

   - When an enemy plane enters combat radius, the fighter will enter combat state

## Bomber

   - Flocks with allied fighters and bombers. When in range of enemy fighters they will scatter and avoid all other planes

### Formation

**Objective:** Flock with allied fighters and each other to stay close to protection

#### Steering Behaviors

   - _List all behaviors used by this state_
   - Obstacles - Flack puffs
   - Seperation - Seperates from enemy planes while flocking
   
#### State Transistions

   - When there are no enemy planes in combat radius, this state will be entered
   
### Scatter

**Objective:** Break formation and scatter/get away from all other planes

#### Steering Behaviors

   - _List all behaviors used by this state_
   - Obstacles - Flack, enemy bullets
   - Seperation - All other planes
   
#### State Transistions

   - When an enemy plane enters the combat radius, this state is entered

## Sources

   - https://opengameart.org/content/wwii-pixel-aircraft-sidescroller-sprites

## Make it Your Own

- _List out what you added to your game to make it different for you_
- _If you will add more agents or states make sure to list here and add it to the documention above_
- _If you will add your own assets make sure to list it here and add it to the Sources section

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

