using Unity.VisualScripting;
using UnityEngine;

//Clase que gestiona la lógica principal del juego.
public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public static GameManager instance;
    public int score = 0;
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

    // Método para agregar puntos a la puntuación actual. Actualiza la puntuación en la interfaz de usuario a través del UIManager.
    public void AddScore(int points)
    {
        score += points;
        uiManager.SetScore(score);
    }

    // Método para obtener la puntuación actual. Devuelve el valor de la variable score.
    public int GetScore()
    {
        return score;
    }
}
