using TMPro;
using UnityEngine;

//Clase que gestiona la interfaz de usuario del juego, incluyendo la visualización del puntaje del jugador y la cantidad de monedas recolectadas.
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI playerScoreValueLabel;
    public TextMeshProUGUI playerCoinsValueLabel;

    private int defaultPlayerScore = 0;
    private int defaultPlayerCoins = 0;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WriteCoinsAmount(LoadPlayerCoins());
        WriteScore(defaultPlayerScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Método para cargar la cantidad de monedas del jugador. Si no hay datos guardados, devuelve el valor por defecto.
    //Por el momento no se utiliza, pero se deja preparado para futuras implementaciones.
    private int LoadPlayerCoins()
    {
        return defaultPlayerCoins;
    }

    //Método para actualizar el puntaje del jugador.
    public void WriteScore(int score)
    {
        playerScoreValueLabel.text = score.ToString();
    }

    // Método para actualizar la cantidad de monedas del jugador en la interfaz de usuario.
    public void WriteCoinsAmount(int amount)
    {
        playerCoinsValueLabel.text = amount.ToString();
    }
}
