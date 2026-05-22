# Bits & Bots
<p align="center">
<img width="100%" height="auto" alt="2026_05_11_0it_Kleki" src="https://github.com/user-attachments/assets/6b4c41cf-7e67-46c3-a282-0f1728d2073f" />
</p>
Een game met match 3 als input waar je robots moet upgraden om tegen vijanden te vechten
---

## Wiki
* [Agile Planning](https://github.com/TimothyKuijper/bits-and-bots/wiki/Agile-Planning)
* [Asset Conventions](https://github.com/TimothyKuijper/bits-and-bots/wiki/Asset-Conventions)
* [Code Conventions](https://github.com/TimothyKuijper/bits-and-bots/wiki/Code-Conventions)
* [Code Documentation](https://github.com/TimothyKuijper/bits-and-bots/wiki/Code-Documentation)
* [Functioneel Ontwerp](https://github.com/TimothyKuijper/bits-and-bots/wiki/Functioneel-Ontwerp)
* [Gitflow (versiebeheer conventies)](https://github.com/TimothyKuijper/bits-and-bots/wiki/Gitflow)
* [Notulen](https://github.com/TimothyKuijper/bits-and-bots/wiki/Notulen)
* [Pipeline](https://github.com/TimothyKuijper/bits-and-bots/wiki/Pipeline)
* [Technisch Ontwerp](https://github.com/TimothyKuijper/bits-and-bots/wiki/Technisch-Ontwerp)
* [User Tests](https://github.com/TimothyKuijper/bits-and-bots/wiki/User-Tests)

## Uitleg
De klant wilt een match 3 game waar het matchen niet de game is, maar de input. Voor de rest hebben wij complete creatieve vrijheid gekregen en is er al snel één concept naar voren gekomen die de klant leuk vind:

In Bits & Bots speel je als de eigenaar van een robot die door middel van match 3 mechanics de robot kan laten vechten tegen steeds sterkere tegenstanders, en vervolgens de robot kan upgraden.

Voor een complete en uitgebreide beschrijving over het functioneel ontwerp ga naar de [wiki](https://github.com/TimothyKuijper/bits-and-bots/wiki)

Game loop:
Game loop cycles through the garage (build mode) and stadium (fight mode), in the garage you can repair broken parts and roll for new parts which gives you a random type of part with random stats based on your level in-game (how many fights you won), in here you can also customise your parts (mind you these do not affect gameplay), in the stadium you will fight progessively stronger enemies, thus needing to use the garage to roll for better parts


# Rollen

| Rol          | Naam | Beschrijving |
|--------------|--------|-------------|
| Artist / Product Owner | Ahmet     | Communicatie met de klant, en maakt art assets o.a. concepten en de arena |
| Developer / Lead Dev     | Timothy   | Checkt of code zich houdt aan de Code Conventies voordat MR's gemerged worden en maakt de match 3 mechanic |
| Artist / Lead Art     | Cicerio   | Let op dat alle models en textures correct in Unity staan en maakt art assets o.a. de speler robot en de match 3 frame |
| Developer / Scrum Master | Ferron    | Schrijft en checkt de Trello board, Noteert de Standup en Standdowns, maakt ook supporting gameplay mechanics |
| Artist       | Renzo     | Maken van art assets o.a. de tiles en background art |
| Artist       | Delainy   | Maken van art assets o.a. de enemy robot |
| Developer    | Noah      | Maakt systemen voor gameplay o.a. de part system en UI |
| Developer    | Teffer    | Maakt mechanics voor gameplay o.a. de rank system |

# Highlighted Game Onderdelen
> De andere features en mechanics zijn te lezen in de [wiki](https://github.com/TimothyKuijper/bits-and-bots/wiki/)

## Match 3

De match 3 is **hoe** de speler terug vecht om de [battle bar](https://github.com/TimothyKuijper/bits-and-bots/wiki/Technisch-Ontwerp#battle-bar) te duwen naar de tegenstander hun kant. Wanneer de speler een match van 3 of meer zelfde icons krijgt duwt de speler de bar terug op basis van hoeveel schade alle [onderdelen](https://github.com/TimothyKuijper/bits-and-bots/wiki/Technisch-Ontwerp#part-system) bij elkaar doen.

<img width="50%" height="auto" alt="20260520-1342-07 8917050" src="https://github.com/user-attachments/assets/622b4d15-65de-46d2-b9bb-aec7d36b6112" />

# Battle Bar

De battle bar is het hart van het gevecht, en geeft een visuele manier om te zien of je wint of verliest. De battle bar begint altijd op 50, en tijdens het gevecht maakt de speler matches die dit getal verhoogd, en de vijand valt aan om dit getal te verlagen. Het gevecht wordt beslist door wie als eerste de bar vol krijgt, voor de speler is dat 100, en voor de vijand 0. 

<img width="456" height="146" alt="20260517-2055-11 4837186" src="https://github.com/user-attachments/assets/c3992707-c6f4-46b0-abe9-3f34518b2bdc" />

## Arena Camera

De arena camera zorgt voor dynamische poses iedere keer als de speler aanvalt, door de camera naar een willekeurig punt in een lijst de lerpen, dit geeft ook meer impact aan de acties van de speler

<img width="400" height="406" alt="ezgif-2c9a8234f8586dde" src="https://github.com/user-attachments/assets/dd9a6dd6-4910-47cd-ac6f-8d1f46d9a16b" />

## Part System

Het onderdelen systeem is verantwoordelijk om de speler te verbeteren buiten hun ervaring. Dit zorgt ervoor dat de spelers altijd betere onderdelen zullen halen om bijvoorbeeld een grotere voorsprong te hebben op vijanden _of_ meer zekerheid hebben van hun kracht op lange termijn.

<img width="252" height="446" alt="Bezig met opnemen 2026-05-22 111532" src="https://github.com/user-attachments/assets/ab55f1d2-7e40-4f94-99fa-9c05e92e465a" />


