using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    private Vector3 initialPosition; //Posición inicial del jugador

    public float initialMovementSpeed; //Velocidad base del jugador
    public float inertiaSpeed; //Velocidad adicional que se suma con la inercia
    public float maxMovementSpeed; //Velocidad máxima que puede alcanzar el jugador
    public float deathAltitude; //Altitud a la que el jugador "muere" y reaparece
    private float gracePeriod = 0.2f; // Tiempo de gracia en segundos
    private float lastInputTime; // Cronómetro interno

    public int jumpForce; //Fuerza del salto
    public int maxJumps; //Máxima cantidad de saltos
    public int maxLives; //Vidas máximas
    private int invulnerabilityFrames = 100; //Frames de invulnerabilidad tras recibir daño
    public int score; //Puntaje del jugador
    public int coins; //Monedas recolectadas por el jugador

    [SerializeField] private int remainingInvulnerabilityFrames = 0; //Frames de invulnerabilidad restantes
    [SerializeField] private int remainingLives; //Vidas restantes
    [SerializeField] private int remainingJumps; //Saltos restantes
    [SerializeField] private float movementSpeed; //Velocidad actual del jugador

    [Header("Feedback visual")]
    [SerializeField] private SpriteRenderer[] spriteRenderers; //Asignar en inspector o se recogerán los hijos

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Configura el juego para correr a 60 FPS y desactiva VSync para evitar limitaciones de rendimiento.
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //Configura la posición inicial, vidas, velocidad y saltos del jugador al iniciar el juego.
        initialPosition = transform.position;
        remainingLives = maxLives;
        movementSpeed = initialMovementSpeed;
        remainingJumps = maxJumps;

        // Si no se asignaron renderers en el inspector, buscarlos en hijos
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        ListenForJumpInputs();

        ListenForHorizontalMovementInputs();

        CheckDeathAltitude();
    }

    public void FixedUpdate()
    {
        if (remainingInvulnerabilityFrames > 0)
        {
            remainingInvulnerabilityFrames--;

            // Parpadeo: cambia visibilidad cada intervalo de frames
            int blinkInterval = 10;
            bool visible = (remainingInvulnerabilityFrames % blinkInterval) > (blinkInterval / 2);

            if (spriteRenderers != null)
            {
                foreach (var sr in spriteRenderers)
                {
                    if (sr != null) sr.enabled = visible;
                }
            }

            // Si justo terminó la invulnerabilidad, asegurar visible
            if (remainingInvulnerabilityFrames == 0 && spriteRenderers != null)
            {
                foreach (var sr in spriteRenderers)
                {
                    if (sr != null) sr.enabled = true;
                }
            }
        }
    }

    //Método público para obtener la velocidad actual del jugador, útil para otros scripts que necesiten esta información.
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    //Método público para recibir daño, reduce vidas y activa invulnerabilidad temporal.
    //Si las vidas llegan a cero, llama al método Die() para reiniciar el juego.
    public void ReceiveDamage()
    {
        //Si está en invulnerabilidad, no recibe daño.
        if (remainingInvulnerabilityFrames > 0)
        {
            return;
        }

        remainingLives--;

        if (remainingLives <= 0)
        {
            Die();
        }
        else
        {
            remainingInvulnerabilityFrames = invulnerabilityFrames;
        }
    }

    //Escucha las entradas de salto del jugador. Si se detecta un salto y el jugador tiene saltos restantes,
    //aplica una fuerza de impulso hacia arriba y reduce los saltos restantes en uno.
    private void ListenForJumpInputs()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && remainingJumps > 0)
        {
            //Neutraliza el impulso para que no afecte el salto.
            SetAccelerationToZero();

            //ForceMode2D.Impulse aplica una fuerza instantánea al objeto.
            //ForceMode2D.Force aplica una fuerza continua al objeto.
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            remainingJumps--;
            //Debug.Log("Salto detectado.");
        }
    }

    //Reduce la fuerza de velocidad a cero.
    private void SetAccelerationToZero()
    {
        rigidBody.linearVelocity = Vector2.zero;
    }

    //Aplicar inercia al movimiento horizontal.
    private void CheckForInertiaAcceleration()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Debug.Log("Movimiento detectado.");
            // El jugador se está moviendo: reseteamos el cronómetro y aumentamos velocidad
            lastInputTime = Time.time;

            if (movementSpeed < maxMovementSpeed)
            {
                movementSpeed = movementSpeed + inertiaSpeed;
            }
        }
        else
        {
            // Si soltó el control, verificamos si ya pasó el tiempo de gracia
            if (Time.time - lastInputTime > gracePeriod)
            {
                movementSpeed = initialMovementSpeed;
            }
        }
    }

    //Reaparece al caer bajo la altitud de muerte.
    private void CheckDeathAltitude()
    {
        if (transform.position.y < deathAltitude)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y + (deathAltitude * -1 + 1));
            Die();
        }
    }

    //Método para manejar la muerte del jugador. Resetea la posición, velocidad, vidas y asegura que el sprite sea visible al reaparecer.
    public void Die()
    {
        //Respawn
        transform.position = initialPosition;
        rigidBody.linearVelocity = Vector2.zero;

        remainingLives = maxLives;

        // Asegurar que el sprite sea visible al reaparecer
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
            {
                if (sr != null) sr.enabled = true;
            }
        }
    }

    //Movimiento horizontal del jugador.
    private void ListenForHorizontalMovementInputs()
    {
        CheckForInertiaAcceleration();

        //"rb.linearVelocity.y" mantiene la gravedad.
        rigidBody.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rigidBody.linearVelocity.y);
    }

    //Detecta colisiones con enemigos. Si el jugador está en estado "powered" y el enemigo es vulnerable, el enemigo muere.
    //De lo contrario, el jugador recibe daño.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyGeneralScript genericEnemy = collision.gameObject.GetComponent<EnemyGeneralScript>();
            //Debug.Log("genericEnemy: " + genericEnemy);

            if (isPowered() && genericEnemy.IsVulnerable())
            {
                genericEnemy.Die();
            }
            else
            {
                ReceiveDamage();
            }

            //Reinicia el collider para evitar invulnerabilidad.
            var playerCollision = GetComponent<Collider2D>();
            if (playerCollision != null)
            {
                playerCollision.enabled = false;
                playerCollision.enabled = true;
            }
        }
    }

    //Checkea requisitos para habilidad de ataque básico.
    private bool isPowered()
    {
        return movementSpeed >= maxMovementSpeed;
    }

    //Resetea los saltos al tocar el suelo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        remainingJumps = maxJumps;
    }
}