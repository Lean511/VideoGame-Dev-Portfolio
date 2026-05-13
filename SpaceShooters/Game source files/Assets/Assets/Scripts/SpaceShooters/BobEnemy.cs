using UnityEngine;

// Clase que representa un enemigo específico en el juego, gestionando su movimiento hacia el jugador, su capacidad de disparar
// cuando está a cierta distancia y su destrucción al ser alcanzado por una bala del jugador.
public class BobEnemy : MonoBehaviour
{
    private PlayerShip player;
    public float movementSpeed = 6f;
    public GameObject rightGunPoint;
    public GameObject leftGunPoint;
    public EnemyBullet enemyBulletPrefab;
    private float currentTime = 3;
    public float minimumDistanceToShoot = 16;
    private float distanceToPlayer;
    public int points;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerShip>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = player.transform.position - transform.position;
        transform.position += transform.forward * movementSpeed * Time.deltaTime;


        distanceToPlayer = (player.transform.position.x - transform.position.x) + (player.transform.position.z - transform.position.z);
        
        if (distanceToPlayer < 0)
        {
            distanceToPlayer = distanceToPlayer * -1;
        }

        if (distanceToPlayer <= minimumDistanceToShoot)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime < 0)
        {
            currentTime = 3;
            EnemyBullet bulletR = Instantiate(enemyBulletPrefab);
            //bulletR.transform.position = transform.position + rightShootOffset;
            bulletR.transform.position = rightGunPoint.transform.position;
            bulletR.transform.up = transform.forward;

            EnemyBullet bulletL = Instantiate(enemyBulletPrefab);
            bulletL.transform.position = leftGunPoint.transform.position;
            bulletL.transform.up = transform.forward;
        }
    }

    // Detecta colisiones con objetos que tengan un componente Bullet (balas del jugador) y destruye tanto el enemigo como la bala al colisionar.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    // Al destruirse el enemigo, se añade una cantidad de puntos al marcador del jugador a través del GameManager.
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(100);
        }
    }

    // Función auxiliar para obtener la dirección de entrada del jugador en el editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumDistanceToShoot);
    }
}
