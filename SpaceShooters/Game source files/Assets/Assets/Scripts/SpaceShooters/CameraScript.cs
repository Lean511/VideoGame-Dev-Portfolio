using UnityEngine;

// Clase que gestiona el comportamiento de la cámara en el juego, siguiendo al jugador desde una posición fija con un offset predeterminado.
public class CameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 26.17f, -9.57f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.FindObjectOfType<PlayerShip>().transform.position + offset;
    }
}
