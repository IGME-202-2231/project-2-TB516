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

   - Wander while Flocking around scene. When an enemy plane is in range, start seeking it to attack.

### Formation

   - Wander fly around scene. Flocks with bombers, but wander force is more powerful so its not a super tight flock.

#### Steering Behaviors

- _List all behaviors used by this state_
   - Wander, Flock with other allied planes, Stay in bounds
- Obstacles - Flack puffs
- Seperation - Light seperation from other fighters to avoid overlapping
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   - When there are no enemy planes in combat range, this state is entered.
   
### Combat

**Objective:** Seek closest enemy plane to destroy it

#### Steering Behaviors

- _List all behaviors used by this state_
   - Pursue, Stay in bounds
- Obstacles - Flack puffs
- Seperation - Seperates from other fighters to avoid overlap
   
#### State Transistions

   - When an enemy plane enters combat radius, the fighter will enter combat state

## Bomber

   - Flocks with allied fighters and bombers. When in range of enemy fighters they will scatter and avoid all other planes

### Formation

**Objective:** Flock with allied fighters and each other to stay close to protection

#### Steering Behaviors

   - _List all behaviors used by this state_
      - Flock, Wander, Stay in bounds
   - Obstacles - Flack puffs
   - Seperation - Seperates from allied bombers to avoid overlap
   
#### State Transistions

   - When there are no enemy planes in combat radius, this state will be entered
   
### Scatter

**Objective:** Break formation and scatter/get away from all other planes. Evades closest enemy fighter.

#### Steering Behaviors

   - _List all behaviors used by this state_
      - Stay in bounds, Flee
   - Obstacles - Flack
   - Seperation - All other planes
   
#### State Transistions

   - When an enemy plane enters the combat radius, this state is entered

## Sources

   - Plane Sprites: https://opengameart.org/content/wwii-pixel-aircraft-sidescroller-sprites

## Make it Your Own

   - I would like to make it so the planes can attack/destroy each other.
   - (Finished) I made it so when 2 enemy planes collide, they will destroy each other. This is most common with the fighters pursuing an enemy. 

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

