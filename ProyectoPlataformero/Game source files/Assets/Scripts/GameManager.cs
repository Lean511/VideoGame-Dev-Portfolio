using UnityEngine;

//Clase que gestiona el estado general del juego, incluyendo la puntuación del jugador y la cantidad de monedas recolectadas.
//Utiliza el patrón Singleton para garantizar que solo exista una instancia de GameManager en la escena, lo que permite un acceso global
//a sus métodos y propiedades desde otras clases como Collectable, ScoreCollectable y CoinCollectable.
public class GameManager : MonoBehaviour
{
    public UIManager uiManager;

    public static GameManager instance;

    [SerializeField] private int playerScore;
    [SerializeField] private int playerCoins;

    private void Awake()
    {
        //Singleton pattern. Ensures only one instance of GameManager exists.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para agregar puntos al puntaje del jugador. Actualiza la interfaz de usuario a través del UIManager para reflejar el nuevo puntaje.
    public void AddScore(int points)
    {
        playerScore += points;
        uiManager.WriteScore(playerScore);
    }

    // Método para obtener el puntaje actual del jugador. Puede ser utilizado por otras clases para mostrar el puntaje o
    // para lógica de juego basada en el puntaje.
    public int GetScore()
    {
        return playerScore;
    }

    // Método para agregar monedas al total del jugador.
    // Actualiza la interfaz de usuario a través del UIManager para reflejar la nueva cantidad de monedas.
    public void AddCoins(int points)
    {
        playerCoins += points;
        uiManager.WriteCoinsAmount(playerCoins);
    }

    // Método para obtener la cantidad actual de monedas del jugador. Puede ser utilizado por otras clases para mostrar la cantidad de monedas.
    public int GetCoinsAmount()
    {
        return playerCoins;
    }
}
