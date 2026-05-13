using UnityEngine;

// Clase que se encarga de generar enemigos en el juego. Instancia enemigos en puntos de generación aleatorios a intervalos regulares.
public class EnemySpawnerSS : MonoBehaviour
{
    public BobEnemy enemyBob;
    public Transform[] spawnPoints;
    private float currentTime = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            currentTime = 1;
            BobEnemy enemy = Instantiate(enemyBob);
            enemy.transform.position = GetSpawnPoint();
        }
    }

    Vector3 GetSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}
