# Stock Exchange simulator/video game
## About

Random project i've been working over the years. The core idea was to simulate some sort of stock market and make a game out of it.
However this was already too complex, and i even wanted to make this for a final project for a coding school i went to in 2017.
Due to my rather poor coding skills when i started work on this, there have been atleast 3 refractors,
and i still consider the codebase to be hard to understand, but it has atleast allowed me to understand what i wrote.

I only choose windows forms because that's all i knew back then, so im just keep using it. However it does serve to refresh my c# knowledge once in a while.

Tho clearly the project has evolved way past such initial ideas, now basically simulating many types of actors who try to act in their own manner.
I use this project to test the complexities of AI and economics (which i dont understand at all and for some reason wanted to make this as a game).


## Simulation systems
```
-Companies exist and produce goods, which are bought by other companies or a single block "population".
-Good price is determined by companies, depending on how much goods were sold last turn
-Goods exist only in sale listings done by companies. 
-Money flows in a closed system, starting off in population. Currently amount of money a company can hold is hard-caped
-Separate "humans" exist, who buy shares in companies and can also invest in new companies
-Banks can lend money to "humans" and companies for investment
-Each actor acts in it's own best interest and does not coordinate
```
## Plans for future
### Major
```
-UI improvements, full info
-Price changes more organically, based on demand, resource flexability, price difference from base price
-Make share buying/selling work like resoure listings
-Implement actual loan system
```
### Minor
```
-Finish converting IO system to JSON
-Make classes hold references to other components instead of string IDs
-Buying resources should be done more equally(less blocking based on price)
```
## Getting Started

The whole project is being worked in Visual studio and is compiled with the help of it.

### Prerequisites

```
Windows forms(through visual studio)
Newtonsoft.Json(NuGet)
```
