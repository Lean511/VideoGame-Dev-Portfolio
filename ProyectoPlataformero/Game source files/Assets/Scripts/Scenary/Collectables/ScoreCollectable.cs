using UnityEngine;

public class ScoreCollectable : Collectable
{
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager = GetComponent<GameManager>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sobrescribe el método Collect para agregar una moneda al contador del jugador a través del GameManager.
    public override void Collect()
    {
        base.Collect();
        gameManager.AddScore(1);
    }
}
