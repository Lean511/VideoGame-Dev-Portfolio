using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Clase que gestiona el menú principal del juego, permitiendo al jugador ingresar su nombre, iniciar el juego o salir de la aplicación.
public class Menu : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public TMP_InputField inputField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayGame()
    {
        string playerName = inputField.text;
        // Save the player name using PlayerPrefs
        PlayerPrefs.SetString("PlayerName", playerName);
        SceneManager.LoadScene("Game");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
