|![](/Documentacion/URJCLogo.png)|<p>Grado en diseño y desarrollo de Videojuegos</p><p>Asignatura X</p>|![](/URJCLogo.png)|
| :- | :- | -: |

![](/Documentacion/URJCLogo.png)

Juegos en Web y Redes Sociales- Grupo C



![](/Documentacion/OongaBungaLogo.png)





CÉCILE LAURA BELLO DUPREZ

CHRISTIAN CAMPOS PAN

GONZALO GÓMEZ TEJEDOR

UMESH MOSTAJO SÁEZ

PAULA ROJO DE LA FUENTE

NATALIA MARTÍNEZ CLEMENTE

HÉCTOR MUÑOZ GÓMEZ





**Lista de actuaciones**

|Versión|Autores|Fecha|Comentarios|
| :- | :- | :- | :- |
|1\.0|CÉCILE LAURA BELLO DUPREZ<br>CHRISTIAN CAMPOS PAN<br>GONZALO GÓMEZ TEJEDOR<br>UMESH MOSTAJO SÁEZ<br>PAULA ROJO DE LA FUENTE<br>NATALIA MARTÍNEZ CLEMENTE<br>HÉCTOR MUÑOZ GÓMEZ|21/10/2023|Versión inicial |
|||||
|||||
|||||



**Índice**

[1. Introducción	3](#_toc214781176)

[1.1. Título	4](#_toc601682880)

[1.2. Concepto principal	4](#_toc1046704064)

[1.3. Características Principales	4](#_toc752207896)

[1.4. Género	5](#_toc1513914694)

[1.5. Propósito y Público Objetivo	5](#_toc670982555)

[1.6. Jugabilidad	5](#_toc1075237159)

[1.7. Estilo Visual	6](#_toc227034420)

[1.8. Alcance	7](#_toc2012679311)

[1.9. Plataforma	7](#_toc1220512801)

[1.10. Categoría	7](#_toc848083267)

[1.11. Licencia	7](#_toc608159600)

[2. Guion	8](#_toc693069025)

[3. Mecánicas del Juego	9](#_toc177702803)

[3.1. Mecánicas del Jugador	9](#_toc415360025)

[3.2. Mecánicas de Armas	12](#_toc826857768)

[3.3. Mecánicas de Niveles	14](#_toc749311310)

[3.4. Mecánicas de Enemigos	14](#_toc685249550)

[4. Estados del Juego	16](#_toc1883111045)

[5. Interfaz	17](#_toc333886849)

[6. Niveles	20](#_toc1599193190)

[7. Progreso del Juego	21](#_toc2016624543)

[8. Personajes	22](#_toc977677325)

[8.1. Mr. Bunny	22](#_toc756159978)

[8.2. Enemigos	22](#_toc159912798)

[8.3. NPCs	24](#_toc696081953)

[9. Ítems	24](#_toc2047340490)

[10. Logros	24](#_toc1975045804)

[11. Música y Sonidos	25](#_toc1138034068)

[12. Arte y Concept	26](#_toc988325685)

[13. Pensamiento Computacional	26](#_toc2028792690)

[14. Modelo de Negocio	27](#_toc1472038550)

[15. Miembros del Equipo	27](#_toc1193314690)

[16. Detalles de la Producción	27](#_toc828254028)

[17. Anexos	27](#_toc1725703381)


# <a name="_toc725972474"></a><a name="_toc914429665"></a><a name="_toc214781176"></a>**1. Introducción**
Este documento especificará todo el diseño del juego ‘Good Night Mr. Bunny. <a name="_int_es0zozod"></a>Los elementos de este GDD se han empezado a crear a partir de del 13 de septiembre de 2023 y en el quedarán registradas todas las acciones que se quieran llevar a cabo y todo el desarrollo del videojuego.

Las partes implicadas en el desarrollo de este documento son: Cécile Laura Bello Duprez, Christian Campos Pan, Gonzalo Gómez Tejedor, Umesh Mostajo Sáez, Paula Rojo de la fuente, Natalia Martínez Clemente y Héctor Muñoz Gómez.
## <a name="_toc125742117"></a><a name="_toc1686112907"></a><a name="_toc601682880"></a>**1.1. Título**
Good Night Mr. Bunny
## <a name="_toc1894095468"></a><a name="_toc1990951798"></a><a name="_toc1046704064"></a>**1.2. Concepto principal**
"Good Night Mr. Bunny" es un juego de terror en primera persona (FP) que sumerge a los jugadores en una experiencia única y aterradora. En el corazón de esta aventura se encuentra un conejo de peluche que cobra vida para proteger a un inocente niño de tan solo dos años de edad de los monstruos que emergen en la oscuridad de la noche. La trama se desarrolla en un escenario de pesadilla, donde el niño se enfrenta a sus miedos más profundos, y nuestro valiente protagonista, el jugador, se convierte en la última línea de defensa contra las criaturas que amenazan con arrastrar al niño a las tinieblas.

La energía y determinación del conejo de peluche, que ahora es controlado por el jugador, son fundamentales para salvaguardar la seguridad del niño y, al mismo tiempo, protegerse de los peligros que acechan en cada rincón de esta aterradora noche. Para lograrlo, el jugador contará con un variado arsenal de dispositivos y herramientas que podrán utilizarse estratégicamente para repeler y eliminar a los monstruos, creando así una experiencia de juego inmersiva y llena de suspense.

El juego no solo ofrece una experiencia de horror desafiante, sino que también fomenta el pensamiento computacional. Presenta un diseño en el que el jugador debe identificar patrones, evaluar el entorno y resolver problemas de manera efectiva para poder ganar la partida, gestionando las distintas herramientas a su disposición en función de los enemigos. <a name="_int_zfmzasau"></a>Esta característica añade una dimensión adicional de desafío y profundidad al juego, lo que lo convierte en una experiencia enriquecedora tanto en términos de entretenimiento como de desarrollo de habilidades cognitivas.
## <a name="_toc1194272903"></a><a name="_toc1907488920"></a><a name="_toc752207896"></a>**1.3. Características Principales**  
"Good Night Mr. Bunny" presenta una serie de características clave que ofrecen a los jugadores una experiencia de juego única y emocionante:

- **Arsenal de Defensa Variado:** Los jugadores tendrán a su disposición un variado arsenal, que incluye desde linternas para repeler monstruos hasta cohetes que explotan al chocarse con los enemigos. Estos recursos son esenciales para repeler a las criaturas y proteger al niño de las pesadillas que lo acechan.
- **Monstruos Aterradores:** Los jugadores se enfrentarán a una variedad de monstruos horripilantes, cada uno con comportamientos y habilidades únicas, lo que añade un nivel de desafío constante al juego.
- **Exploración y Decisiones Estratégicas:** Además de la acción intensa, el juego presenta elementos de exploración y toma de decisiones. Los jugadores deberán decidir entre superar el nivel mediante la búsqueda de interruptores o aguantar en el tiempo solucionando desafíos estratégicos en las habitaciones para poner fin a las pesadillas y avanzar en la trama.
- **Atmósfera Inmersiva:** El apartado artístico, los efectos de sonido envolventes y la iluminación atmosférica contribuyen a una experiencia de juego inmersiva que sumerge a los jugadores en el oscuro y espeluznante mundo del juego, aumentando la sensación de tensión.

Estas características principales se combinan para crear una experiencia de juego que desafiará a los jugadores a superar sus miedos mientras luchan por sobrevivir en "Good Night Mr. Bunny".
## <a name="_toc1573702885"></a><a name="_toc1181482884"></a><a name="_toc1513914694"></a>**1.4. Género**
"Good Night Mr. Bunny" es un juego que combina los géneros de terror y acción en un formato de primera persona (FPS), ofreciendo a los jugadores una experiencia intensa y emocionante.
## <a name="_toc1320777951"></a><a name="_toc30090800"></a><a name="_toc670982555"></a>**1.5. Propósito y Público Objetivo**
El objetivo principal de "Good Night Mr. Bunny" es proporcionar una experiencia de juego que fomente la identificación de patrones y la resolución de problemas mediante estrategias, haciendo uso de situaciones estresantes para potenciar una experiencia inmersiva, entretenida, desafiante y a la vez ligeramente aterradora.

El juego está diseñado para un público diverso, abarcando edades desde jóvenes hasta adultos, lo que permite que una amplia gama de jugadores disfrute de la experiencia. 

Es importante destacar que "Good Night Mr. Bunny" no es recomendado para personas que estén embarazadas ni para aquellos que tengan problemas cardíacos o condiciones médicas que puedan verse agravadas por el estrés y nerviosismo. La intensidad de la experiencia de juego puede no ser adecuada para estos grupos y, por lo tanto, se sugiere precaución al considerar jugarlo.
## <a name="_toc1027322460"></a><a name="_toc84882721"></a><a name="_toc1075237159"></a>**1.6. Jugabilidad**
"Good Night Mr. Bunny" ofrece una jugabilidad única en la que los jugadores se encuentran en la posición de defender a un niño en su cuna de los diversos monstruos que nacen de su imaginación. La habitación se convierte en un campo de batalla, y los jugadores deben hacer uso de un variado arsenal de armas y herramientas, mientras se enfrentan a las diferentes mecánicas de los monstruos. Cada tipo de monstruo presenta desafíos y tácticas únicas, lo que requiere que los jugadores adapten sus estrategias y utilicen sus recursos de manera inteligente para proteger al niño.

Además, la mecánica especial que añade una capa adicional de emoción al juego es la presencia de un gato hostil. A diferencia de los monstruos, el gato no se dirige al niño, sino al jugador. Los jugadores deberán utilizar tácticas como correr, distraer o esconderse para evitar al gato y esperar pacientemente hasta que decida marcharse de la habitación. Esta mecánica agrega una dimensión estratégica y tensa a la jugabilidad, ya que el jugador debe equilibrar la protección del niño con la necesidad de evitar al felino acechante.

El objetivo final del jugador en "Good Night Mr. Bunny" implica avanzar a través de diferentes niveles o noches, cada una más desafiante que la anterior. A medida que los jugadores progresan, se enfrentarán a una creciente variedad de monstruos y desafíos, lo que les brindará una sensación de logro y progresión en su lucha por sobrevivir a esta aterradora pesadilla.
## <a name="_toc1689632288"></a><a name="_toc1469354395"></a><a name="_toc227034420"></a>**1.7. Estilo Visual**
El estilo visual para el videojuego es un concepto 3D estilizado y semi-cartoon, más orientado a un aspecto oscuro y lúgubre, pero sin profundizar en este. Este estilo se distingue por su simplicidad y su enfoque meticuloso en los detalles, con un toque artístico que se crea a mano para dar vida al mundo del juego.

El estilo artístico es un estilo simplista estilizado a mano, con un aire alegre pero oscuro. La simplicidad estilizada a mano se combina con elementos alegres y oscuros en un equilibrio delicado que permite al juego fusionar los aspectos encantadores del mundo 'Cuqui' con la emoción del terror y el miedo, consiguiendo así una mezcla interesante que permite incluso a niños poder aventurarse en este juego.

<a name="_int_6an4aoku"></a>El estilo visual desempeña un papel fundamental en la narrativa y la jugabilidad. Los jugadores se verán inmersos en un mundo donde la estética única influye en su percepción de los personajes, los monstruos y el entorno. Esto, a su vez, contribuirá a la creación de una experiencia de juego memorable y única.

*Referencias artísticas*:

|![It Takes Two
](/Documentacion/ItTakesTwoRef1.jpeg)![It Takes Two for Nintendo Switch - Nintendo Official Site](/Documentacion/ItTakesTwoRef1.jpeg)|||
| :-: | :- | :- |
|<a name="img1"></a>[Ref 1](#ref1)|||
|![imagen diseñada por Christoffer Sjöström
https://www.artstation.com/artwork/oYOJq](/Documentacion/ZeldaRef.jpeg)<a name="img2"></a>[Ref 2](#ref2)|![Imagen diseñada por Richie Bassett.
https://www.artstation.com/artwork/6azkyw](/Documentacion/SceneRef.jpeg)<a name="img3"></a>[Ref 3](#ref3)|<p>![imagen creada por Hakan Aytac
https://www.artstation.com/artwork/5XdE1w](/Documentacion/SceneRef2.jpeg)</p><p><a name="img4"></a>[Ref 4](#ref4)</p>|

*Referencias del ambiente*:


|![Five Nights at Freddy´s': Tráiler español de la película - Aullidos.com](/Documentacion/FNAFRef.jpeg)<a name="img5"></a>[Ref 5](#ref5)|![/Documentacion/SONG OF HORROR COMPLETE EDITION en Steam](/SonOfHorrorRef.jpeg)<a name="img6"></a>[Ref 6](#ref6)|
| :-: | :-: |
## <a name="_toc1175683772"></a><a name="_toc1160216879"></a><a name="_toc2012679311"></a>**1.8. Alcance**
En cuanto al alcance de nuestro proyecto, "Good Night Mr. Bunny" se lanzarán una serie de niveles iniciales en una versión primeriza del juego de forma gratuita, y posteriormente una versión final de pago que ofrecerán una experiencia completa. La versión final estará sujeta a actualizaciones que recibirá de forma gratuita.

No está previsto el lanzamiento de DLCs a corto plazo. La visión principal es proporcionar a los jugadores un juego completo y enriquecedor que ofrezca horas de diversión y desafíos desde el momento en que se lance. Sin embargo, no se descarta la posibilidad de considerar contenido adicional en el futuro si surge una demanda significativa por parte de la comunidad de jugadores y si se alinea con la visión del juego. Cualquier expansión potencial se planificará cuidadosamente para mantener la coherencia y calidad del juego principal.
## <a name="_toc1871395672"></a><a name="_toc350029140"></a><a name="_toc1220512801"></a>**1.9. Plataforma**
Nuestro videojuego está diseñado para ser versátil en plataformas. La primera versión del juego se lanzará inicialmente para ser jugable en navegador web y dispositivos móviles, lo que brindará a los jugadores la flexibilidad de acceder a la experiencia de juego desde una variedad de dispositivos.

Para garantizar una jugabilidad óptima en el navegador web y dispositivos móviles, se realizarán adaptaciones técnicas y de diseño. Estas adaptaciones permitirán que los jugadores disfruten de una experiencia fluida y envolvente en estas plataformas, independientemente del dispositivo que utilicen.

En un futuro, se deja abierta la posibilidad de trasladar el videojuego a otras plataformas, lo que permitirá optar a un público aún más amplio y diversificado. Esta estrategia busca no solo mantener la relevancia del juego a largo plazo, sino también aprovechar nuevas oportunidades emergentes en el mercado de los videojuegos.
## <a name="_toc248024303"></a><a name="_toc591882244"></a><a name="_toc848083267"></a>**1.10. Categoría**

## <a name="_toc528725454"></a><a name="_toc29852745"></a><a name="_toc608159600"></a>**1.11. Licencia**

# <a name="_toc804474376"></a><a name="_toc1238918874"></a><a name="_toc693069025"></a>**2. Guion**
La trama del juego se reproducirá a través de una cinemática que se reproduce al iniciar una nueva partida.

La secuencia se inicia con un libro de cuentos cerrado, el cual se abre para presentar unas páginas ilustradas con dibujos que acompañan a la minimalista narración de la historia. A medida que las páginas van pasando (dando tiempo al jugador a asimilar la información) presentará los hechos y sumergirá al jugador en el punto de vista del personaje principal.

- **Primera Página:**  dibujo del niño en la cuna, arropado hasta los ojos temblando con sombras amenazadoras alrededor. 
  La imagen tiene un estilo de dibujo simplificado, recordando los estilos de libros ilustrados infantiles. [Ref 7](#img7) 
  El texto aparece en la parte inferior de la ilustración.

*“Érase una vez, un pequeño niño que no podía dormir...* 

*Unos terroríficos monstruos merodeaban por su habitación.”*

- **Segunda Página:** dibujo del niño abrazando al conejo de peluche.
  La imagen tiene un estilo de dibujo simplificado, recordando los estilos de libros ilustrados infantiles. [Ref 7](#img7) 
  El texto aparece en la parte inferior de la ilustración.

*“El pequeño se aferraba con fuerzas a su conejito de peluche:*

*‘Por favor, haz que los monstruos se vayan,’ rogaba.”*

- **Tercera Página:** dibujo del niño sorprendido y el conejo con las extremidades estiradas rodeado por un haz de luz.
  La imagen tiene un estilo de dibujo simplificado, recordando los estilos de libros ilustrados infantiles. [Ref 7](#img7) 
  El texto aparece en la parte inferior de la ilustración.

*“De repente, una fugaz luz iluminó la habitación y el peluche cobró vida según los deseos del niño.”*

- **Cuarta Página:** dibujo del conejo con un brazo en el pecho y dibujo de la primera imagen del gameplay.
  La imagen tiene un estilo de dibujo simplificado, recordando los estilos de libros ilustrados infantiles. [Ref 7
  ](#img7)El texto aparece en la parte inferior de la ilustración.

*“Y así, entendiendo su cometido, el conejo se armó de valor para enfrentarse a sus enemigos.* 

*La noche tan solo estaba empezando...”*

Una vez que se muestra la última página, la cámara se acerca a la última ilustración, que representa la pantalla durante el juego, y entra en la imagen, iniciando oficialmente el nivel.

|![Imagen](/Documentacion/DrawingStyleRef.jpeg)|<a name="img7"></a>[Img 7](#ref7)|
| :- | :- |
# <a name="_toc828811925"></a><a name="_toc7628429"></a><a name="_toc177702803"></a>**3. Mecánicas del Juego**
En los siguientes apartados se describen diversas las mecánicas que existen en el proyecto. Incluyendo las mecánicas del Jugador, de los diversos enemigos, de las armas y de los niveles.
## <a name="_toc415360025"></a>**3.1. Mecánicas del Jugador**
Los movimientos principales del jugador son los siguientes:

- **Desplazamiento:** a través de teclado, y palanca de control en móvil. El jugador se mueve en un entorno 3D en todas las direcciones posibles. En ordenador las teclas serán W A S D, y en móvil/dispositivo portátil será con un Joystick Izquierdo.

|![] (/Documentacion/MovementeBoc.jpeg)|
| :- |
|Boceto Conejo Caminando|

- **Correr:** pulsando/manteniendo una tecla hará que el movimiento del jugador se acelere. Esta aceleración será temporal y necesitará recargarse (estamina). 
  Utilizará el Shift en ordenador y en dispositivos móviles será una tecla a la derecha (*sujeto a cambios*)

|![](/Documentacion/MovementBoc2.jpeg)|
| :- |
|Boceto Conejo corriendo|

- **Saltar:** el jugador podrá realizar saltos para llegar a lugares elevados. 
  En PC se usará la Barra Espaciadora y en dispositivos móviles será otro botón al lado derecho de la pantalla.

|![](/Documentacion/MovementBoc3.jpeg)|
| :- |
|Boceto conejo saltando|

- **Interactuar con objetos:** a través de una tecla, el jugador puede coger o activar objetos. 
  En el PC será Clic Derecho del ratón. En dispositivos móviles será un botón que solo aparece cuando se puede interactuar con el objeto.

|![](/Documentacion/MovementBoc4.jpeg)|
| :- |
|Boceto interacción con elementos del escenario|

- **Apuntar/disparar:** En dispositivos móviles, esta mecánica está implementada mediante un joystick, el cual se encuentra a la derecha de la pantalla, con el que, al mantenerlo pulsado, permitirá atacar al objetivo, a la vez que permite ligeramente el movimiento de la pantalla para apuntar al enemigo. En PC el giro de cámara se realiza con el Puntero del Ratón, y el ataque se realiza con el Botón Izquierdo del Ratón.

|![](/Documentacion/MovementBoc5.jpeg)|
| :- |
|Boceto Conejo apuntando|

La cámara en el juego será en primera persona, permitiendo ver las extremidades del personaje parcialmente, pues estará limitada a ciertos ángulos. 

|![ref1](/Documentacion/ControlesEsquema.png)|<p>Controles del jugador con teclado y ratón</p><p><a name="img8"></a>[Img 8](#ref8)</p>|
| :-: | :-: |
|![](/Documentacion/ControlesEsquema2.png)|<p>Controles del jugador con móvil</p><p><a name="img9"></a>[Img 9](#ref9)</p>|

## <a name="_toc826857768"></a>**3.2. Mecánicas de Armas**
- **Escapar y Esconderse:** 

Existen ciertos enemigos que perseguirán directamente al jugador cuando le localicen. En estos casos, el jugador tendrá que buscar escondites o evitar al enemigo para que no le atrape. 

Las opciones para esconderse consisten en colocarse en lugares inaccesibles para el enemigo, como sitios elevados (muebles, encima de la cuna, …), o esconderse dentro de lugares estrechos (cubos, casitas de juguete, …). O simplemente mantenerse fuera de la vista del enemigo.

Otro método es usar elementos del escenario para distraer al enemigo, eso se puede conseguir recurriendo a las armas: lanzar el cohete en el suelo, enfocar la luz de la linterna en el suelo o las paredes, lanzar plastilina hacia diversos objetos, etc.

- **Linterna:** 

El jugador tendrá que apuntar a los monstruos que sean débiles frente a la luz para ahuyentarlos y/o matarlos.  Los enemigos más débiles frente a esta arma son las Sombras. 

La linterna se quedará sin batería progresivamente, y el jugador tendrá que ir a las estaciones de carga para poder recargarla y depositarla en esta. Mientras está cargando, el jugador puede hacer otras acciones. La linterna se puede recoger en cualquier momento independientemente de cuanta batería tenga. 

La luz de la linterna empezará a parpadear a medida que vaya bajando la batería, siendo un indicativo de la cantidad de batería restante. Empezando con un simple parpadeo cuando llega a la mitad de la batería. Al llegar a un cuarto de batería, parpadea cuatro veces. A partir de ese punto, la luz de la linterna pierde intensidad y se pone a parpadear con más frecuencia, parpadeando sin parar en los últimos segundos de la batería.

Luz In Game

Luz como se veria en la vida real
![](/Documentacion/LinternaMec1.png)

- **Cubo de Plastilina:** 

Se encuentran repartidos por el escenario y el jugador puede interactuar con ellos para recoger 6 bolas de plastilina que puede lanzar a los enemigos al mantener pulsado el botón de ataque. Cuanto más tiempo pulse el botón, más lejos llega el lanzamiento.

Estas bolas causan daño a los enemigos, matando de un golpe al Plancton, causan algún daño a Kitenstinger y apenas daña a la Sombra. Si golpean una superficie, como el suelo, se quedan pegadas y el jugador las puede usar como trampolín de un solo uso. Si se alcanza un número determinado de trampolines en el escenario, los más viejos irán desapareciendo para ser reemplazados con cada nuevo trampolín.

- **Cohete**: 

En la habitación hay varias estaciones de lanzamiento, generalmente en zonas altas. Desde estas estaciones, se puede apuntar (con libertad de ángulo prácticamente sin limitaciones) y lanzar un cohete en la dirección elegida. Una vez lanzado el cohete, tras un tiempo aparece uno nuevo en la estación.

Tras el lanzamiento, el cohete se precipita hacia la dirección indicada, donde chocará y explotará contra el primer elemento con el que se encuentre.

Esta arma derriba de un solo golpe al Kitenstinger. Su explosión sirve para dispersar y dañar a los grupos de Plancton.
## <a name="_toc749311310"></a>**3.3. Mecánicas de Niveles**
- **Aguantar:** 

El jugador tendrá que aguantar defendiendo al bebé hasta el amanecer, si lo consigue se supera el nivel. El tiempo de aguante está definido pero el jugador no lo podrá ver de manera directa. Se integrarán las horas dentro del juego mediante relojes esparcidos por la habitación. 

- **Interruptores:** 

Existirán varios interruptores esparcidos por la habitación, los cuales el jugador tendrá que accionar para activarlos y una vez estén todos activos, el nivel será superado. Estos interruptores aparecerán de manera aleatoria por la sala en la que esté jugando el jugador y cuando estén accionados emitirán una pequeña luz. No se puede interactuar con ellos hasta que no pasa un tiempo desde el inicio del nivel.
## <a name="_toc685249550"></a>**3.4. Mecánicas de Enemigos**
- <a name="_int_gzd4phiq"></a>**Comandante Blubs:** 

En ocasiones, un gato (denominado “<a name="_int_yzu4ymyh"></a>Comandante Blubs”) irrumpirá en la habitación donde se desarrolla el nivel. Dicho enemigo no consta de debilidades ni puede ser eliminado, por lo cual, la única opción del jugador será esconderse o distraerlo. Una vez que el gato atrapa al jugador, se pierde el nivel, pues se lo lleva fuera de la habitación. 

El gato no hace ruido al avanzar y la única forma en la que se avisa de su llegada es cuando araña la puerta de la habitación y entra, haciendo que entre algo de luz del pasillo. En ocasiones, al no encontrar al jugador, puede llevarse alguno de los juguetes que el jugador usa como arma, complicando un poco más el nivel.

- **Sombra Básica:** 

Aparece desde las paredes y se mueve flotando en línea recta hacia la cuna. Cuando el jugador lleva equipada una linterna siguen avanzando hacia la cuna, pero manteniéndose lejos del jugador y si puede ser fuera de su campo de visión. Cuando el jugador les ilumina se vuelven más rápidos e intentan moverse de forma errática para que sea más difícil apuntarles.

Si hay una linterna que se carga en una estación de carga y una sombra pasa lo suficientemente cerca, irá a esa estación para quitarle la carga a la linterna e impedir que cargue durante cierto tiempo, pero el jugador puede interrumpir a la sombra empleando las armas del escenario. Si el jugador posee una linterna no podrá realizar este comportamiento.

Al llegar junto a la cuna empieza a hacer daño al niño.

Es débil contra la luz de la linterna y tras acumular cierta cantidad de daño desaparecerá.

- **Plancton:** 

Es de tamaño reducido, aparecen de sitios estrechos y cerrados como los huecos de los muebles y se mueve en sprints cortos pero rápidos mientras se acerca a la cuna realizando un camino serpenteante. 

Si hay un interruptor encendido a cierta distancia, va hacia él e intenta apagarlo. Como necesitan ser al menos 3 para completar esta acción, llama a otros Plancton cercanos para ayudarle hasta sean suficientes en el interruptor, en cuyo caso se apaga y no puede volver a encenderse durante un periodo de tiempo. Esta acción solo se intenta llevar a cabo si hay suficientes Plancton en el escenario. 

Otro de sus comportamientos característicos es que intenta viajar en grupo con otros de su especie. 

Desaparece tras ser golpeado una vez por una bola de plastilina o acumular cierta cantidad de daño con otras armas.

Cuando llegan debajo de la cuna ya no pueden recibir daño y se van juntando formando un monstruo más grande. Cuando se juntan un gran numero el monstruo fusionado se completa y automáticamente se pierde la partida.

- **Mecánica Kitestinger:** 

Aparece desde las esquinas superiores de la habitación y se desplaza por el techo y las paredes acercándose a la cuna y lanzando trampas desde la boca que posee en la espalda, las cuales caen justo debajo de él. 

Al elegir la ruta, pasará justo por encima de ciertos puntos clave y objetos como los interruptores, cohetes, estaciones de recarga, cuna, armas que no se encuentran en posesión del jugador, Etc. Si un objeto se mueve de sitio elige otro punto por el que pasar y si el jugador se encuentra justo debajo lanzará automáticamente una trampa intentando atinar a este. Si ya hay cierto número de trampas en el escenario no pone más, y no ponen trampas cerca de otras existentes.

Las trampas permanecen inmóviles en su sitio hasta que se acerca el jugador cierta distancia, en ese momento sale de ellas una figura propulsada por un muelle que golpea al jugador y lo aturde durante un tiempo y le empuja.

Para atacar al niño ha de hallarse justo encima de este.

Desaparece tras ser golpeado una vez por un cohete o acumular gran cantidad de daño con otras armas.
# <a name="_toc1804154198"></a><a name="_toc1055988358"></a><a name="_toc1883111045"></a>**4. Estados del Juego**

![](/Documentacion/EstadosDelJuegoRef.png)

El juego se distribuye en los siguientes estados:

- **Inicio:** al iniciar el juego, el jugador debe escribir su edad y seleccionar su género. Después, debe escribir su nombre de usuario para poder iniciar sesión. Una vez realizado lo anterior, la pantalla se dirige al Menú Principal.
- **Menú Principal:** este menú es el que se abre después de iniciar el juego.  En este menú están contenidos el Menú de Ajustes, el Menú de Modo de Juego y el Menú de Salir del Juego.
- **Menú de Ajustes:** este menú contendrá cuatro pestañas por las que el jugador puede navegar: *Volumen*, *Gráficos*, *Controles* y *Enlaces*. En el *Volumen*, el jugador puede ajustar el nivel de audio del juego; en los *Gráficos*, el jugador ajustar los gráficos del juego, la resolución de las texturas y el tamaño de pantalla; en el menú de *Controles* se mostrará una lista con todos los controles que el jugador puede realizar, y en el menú *Enlaces* se mostrará una lista de los enlaces de la empresa y el juego.
- **Menú de Seleccionar Modo:** este menú solo tendrá disponible partida normal en un principio (más adelante se plantea crear un modo infinito). 

Tras seleccionar el modo de juego, se podrá pasar a Jugar una nueva partida o a continuar una partida ya existente. 

- **Continuar Partida:** El juego se indicia desde el último punto de guardado, este punto de guardado es el inicio de la noche en la que se encuentre.
- **Nueva Partida:** El jugador tendrá que seleccionar la dificultad del nivel. Al principio tendrá solo disponible el modo de dificultad normal. Tras esto saltará a la cinemática inicial y cuando finalice se iniciará la escena del juego.
- **Escena de Juego:** Durante la partida el jugador se moverá por el escenario, interactuando con objetos y enemigos de manera constante. De este modo de juego el jugador puede abrir el menú de pausa.
  - **Menú de Pausa:** Este menú permite entrar a los ajustes del juego, salir al menú principal, guardar partida y volver a la pantalla de juego.** 
    Cuando se abre el menú de pausa, el tiempo de juego se para.
# <a name="_toc1017628324"></a><a name="_toc1766648702"></a><a name="_toc333886849"></a>**5. Interfaz**
**Controles dispositivos móviles:**

![](/Documentacion/ControlesEsquema2.png)
[Img 8](#ref8)

**Controles teclado:**

![ref4](/Documentacion/ControlesEsquema.png)
[Img 9](#ref9)

**Menú inicial (Concept básico, sujeto a cambios):**

![](/Documentacion/InterfazTitulo.png)

**Menú ajustes**

![](/Documentacion/InterfazAjustes1.png)

![](/Documentacion/InterfazAjustes2.png)

![](/Documentacion/InterfazAjustes3.png)

![](/Documentacion/InterfazAjustes4.png)

**In game**

|![](/InterfazInGame1.png)|<p>Pantalla general</p><p></p>|
| :- | :-: |
|![](/Documentacion/InterfazInGame2.png)|<p>Coger objetos</p><p></p>|
|![](/Documentacion/InterfazInGame3.png)|<p>Sprint (solo aparece cuando se corre)</p><p></p>|
**Créditos**

![](/Documentacion/InterfazCreditos.png)
# <a name="_toc2047301626"></a><a name="_toc1617721293"></a><a name="_toc1599193190"></a>**6. Niveles** 
Este juego se compone de varios Escenarios, y cada escenario consta de varios niveles.

El primer Escenario sería el nivel introductorio, el cual se desarrolla en la habitación de un niño pequeño, donde el jugador tendrá que proteger durante el transcurso de una noche. Este escenario consta de varias noches de supervivencia. Para este escenario habrá cuatro noches o niveles. 
En el escenario habrá repartidos elementos grandes por los que el jugador puede subirse y tener una mejor panorámica del escenario. También habrá objetos medianos y pequeños que entorpecerán su camino y objetos medianos que servirán para esconderse de algunos enemigos. Los objetos interactivos (interruptores) aparecerán de manera aleatoria entre una serie de puntos estratégicos establecidos. Las armas se encuentran repartidas por el escenario de manera fija, es decir, siempre aparecen en el mismo sitio dependiendo de la disposición de los objetos del escenario. 
Para que tenga variabilidad y no sea siempre el mismo escenario, habrá variaciones de este mismo, de este modo se aumenta la variabilidad del juego. 

![](/Documentacion/Niveles1Mapa.jpeg)![](/Niveles1Mapa2.jpeg)![](Niveles1Mapa3.jpeg)

El siguiente nivel se desarrolla en el salón de la casa, siguiendo la misma mecánica de supervivencia anteriormente descrita. El nivel será un poco más grande que el anterior, implementando nuevos modelos que el jugador podrá usar para visionar el campo de visión, esconderse e interactuar.  Seguirá la misma mecánica de los Interruptores repartidos aleatoriamente entre una serie de puntos establecidos y las armas aparecerán siempre en el mismo sitio dependiendo de la disposición de los elementos del escenario.

El tercer nivel se desarrolla en la guardería, siguiendo las mismas mecánicas de supervivencia anteriores. A diferencia de los anteriores este escenario abarcará más de una sala, pero las mecánicas y el funcionamiento seguirá los mismos pasos que los anteriores. Además, en este nivel se deberá de proteger no solo a un NPC, sino a varios.
# <a name="_toc810378806"></a><a name="_toc1595767318"></a><a name="_toc2016624543"></a>**7. Progreso del Juego**
Para superar la etapa o nivel el jugador deberá sobrevivir varias noches seguidas.
Una vez has sobrevivido a todas las noches de ese nivel, saltará una cinemática introductoria que desbloqueará el siguiente Escenario.

El siguiente escenario funcionará de una manera similar, teniendo que superar varias noches o niveles de manera sucesiva hasta llegar a la última noche.

Para aumentar la dificultad de los niveles de manera progresiva, se actualizará la cantidad de enemigos que pueden aparecer en el escenario, aumentando en cada nivel. En el cambio de escenarios, al ser este más grande, el jugador tendrá que cubrir más terreno y gestionar mejor el uso de las armas y aprender a moverse por los nuevos escenarios, lo que suma un plus de dificultad.

Escenario X/Stage X

Nivel 1

Nivel X

Nivel final

Escenario X+1/Stage X+1

![](/Documentacion/EsquemaProgresoDelJuego.png)
# <a name="_toc227637051"></a><a name="_toc81940192"></a><a name="_toc977677325"></a>**8. Personajes**
## <a name="_toc756159978"></a>**8.1. Mr. Bunny**
El jugador encarnará a un peluche con forma de conejo, el cual tendrá la misión de proteger a su dueño de los diversos monstruos que lo amenazan.

\*Extender ficha de personaje

## <a name="_toc159912798"></a>**8.2. Enemigos**
Los enemigos son una representación de los miedos de un niño, como pueden ser sombras, monstruos, bichos..., así como la encarnación de problemas en casa y con sus padres.

**8.2.1. Sombra / Clownado / Cypher / Styx / Zanybell**

Es una sombra que consiste en una cabeza flotante con forma de bufón. Este monstruo representa el miedo que tienen los niños cuando se les acerca demasiado un extraño. El aspecto de la sombra hace referencia a los payasos (Joker, IT) ya que son personajes que pueden provocan miedo en los niños.  

![](/Documentacion/Enemigo1.png)

\*Añadir descripciones, objetivos, mostrar imagen, etc.

**8.2.2. Plancton**

Es de aspecto similar al conejo de peluche, pero destrozado. Representa el miedo a que las cosas preciadas se rompan y a los seres erráticos e impredecibles.

\*Añadir descripciones, objetivos, mostrar imagen, etc.

**8.2.3. Esqueleto**

Es el resultado de la fusión de gran cantidad de plancton debajo de la cuna. Su origen es lo desconocido que se encuentra justo en un punto ciego para el niño, haciendo alusión al miedo del “monstruo de debajo de la cama”.

\*Añadir descripciones, objetivos, mostrar imagen, etc.

Debajo de la cama.

**8.2.4. Kitestinger**

Es una especie de insecto que trepa por el techo, con una boca dentada en la espalda, una cola similar a la de un escorpión y forma romboidal, haciendo alusión a la forma de una cometa de juguete. Este monstruo representa el temor que se puede transmitir en los niños las fobias y prejuicios de los padres como es el miedo a los insectos. 

\*Añadir descripciones, objetivos, mostrar imagen, etc.

**8.2.5. Comandante Blubs**

Es un gato de aspecto rechoncho, peludo y de mal carácter perteneciente a la familia del niño. Es el archienemigo del conejo de peluche dado que este, debido a su curiosidad felina, intenta darle caza; por lo que es amenazador desde el punto de vista del jugador.

\*Añadir descripciones, objetivos, mostrar imagen, etc.
## <a name="_toc696081953"></a>**8.3. NPCs**
El niño que tendrá que proteger el jugador. Hijo de X y <a name="_int_ythybroe"></a>Y, con 2 años de edad ... y sus padres (si los padres no aparecen en el juego, no son NPCs, son solo parte del lore).
# <a name="_toc1505542881"></a><a name="_toc722633991"></a><a name="_toc2047340490"></a>**9. Ítems**
\*Añadir ítems como pilas o armas como la linterna

**9.1. Linterna**

Este es el arma básica del juego y puede dejarse apoyada en el suelo en cualquier momento. Esta arma inflige daño a los enemigos cuando los alcanza con el foco de la luz. Tiene más poder sobre las sobre las sombras. Para recargar la linterna, hay que colocarla en una plataforma de carga, las cuales están distribuidas por el mapa.

**9.2. Plastilina**

La plastilina es un arma que consiste en lanzar bolas de plastilina a los enemigos, se obtiene y recarga a partir del cubo de plastilina grande. Cuando sueltas este objeto la munición restante desaparece. Es más efectivo contra el plancton, al cual fulmina con un solo impacto.

**9.3. Cohete**

El cohete se encuentra estático en una plataforma de disparo en el mapa. Para utilizarlo el jugador ha de ir hasta esta plataforma, apuntar el cohete al objetivo y accionar el interruptor de lanzamiento. <a name="_int_umidmxnu"></a>Al cabo de un tiempo, la plataforma suministra un nuevo cohete. Derrota de un solo golpe al kitestinger.

**9.4. Plataforma de carga de linterna**

**9.5. Cubo de plastilina grande**


\*Añadir descripciones, definir mecánicas, definir estadísticas, mostrar imágenes, etc.


# <a name="_toc2029620357"></a><a name="_toc582927498"></a><a name="_toc1975045804"></a>**10. Logros**
A lo largo de los niveles, el jugador podrá conseguir diversos logros. Estos no proporcionan elementos del juego ni mejoras, simplemente son unas curiosidades para coleccionar por el jugador.

- **Cazador de Pesadillas:** Elimina 50 monstruos de pesadillas en una sola partida.
- **Mal Defensor:** Pierde el nivel sin haber matado ni un solo monstruo.
- **Defensor de Sueños:** Protege al niño durante 10 minutos sin dejar que ningún monstruo de pesadilla lo toque.
- **Pesadillas Exorcizadas:** Completa el juego en dificultad pesadilla.
- **Cazador de Sueños:** Recoge todos los objetos coleccionables a lo largo del juego. **(¿Patos de goma, atrapasueños?)**
- **Perfeccionista:** Sobrevive a un nivel sin recibir daño de los monstruos.
- **Cazado:** El gato te ha cazado.
- **Fuera Gatito:** El gato te persigue, pero no logra atraparte.
- **Hacia la Luna y más allá:** Súbete en el Cohete y sal disparado con él.
- **Falso Despegue:** Haz que el Cohete se estrelle en el suelo.
- **Houston, we have a problem:** Kitenstinger esquiva la explosión del cohete por un centímetro o menos.
- **Mini Infarto:** Activa una trampa de Kitenstinger.
- **Infarto Mortal:** Activa 10 trampas de Kitenstinger en una misma partida.
- **Corazón Inmune:** Activa 20 trampas de Kitenstinger en una misma partida.
- **Super Trampolín:** Lanza las 5 bolas de plastilina en un mismo lugar y salta en el conjunto de trampolines generados.
- **Lluvia de Arañas:** Derriba a Kitenstinger lanzando únicamente bolas de plastilina.
- **Apagón:** Los Plánctones logran apagar un interruptor.
- **Por los Pelos:** Acaba el nivel cuando sólo falta un Plancton para el monstruo de debajo de la cuna.
# <a name="_toc705505507"></a><a name="_toc2005323703"></a><a name="_toc1138034068"></a>**11. Música y Sonidos**
Música:

- Menú principal
  - Canción principal del juego
  - Efectos de puerta abriéndose (No constante, de manera discontinua), añade profundidad
- In Game (escenario 1):
  - Música principal:
    - Canción del cuarto, dsfaja
  - Efectos de sonido:
    - Apretura de la puerta. (indicador de cuando este enemigo entra en la habitación)
    - Ronroneo del gato (indicador de posición del gato, no es constante)
    - Arañazos del gato en paredes sutiles (indicador de posición del gato, no es constante)
    - Risa tenebrosa suave (indicador de que Sombra está en la habitación, no es constante)
    - Sonido de pasos consecutivos (Efecto de sonido Kitestinger, indica cuando está cerca del jugador en la habitación)
    - Risitas de niños (Efecto de sonido que suena cuando un Plancton aparece, indicando al jugador que tiene que tener cuidado porque se pueden acumular). Este efecto tiene que parecer que es lejano.
- In Game (escenario 2)
- In Game (escenario 3)
- Game over:
- Superar nivel: Sonido que indique que la mañana ha llegado. Puede ser un gallo junto con un sonido cálido

Efectos de Sonido:

- Selección: efecto de sonido al pulsar un botón en los menús logging, principal, pausa y ajustes
- Caminar: sonido de pasos sobre madera que no sean muy duros, pues un peluche no pesa. Se asigna al jugador cuando anda.
- Correr: sonido de pasos rápidos sobre madera que no sean duros. Se asigna al jugador cuando corre.
- Bebé: efecto de bebé llorando. Cuando la vida del bebé está cerca de cero, comenzará a llorar, indicando al jugador que tenga cuidado con los enemigos.
- Interruptores: switch On / Off. Sonido que se escucha cuando los interruptores se encienden y apagan. Es distinto para el encendido y el apagado.
- Botón de linterna: sonido de pulsador de linterna cuando el jugador enciende y apaga la linterna.
- Lanzar/colocar objeto: sonido de caída cuando un objeto colisiona con una pared o suelo.
# <a name="_toc26394627"></a><a name="_toc1819926170"></a><a name="_toc988325685"></a>**12. Arte y Concept**
![https://cdn.discordapp.com/attachments/491436482565898250/1151602479218315474/1070087967294631976-326cde8c-f781-454e-bae2-587d64824cea-628058.2000000002.png](Aspose.Words.50e33a12-fb26-42cc-8a91-5a584cf8cc6d.038.png)   Primer esquema Escena ’Habitación Niño’

\*Añadir imágenes o separar el documento y hacer uno artístico

Las paredes de la habitación están decoradas con nubes sobre un fondo azul. 

# <a name="_toc2028792690"></a>**13. Pensamiento Computacional**
Página y media mínimo.

Destrezas del juego:

- Descomposición
- Evaluación
- Análisis de Datos ¿?
- Generalización ¿?
# <a name="_toc1472038550"></a>**14. Modelo de Negocio**
Añadir modelo de negocio como el modelo de lienzo o canvas, caja de herramientas y mapas de empatía. \*REQUISITO OBLIGATORIO

# <a name="_toc123113920"></a><a name="_toc166221834"></a><a name="_toc1193314690"></a>**15. Miembros del Equipo**

|CÉCILE LAURA BELLO DUPREZ|2D, Interfaces y 3D|
| -: | - |
|CHRISTIAN CAMPOS PAN|Programación|
|GONZALO GÓMEZ TEJEDOR|Programación y sonido|
|NATALIA MARTÍNEZ CLEMENTE|3D y texturas|
|UMESH MOSTAJO SAEZ|2D, interfaces y texturas|
|HECTOR MUÑOZ GÓMEZ|3D y Programación|
|PAULA ROJO DE LA FUENTE|3D y sonido|
# <a name="_toc793122760"></a><a name="_toc168511990"></a><a name="_toc828254028"></a>**16. Detalles de la Producción**
# <a name="_toc1231427144"></a><a name="_toc1684499129"></a><a name="_toc1725703381"></a>**17. Anexos**
VERSION DE UNITY 2022.3.2f1

Cuadro de Referencias

|<a name="ref1"></a>[Ref 1](#img1)|It Takes Two for Nintendo Switch - Nintendo Official Site|
| :- | :- |
|<a name="ref2"></a>[Ref 2](#img2)|imagen diseñada por Christoffer Sjöström<br>[Link al trabajo en artstation de Christoffer Sjöström](https://www.artstation.com/artwork/oYOJq)|
|<a name="ref3"></a>[Ref 3](#img3)|Imagen diseñada por Richie Bassett<br>[Link al trabajo en artstation de Richie Bassett](https://www.artstation.com/artwork/6azkyw)|
|<a name="ref4"></a>[Ref 4](#img4)|imagen creada por Hakan Aytac<br>[Link al trabajo en artsation de Hakan Aytac](https://www.artstation.com/artwork/5XdE1w)|
|<a name="ref5"></a>[Ref 5](#img5)|Portada de la película 'Five Nights at Freddy´s' <br>[Página oficial del creador](http://Scottgames.com)      <br>[Página Fandom FANF](https://five-nights-at-freddys-world.fandom.com/wiki/Scottgames.com)|
|<a name="ref6"></a>[Ref 6](#img6)|Imagen del juego “Song Of Horror”<br>[Enlace a la página de los desarrolladores del Juego, "Protocol Games"](http://www.protocolgames.com/)|
|<a name="ref7"></a>[Ref 7](#img7)|<a name="marcador1"></a>Imagen referencia del estilo visual de las ilustraciones de la cinemática inicial<br>Imagen creada por el usuario @nin0318 en Twitter/X/Instagram<br>[nin.0318 Instagram](https://instagram.com/nin.0318?igshid=MzRlODBiNWFlZA==)|
|<a name="ref8"></a>[Ref 8](#img8)|Controles del jugador con teclado y ratón|
|<a name="ref9"></a>[Ref 9](#img9)|Controles del jugador con dispositivo móvil|

||29|
| :-: | -: |

[ref1]: Aspose.Words.50e33a12-fb26-42cc-8a91-5a584cf8cc6d.018.png
[ref2]: Aspose.Words.50e33a12-fb26-42cc-8a91-5a584cf8cc6d.019.png "Insertando imagen..."
[ref3]: Aspose.Words.50e33a12-fb26-42cc-8a91-5a584cf8cc6d.022.png "Insertando imagen..."
[ref4]: Aspose.Words.50e33a12-fb26-42cc-8a91-5a584cf8cc6d.023.png
