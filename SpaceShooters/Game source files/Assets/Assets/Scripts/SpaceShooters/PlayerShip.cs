using UnityEngine;
using UnityEngine.SceneManagement;

// Clase que representa la nave del jugador, gestionando su movimiento, disparo, salud y colisiones con balas enemigas.
public class PlayerShip : MonoBehaviour
{
    public Material material;
    public GameObject rightGunPoint;
    public GameObject leftGunPoint;
    public Bullet bulletPrefab;
    public Rigidbody rb;
    public LayerMask groundLayer;
    public float movementSpeed = 5;
    private float maxRayDistance = 1000;
    public int maxHealth = 15;
    public int health;
    public bool invulnerable = false;
    private float recoveryTime = 2;
    public UIManager uiManager;

    private void Awake()
    {
        material.color = Color.white;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            return;
        }

        //Rayo desde la cámara hacia la posición del mouse.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Almacenar información del rayo.
        RaycastHit raycastHit;
        //Dispara el rayo y comprueba si ha colisionado con algo. Luego almacena la información en raycastHit.
        if (Physics.Raycast(ray, out raycastHit, maxRayDistance, groundLayer))  
        {
            //Obtiene la posición del punto de colisión.
            Vector3 hitPoint = raycastHit.point;
            //Mantiene la altura de la nave.
            hitPoint.y = transform.position.y;
            //Hace que la nave mire hacia el punto de colisión.
            transform.forward = hitPoint - transform.position;
            rb.linearVelocity = transform.forward * movementSpeed;
        }

        //rb.linearVelocity = GetInput() * movementSpeed;

        //Disparar.
        if (Input.GetMouseButtonDown(0))
        {
            Bullet bulletR = Instantiate(bulletPrefab);
            //bulletR.transform.position = transform.position + rightShootOffset;
            bulletR.transform.position = rightGunPoint.transform.position;
            bulletR.transform.up = transform.forward;

            Bullet bulletL = Instantiate(bulletPrefab);
            bulletL.transform.position = leftGunPoint.transform.position;
            bulletL.transform.up = transform.forward;
        }

        //Tiempo de invulnerabilidad después de ser golpeado por una bala enemiga.
        if (invulnerable)
        {
            recoveryTime -= Time.deltaTime;
            if (recoveryTime <= 0)
            {
                invulnerable = false;
                material.color = Color.white;
                recoveryTime = 2;
            }
        }
    }

    Vector3 GetInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));  
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    // Método que se llama cuando la nave del jugador colisiona con otro objeto.
    // Si el objeto es una bala enemiga y el jugador no es invulnerable, se reduce la salud del jugador,
    // se actualiza la interfaz de usuario y se destruye la bala enemiga.
    // Si la salud del jugador llega a cero, se muestra la pantalla de fin de juego.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyBullet>() && !invulnerable)
        {
            invulnerable = true;
            material.color = Color.red;
            health--;
            uiManager.SetNewLife(health, maxHealth);
            if (health <= 0)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                uiManager.ShowEndgameScreen();
            }
            Destroy(collision.gameObject);
        }
    }
}
