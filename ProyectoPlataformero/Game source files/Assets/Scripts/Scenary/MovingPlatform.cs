using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float maxBlocksUp;
    public float maxBlocksDown;
    public float speed;
    private float maxBlocksUpCoordinates;
    private float maxBlocksDownCoordinates;

    public Rigidbody2D rigidBody;

    private int direction = 1; // 1 for up, -1 for down


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxBlocksUpCoordinates = transform.position.y + maxBlocksUp;
        maxBlocksDownCoordinates = transform.position.y - maxBlocksDown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    // Método de movimiento que mueve a la plataforma hacia arriba o hacia abajo dependiendo de la dirección actual,
    // y verifica si es necesario cambiar la dirección al alcanzar los límites definidos por maxBlocksUpCoordinates y maxBlocksDownCoordinates.
    private void Move()
    {
        switch (direction)
        {
            case -1:
                MoveVertically(-speed);
                CheckDirectionChange();
                break;
            case 1:
                MoveVertically(speed);
                CheckDirectionChange();
                break;
        }
    }

    // Verifica si la plataforma ha alcanzado los límites definidos por maxBlocksUpCoordinates y maxBlocksDownCoordinates para
    // cambiar su dirección de movimiento.
    private void CheckDirectionChange()
    {
        if (transform.position.y <= maxBlocksDownCoordinates)
        {
            direction = 1; // Change direction to up
        }
        else if (transform.position.y >= maxBlocksUpCoordinates)
        {
            direction = -1; // Change direction to down
        }
    }

    // Método que mueve la plataforma verticalmente utilizando Rigidbody2D.MovePosition para garantizar una física adecuada y evitar problemas de colisión.
    private void MoveVertically(float distance)
    {
        // We calculate the next position based on the current position and the desired movement.
        Vector2 nextPos = rigidBody.position + new Vector2(0, distance * Time.fixedDeltaTime);

        // We use MovePosition to move the Rigidbody2D to the next position.
        rigidBody.MovePosition(nextPos);
    }

    // Métodos de colisión para hacer que el jugador se mueva junto con la plataforma mientras esté en contacto con ella.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
