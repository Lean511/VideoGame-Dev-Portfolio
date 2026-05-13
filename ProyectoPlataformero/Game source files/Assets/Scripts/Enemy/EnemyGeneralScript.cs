using UnityEngine;

public class EnemyGeneralScript : MonoBehaviour
{
    [SerializeField] private bool vulnerable = false;

    // Sistema de salud y daño genérico. Actualmente deshabilitado.
    /*
    public float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }
    */

    private void Update()
    {
    }

    // Método genérico de muerte del enemigo. Actualmente solo destruye el objeto, pero puede ser ampliado para incluir animaciones, efectos de sonido, etc.
    public void Die()
    {
        Destroy(gameObject);
    }

    // Método genérico de patrullaje. Actualmente no hace nada, pero puede ser sobrescrito por clases derivadas para implementar comportamientos específicos de patrullaje.
    public virtual void Patrol() {}

    //Hace vulnerable al enemigo al colisionar con el jugador. Comportamiento default genérico de enemigo.
    //Sirve para saber si el enemigo cumple con las condiciones especificas para ser vulnerable.
    //Se puede sobreescribir para implementar condiciones de vulnerabilidad específicas para cada tipo de enemigo.
    public void CheckVulnerabilityByCollision(Collider2D collision)
    {
        //Debug.Log("Checking vulnerability by collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            vulnerable = true;
        }
        else
        {
            vulnerable = false;
        }
    }

    // Método para verificar si el enemigo es vulnerable. Puede ser utilizado por otros scripts para determinar si el enemigo puede recibir daño o no.
    public bool IsVulnerable()
    {
        return vulnerable;
    }
}
