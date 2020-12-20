Praxis
======

Praxis is a chess engine written in C#. The project is a console application whose executable you would select using a chess interface, such as [Arena](http://www.playwitharena.de/).

Roadmap
-------
The engine does function in its current state but is not complete.

  - Enhance valid move discovery, such as not attempting to castle if a space in-between can be attacked.
  - Expand promotion logic. Right now, always assuming promotion to queen.
  - Add local redundant tablebase and/or incorporate a deeper one. Right now, a 5-piece tablebase is used via a web service, but larger ones exist.
  - Expand opening selection. Right now, first move of the engine is always e2e4 and from there will follow any opening it can use from the ECO.
  - Develop mid-game move evaluation. Right now, a random legal move is selected.
  - Implement additional communication protocol. Right now, only UCI has been started, but developing a [CECP](https://www.chessprogramming.org/Chess_Engine_Communication_Protocol) (Winboard) protocol would make the engine compatible with more interfaces.
