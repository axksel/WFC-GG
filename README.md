# WFC-GG
Wave Function Collapse and Growing Neural Grids combined! The main branch is cleaned up version  of our implementation without all the extra things we have worked on. The project was made for our master thesis. The master thesis can be found [here](https://www.researchgate.net/publication/334416222_Expanding_Wave_Function_Collapse_with_Growing_Grids_for_Procedural_Content_Generation_Project_Expanding_Wave_Function_Collapse_with_Growing_Grids_for_Procedural_Content_Generation).

#### Modulesets.
The modulesets are all based on marching squares. A really good introduction can be found [here](https://www.boristhebrave.com/2013/07/14/tileset-roundup/).

#### Growing Grid
The growing grid implementation is based on Dr. Bernd Fritzkes [work](https://www.demogng.de/) .
The adjustable parameters is largely the same as in his implementation and a explanation can found at the demo website.

#### Instructions
There are three different modulesets included.
- 3dModuleSet. This set contains a city moduleset made for three dimmensions. The bool "Corner Setup Clockwise" should be false when using this set.

- HandDrawnset. This is a two dimmensional set of a handrawn city. The Grid Y should be set to 1 and the "Corner Setup Clockwise" should be true.

- modules. This is the original set and is a 2.5 dimmensional cavern set. The Grid Y should be set to 1 and the "Corner Setup Clockwise" should be true.

#### Made by
- [Tobias]( https://github.com/Tobiasnm)
- [Jonas]( https://github.com/axksel)
