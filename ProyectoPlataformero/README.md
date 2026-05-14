# Prototipo de Plataformas: Sistema de Inercia y Combate por Velocidad

 Este proyecto se centra en el "Game Feel" y la manipulación de físicas 2D para crear un movimiento fluido basado en la acumulación de energía cinemática. <br>
 Todos los scripts se encuentran en "ProyectoPlataformero\Game source files\Assets\Scripts". <br>
 Todo el código fue escrito únicamente por mí, a excepción de los métodos que involucran la manipulación de sprites.

 ![Test](clip.gif)

## 🏃 Lógica de Movimiento e Inercia
En lugar de simplemente utilizar un movimiento binario (moverse/detenerse), implementé un sistema de aceleración progresiva que recompensa el movimiento continuo.

*   **Manipulación de Velocidad Linear:** Como se observa en `ListenForHorizontalMovementInputs()`, utilizo manipulación directa de `rigidBody.linearVelocity` para garantizar una respuesta inmediata del input, preservando la gravedad del motor de física.

*   **Curva de Aceleración Personalizada:** Mediante el método `CheckForInertiaAcceleration()`, el `movementSpeed` aumenta gradualmente hasta un `maxMovementSpeed`. 

*   **Tiempo de Gracia (Grace Period):** Implementé un sistema basado en `Time.time` para permitir que el jugador mantenga su inercia durante un breve periodo incluso después de soltar el input, mejorando la fluidez del control.

## ⚔️ Sistema de Combate Condicional
El núcleo del gameplay es la vulnerabilidad basada en estados. El jugador no posee un botón de ataque, sino que su propio movimiento es el arma.

*   **Detección de Estado "Powered":** Utilizo un método booleano `isPowered()` que actúa como un "Gatekeeper", permitiendo acciones ofensivas solo cuando la velocidad actual iguala o supera la máxima permitida.

*   **Comunicación entre Objetos:** El sistema utiliza `OnTriggerEnter2D` para identificar enemigos. Si se cumplen las condiciones (`isPowered` + `isVulnerable`), se dispara el método `Die()` del enemigo; de lo contrario, el jugador ejecuta `ReceiveDamage()`.

*   **Solución de Errores (Fix de Colisiones):** Como detalle técnico, incluí un reinicio rápido del `Collider2D` tras el impacto para prevenir errores de detección múltiple y asegurar la invulnerabilidad temporal del jugador.

## 🛠️ Buenas Prácticas Implementadas
*   **Encapsulamiento:** Métodos privados para lógica interna y públicos para interacción externa.

*   **Modularidad:** Separación clara entre la escucha de inputs, el cálculo de físicas y la resolución de colisiones.

*   **Código Auto-documentado:** Nombres de variables descriptivos y comentarios claros en español para facilitar el mantenimiento.
