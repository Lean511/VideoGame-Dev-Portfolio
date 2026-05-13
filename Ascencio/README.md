# Ascension: Lógica de Ajedrez y Sistemas de Tablero

 Este proyecto es un motor de ajedrez funcional desarrollado en Unity, enfocado en la creación de un sistema de reglas robusto 
 y escalable para juegos de estrategia por turnos.
 Todos los scripts se encuentran en "Ascencio\Game source files\Assets\Scripts".
 Únicamente algunas partes del código fue desarrollado por mí en este proyecto, cuyos detalles son descritos a continuación.

## 🧩 Arquitectura del Sistema
El núcleo del juego se basa en una arquitectura modular que separa la representación de datos de la lógica de ejecución:

*   **Definición de Entidades:** Utilizo una clase base `Piece` que centraliza propiedades clave como el tipo de pieza, capacidades de vuelo y 
*	  rangos de movimiento (horizontal, vertical y diagonal).
*
*   **Sistema de Rejilla (GridSystem):** El tablero se gestiona mediante matrices, permitiendo una búsqueda y actualización de estados de celda eficiente.
*
*   **Controlador de Turnos:** Implementé una gestión de turnos persistente para validar qué equipo tiene la autoridad de realizar movimientos en cada ciclo de juego.
*
*	**Integración del Sistema de Habilidades:** Como la propuesta de este prototipo se extiende más allá que el de un simple juego de Ajedrez, desarrollé un sistema general
*	  para la integración de habilidades para cada pieza. Actualmente la única habilidad configurada es la del "Enroque", que permite realizar un movimiento especial cuando
*	  una pieza del tipo "Rook" (Torre) interactúa con una pieza de tipo "King" (Rey). Si bien esta es una mecánica base del Ajedrez clásico, aquí fue programada como una
*	  habilidad especial, dentro de un sistema complejo pero abierto para la integración de más habilidades personalizadas.

## ⚙️ Implementación Técnica: La Clase ChessPiece
Mi mayor contribución técnica en este proyecto fue el desarrollo integral de la lógica de movimiento y validación con la clase ChessPiece:

*   **Asignación Dinámica de Reglas:** Utilizo estructuras de control `switch` para asignar valores de movimiento específicos basados en el `pieceType` de cada objeto.
*	  Cada tipo de pieza cuenta con sus propios comportamientos y características, permitiendo que cada una actúe de manera ágil según su tipo, principalmente para
*	  facilitar el desarrollo de las piezas.
*
*   **Algoritmo de Validación de Caminos:** Desarrollé un sistema que verifica la legalidad de los movimientos filtrando obstáculos en tiempo real. 
*	  Esto incluye lógica especializada para trayectorias diagonales (`CheckDiagonalPath`) y ortogonales (`CheckPath`), además de la lógica detrás del movimiento de cada
*	  pieza según su tipo, incluyendo la posibilidad de "comer" otras piezas.
*
*   **Ejecución y Registro de Movimientos:** El método `MovePiece` no solo actualiza la posición física, también gestiona la ocupación de celdas en el `GridSystem`, 
*	  registra el historial de movimientos y dispara la actualización de elementos visuales (highlights).

## 🚀 Flexibilidad
*   **Manejo de Casos Especiales:** Diseñar un sistema que diferencie entre el movimiento general y habilidades especiales (`hasSpecialMovement`) permitió 
*	  mantener un código limpio y fácil de extender para mecánicas futuras.
*
*	**Estructura abierta:** Todo el juego se desarrolló teniendo en mente agregar más piezas, más tipos de pieza y hasta más habilidades, por lo que la estructura
*	  del mismo se construyó de manera que facilite la implementación de más contenido futuro al juego con la mayor facilidad posible.