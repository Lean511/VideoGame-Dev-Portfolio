using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BasicGroundEnemy : EnemyGeneralScript
{
    public float StepsToLeft;
    public float StepsToRight;
    public float speed;
    private float StepsToLeftCoordinates;
    private float StepsToRightCoordinates;

    public Rigidbody2D rigidBody;

    private int direction = 1; // 1 for right, -1 for left

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StepsToLeftCoordinates = transform.position.x - StepsToLeft;
        StepsToRightCoordinates = transform.position.x + StepsToRight;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckVulnerabilityByCollision(collision);
    }

    // Método de patrullaje que mueve al enemigo de izquierda a derecha entre dos puntos definidos por StepsToLeft y StepsToRight.
    public override void Patrol()
    {
        switch (direction)
        {
            case -1:
                MoveHorizontally(-speed);
                CheckDirectionChange();
                break;
            case 1:
                MoveHorizontally(speed);
                CheckDirectionChange();
                break;
        }
    }

    // Verifica si el enemigo ha alcanzado los límites definidos por StepsToLeftCoordinates y StepsToRightCoordinates para cambiar su dirección de movimiento.
    private void CheckDirectionChange()
    {
        if (transform.position.x <= StepsToLeftCoordinates)
        {
            direction = 1; // Change direction to right
            FlipSprite();
        }
        else if (transform.position.x >= StepsToRightCoordinates)
        {
            direction = -1; // Change direction to left
            FlipSprite();
        }
    }

    // Invierte la escala del sprite en el eje x para que el enemigo mire en la dirección opuesta cuando cambie de dirección.
    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invert the x scale to flip the sprite
        transform.localScale = scale;
    }

    // Mueve al enemigo horizontalmente a una velocidad determinada por el parámetro distance (Definido por la variable pública speed), manteniendo su velocidad vertical actual.
    private void MoveHorizontally(float distance)
    {
        rigidBody.linearVelocity = new Vector2(distance, rigidBody.linearVelocity.y);
    }
}
