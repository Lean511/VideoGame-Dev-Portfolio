using UnityEngine;

// Clase que representa la bala disparada por el jugador, gestionando su movimiento y destrucción después de un tiempo determinado.
public class Bullet : MonoBehaviour
{
    public float shotSpeed = 70f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * shotSpeed * Time.deltaTime;
    }
}
