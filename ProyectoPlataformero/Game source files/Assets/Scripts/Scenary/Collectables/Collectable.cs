using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

//Clase genérica de objeto coleccionable. Puede ser heredada por objetos específicos como monedas, power-ups, etc. para implementar
//comportamientos específicos de recolección y animación.
public class Collectable : MonoBehaviour
{
    public AudioSource pickUpSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Detecta colisiones con el jugador para ejecutar la recolección del objeto, reproducir el sonido de recogida y luego
    //desactivar el objeto para su destrucción posterior.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            //Debug.Log("Collectable collected by player.");
            Collect();

            GhostDestroy(pickUpSound.clip.length);
        }
    }

    // Método virtual para reproducir animaciones específicas del objeto coleccionable.
    // Puede ser sobrescrito por clases derivadas para implementar animaciones personalizadas al ser recogido.
    public virtual void ReproduceAnimation(){}

    //Desactiva el objeto para que no se pueda ver ni interactuar con él, pero permite que se ejecute código
    //completamente antes de destruirlo, como por ejemplo sonido.
    private void GhostDestroy(float delay)
    {
        //Desactiva el collider para evitar que se pueda recoger varias veces mientras se reproduce el sonido.
        GetComponent<Collider2D>().enabled = false;

        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().enabled = false;

        Destroy(gameObject, delay);
    }

    // Método virtual para ejecutar la lógica de recolección del objeto.
    public virtual void Collect()
    {
        //Debug.Log("Ejecutando pickUpSound");
        pickUpSound.Play();
    }
}
