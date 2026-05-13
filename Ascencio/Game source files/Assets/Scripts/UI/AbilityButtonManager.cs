using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonManager : MonoBehaviour
{
    public Button activateAbilityButton;
    public ChessPiece selectedPiece;
    public Vector2Int destination;
    public int abilityId;
    public AbilityFunctions abilityFunctions;

    void Awake()
    {
        abilityFunctions = gameObject.AddComponent<AbilityFunctions>();
    }

    void Start()
    {
        activateAbilityButton.onClick.AddListener(CallAbilityFunctions);
    }

    void CallAbilityFunctions()
    {
        abilityFunctions.ActivateAbilityFunction(abilityId, selectedPiece, destination);
        activateAbilityButton.gameObject.SetActive(false);
        //Cambia el turno después de usar la habilidad.
        selectedPiece.inputManager.ChangeTurn();
        selectedPiece.inputManager.selectedPiecesCount = 0;
    }
}
