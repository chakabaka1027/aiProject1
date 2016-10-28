Welcome to our Mario Pathfinder!

This project utilizes A* pathfinding to navigate an NPC agent, Bowser, around the map to a target destination, Peach. It is the player's job as Mario to reach Peach before Bowser.
This project also utilizes a parser that reads an ansProlog generated txt file that creates levels with defined specifications.

For our players:
WASD moves and jumps Mario around the level.
The right mouse button places Peach, the A* goal node, into the level.
The space bar summons Bowser to chase Peach.

For developers interested in how this all works:
Our worlds are parsed through a txt file located in the resources folder of Unity. This text file is called "Level 1".
The LevelManager script parses the txt file based off of preset delimiter characters into several arrays. The level is curated by accessing the integers that are parsed from the strings of these arrays.
From there, an invisible grid is overlayed over the world to create the A* navigation space. Each cell in the grid corresponds to a tile sprite in the game as well as a node containing the information from the Node script.
The AStar script uses the grid script to see the search space, understanding which tiles are traversable and which are not. It runs through a loop, beginning with the starting node (the cell Bowser begins at), and searches neighboring nodes, defines Fcosts, and adds and eliminates nodes from an open list and a closed hashset.
Once the path is found, it is retraced by looping back through each parent and child node. This path directs Bowser through the level.

There is also a PathRequestManager script that creates a queue of A* calls, in case players decide to use multiple Bowser nav agents. This was employed because in most games, there will be multiple agents and thus using a queue is necessary to not overtax the machine with A* calls all at once.