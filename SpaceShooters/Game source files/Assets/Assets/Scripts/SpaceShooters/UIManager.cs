using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

// Clase que gestiona la interfaz de usuario del juego, incluyendo la puntuación, la barra de salud y la pantalla de fin de juego.
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public Image healthBar;
    public GameObject endgameScreen;
    public GameObject actualScoreLabel;
    public TextMeshProUGUI finalScoreText;
    public Button backToMenuButton;

    public int _score;

    private void Start()
    {
        Time.timeScale = 1;
        backToMenuButton.onClick.AddListener(BackToMenu);
        SetScore(0);
    }

    // Método que se llama al hacer clic en el botón "Volver al menú". Restaura el tiempo normal y carga la escena del menú.
    void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    // Método para actualizar la puntuación en la interfaz de usuario. Recupera el nombre del jugador desde PlayerPrefs y muestra la puntuación actual.
    public void SetScore(int score)
    {
        _score = score;

        // Retrieve player name from PlayerPrefs
        string playerName = PlayerPrefs.GetString("PlayerName");
        pointsText.text = playerName + ": " + _score.ToString();
    }

    // Método para actualizar la barra de salud en la interfaz de usuario. Calcula el porcentaje de salud actual y ajusta el fillAmount de la barra de salud en consecuencia.
    public void SetNewLife(float currentHealth, float maxHealth)
    {
        float percentage = currentHealth / maxHealth;
        Debug.Log("Percentage: " + percentage);
        healthBar.fillAmount = percentage;
    }

    // Método para mostrar la pantalla de fin de juego. Detiene el tiempo, oculta la etiqueta de puntuación actual y muestra la pantalla de
    // fin de juego con el nombre del jugador y la puntuación final.
    public void ShowEndgameScreen()
    {
        string playerName = PlayerPrefs.GetString("PlayerName");

        Time.timeScale = 0;

        actualScoreLabel.SetActive(false);
        endgameScreen.SetActive(true);
        finalScoreText.text = playerName + " - " + _score.ToString();
    }
}
