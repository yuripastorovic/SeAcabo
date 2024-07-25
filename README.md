# Se Acabó! for Android
![This is an alt text.](/image/sample.png "This is a sample image.")

Desarrollo de una Aplicación Móvil en Unity pata Android basada en el juego de mesa Time's Up Party

## Descripción General:
Se trata del desarrollo de una aplicación móvil que adapta la jugabilidad del juego de mesa Time's Up Party. Esta destinada a la plataforma Android, esta desarrollada en Unity en C#.

## Objetivos del proyecto:

### Proporcionar una reversión en Andriod a Time's Up Party:
Hace años Time's Up Party contaba con una versión Android e IOs, pero a día de hoy esa versión no se encuentra disponible.

Mi objetivo es volver a crear una version Android del juego, con nuevas mecánicas. No es un proyecto para lanzar en la PlayStore, se trata de un proyecto personal. 
### Consolidar y expandir conociemtos:
Otro objetivo perseguido con este proyecto es consolidar y expandir conociemtos ya adquiridos en Desarrollo en Unity. Así como aprender a trabajar con MongoDB.

## Motivación para el Proyecto:
Juagar con mis amigos a Time's Up Party es algo que hacemos frecuentemente, pero solo podiamos jugar cuando entre ellos habia un usuario de IOs, ya que no existe versión Android en la actualidad. Ante esta necesidad decidí comenzar el desarrollo de la aplicación.


Otro punto a tener en cuenta es siempre he sido muy creativo, y no me ha costado empezar un proyectos cuando algo en el mercado no me gustaba o sabia que podía ser mejor.

## Actualización sobre el Estado Actual (Agosto 2024):
Este sería el cronograma del proyecto.



| -              | Estado          | Started         | Last Update   | Ended         |
| -------------  |:-------------:  |:-------------:  |:-------------:|:-------------:|
| Vistas         | Funcional       | Julio 2024      | Julio 2024    | En progrseo   |
| Lógica         | Terminado       | Julio 2024      | Julio 2024    | En progrseo   |
| BBDD           | Problemas en version Android | Julio 2024 | Julio 2024| En progrseo |
| Jugabilidad    | Terminado       | Julio 2024      | Julio 2024    | En progrseo   |

La base de datos del proyecto utilizará MongoDB.

## Enfoque en Nuevas Mecánicas:
### Partidas dinámicas:
Durante las sesiones de juego la partida puede volverse aburrida con turnos lentos o reglas muy estaticas. Con la revisión a la jugabilidad de Times's Up Party  se busca crear sesiones de juego mas dinámicas, mas divertidas y que inviten a jugar más de una.

* Durante la primera ronda el juego si deja pasar a la siguiente carta, pero penaliza con 15s.
* Durante la ultima ronda el jugador podrá cantar o tararear tambien.

### Amplia Variedad de posiblidades:
La versión de IOs, es gartuita e incluye una 120 tarjetas por lo que en 3 sesiones han salido la mayoria de las cartas y partir de esta se empiezan a repetir. La intención es crear un amplio catalogo de cartas, crear hasta 500 cartas diferentes que creen sesiones de juego más largas y disfrutables. 

Para ello las cartas utilizadas en una sesión de juego seran apartadas del resto para evitar la repetición de estas. Una vez cerrada la aplicacion estas volveran con las demas.

### Localización, actualización y personalización:
En la versión de IOs, la mayoria de las cartas se refierena personajes que estan "anticuados" o que no son conocidos en España. En esta versión las cartas estarán localizadas en España y serán de personajes actuales.

* Xokas (streamer), Marta Diez (influencer), Kiko Ribera...

Las cartas serán de una categoría y el jugador decidirá si quiere jugar con esa categoría o no. 

* Futbolistas, Anime, Famosos...

### Feedback de los usuarios:
Tras haber finalizado el desarrollo de Time's Up Party for Android, el siguiente paso es el desarrollo de una pagina web de la aplicación. En esta los usuarios podrían votar semanalmente por un personaje de interes y si la consulta es mayoritariamente a favor, el personaje entrará en el juego.

El personaje sería la persona de la semana que más visitas haya generado en la web Wikipedia 


## Tecnologías Utilizadas:
El proyecto esta Desarrolado en:
* Unity: Como motor de videojuegos.
* C#: como lenguaje de programacion.
* MongoDB: Sistema gestor de Base de datos, para actualizar las cartas.