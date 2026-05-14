# Space Shooters: Sistema de Navegación y Combate 3D

 Este proyecto es un shooter de perspectiva cenital centrado en el control preciso del jugador y sistemas de generación de enemigos dinámicos. <br>
 Todos los scripts se encuentran en "SpaceShooters\Game source files\Assets\Assets\Scripts". <br>
 Todo el código de este proyecto fue desarrollado únicamente por mí.
 
<video src="SpaceShooters.mp4" width="100%" controls autoplay loop muted></video>

## 🕹️ Mecánicas de Navegación y Control
El movimiento de la nave integra la posición del cursor del mouse directamente en la lógica de navegación:

*   **Raycasting para Orientación:** Utilizo `Camera.main.ScreenPointToRay` para proyectar un rayo desde la pantalla hacia el mundo. <br>
El punto de impacto en el `groundLayer` define el objetivo de rotación de la nave, manteniendo la coherencia en el eje Y.

*   **Locomoción Basada en Física:** La velocidad de traslación se aplica a través de `rb.linearVelocity` en la dirección frontal de la nave, permitiendo un control direccional fluido y preciso.

## ⚔️ Sistema de Combate y Gestión de Daño
Implementé una detección de impactos basada en etiquetas (Tags) y componentes, permitiendo que cada entidad reaccione al daño según su propia lógica interna sin depender de un controlador externo.

*   **Detección por Triggers:** Utilizo `OnTriggerEnter` con validación de componentes (`GetComponent<Bullet>`) para gestionar las colisiones. <br>
Esto permite que cada entidad (Jugador o Enemigo) decida cómo reaccionar al impacto de forma independiente.

*   **Feedback Visual y UI:** El sistema de daño del jugador incluye estados de invulnerabilidad temporal y actualización en tiempo real de la interfaz mediante un `uiManager`. Esto se ve reflejado en la apariencia de la nave al recibir daño y en la barra de vida.

## 👾 Arquitectura de Spawning
Para la gestión de enemigos, desarrollé una clase `EnemySpawnerSS` que utiliza:

*   **Puntos de Generación Definidos:** Uso de un array de `Transform` para predefinir zonas seguras de spawn en el mapa.

*   **Lógica de Instanciación Aleatoria:** Un sistema basado en cronómetros (`Time.deltaTime`) que selecciona índices aleatorios de la lista de puntos para instanciar enemigos a intervalos regulares, asegurando una dificultad constante y variada.

## 🛠️ Detalles de Implementación
*   **Optimización de Colisiones:** Uso de capas (`groundLayer`) en el Raycast para evitar cálculos innecesarios con otros objetos del juego.

*   **Estructura Limpia:** Métodos helper como `GetSpawnPoint()` para mantener el flujo del `Update` legible y fácil de mantener.
