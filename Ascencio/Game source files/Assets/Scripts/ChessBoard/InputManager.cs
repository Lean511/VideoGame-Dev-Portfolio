using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GridSystem gridSystem;
    public HighlightManager highlightManager;
    public MoveHistory history;

    private ChessPiece selectedPiece;
    private ChessPiece oldSelectedPiece;
    private Vector2Int selectedPositionOnGrid;
    public ChessAI chessAI;
    public Teams.TeamName currentTurnTeam = Teams.TeamName.White;
    public int selectedPiecesCount = 0;
    public Button activateAbilityButton;
    public AbilityButtonManager abilityButtonManager;
    public TextMeshProUGUI abilityNameLabel;

    // Update is called once per frame
    void Update()
    {
        if (currentTurnTeam == Teams.TeamName.Black)
            return;

        //Si el botón de activar habilidad está activo y se hace click izquierdo o derecho, se desactiva la pieza y se vuelve a la pieza seleccionada anteriormente.
        //if (activateAbilityButton.IsActive() && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        if (activateAbilityButton.IsActive() && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Habilidad no seleccionada");
            activateAbilityButton.gameObject.SetActive(false);
            SelectPiece(selectedPiece, false);
            selectedPiecesCount--;
            SelectPiece(oldSelectedPiece, true);
            return;
        }

        //Al hacer click derecho
        if (Input.GetMouseButtonDown(1))
        {
            //Si no hay una pieza seleccionada, se deselecciona la pieza seleccionada previamente.
            if (selectedPiece == null)
            {
                SelectPiece(oldSelectedPiece, false);
            }

            //Busca la pieza seleccionada anteriormente y la deselecciona
            SelectPiece(selectedPiece, false);
            Debug.Log("Pieza deseleccionada.");
        }

        //Al hacer click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Ray sentRay = SendRay();
            ChessPiece piece = FindPiece(sentRay);
            SelectCell(sentRay);

            // Seleccionar la pieza clickeada
            //if (piece != null && selectedPiece == null)
            if ((piece != null && selectedPiece == null) || (piece != null && selectedPiece.isMovementLegal(selectedPiece, selectedPositionOnGrid)))
            {
                /////////////////
                if (GameManager.Instance != null && GameManager.Instance.gameEnded)
                    return;
                /////////////////

                if ((!gridSystem.debugMode && piece.playerId == 0) || gridSystem.debugMode)
                {
                    //SelectPiece(selectedPiece, false);
                    SelectPiece(piece, true);
                    Debug.Log("Seleccionaste: " + piece.pieceName);
                }
            }

            //Activate ability with a button
            //Si hay dos piezas seleccionadas, se activa el botón de activar habilidad.
            if (selectedPiecesCount == 2)
            {
                activateAbilityButton.gameObject.SetActive(true);
                abilityNameLabel.text = "Activate " + oldSelectedPiece.GetAbilityGroup().GetActivableAbility(oldSelectedPiece, selectedPiece.position).name + "?";
                abilityButtonManager.selectedPiece = oldSelectedPiece;
                abilityButtonManager.destination = selectedPiece.position;
                abilityButtonManager.abilityId = oldSelectedPiece.selectedAbility;
            }

            #region Calcular movimientos posibles.
            // Mover la pieza seleccionada a la celda clickeada
            //Si hay una pieza seleccionada
            //Debug.Log("Pieza seleccionada: " + (selectedPiece != null ? selectedPiece.pieceName : "Ninguna"));
            if (selectedPiece != null)
            {
                ////////////////////
                if (GameManager.Instance != null && GameManager.Instance.gameEnded)
                    return;
                /////////////////////////
                //Si la celda no está ocupada o hay una pieza rival, se sigue el proceso de movimiento.
                if (!gridSystem.IsCellOccupied(selectedPositionOnGrid) || (piece != null && piece.playerId != selectedPiece.playerId))
                {
                    if (selectedPiece.isMovementLegal(selectedPiece, selectedPositionOnGrid))
                    {
                        // No dejar al rey en jaque
                        if (!gridSystem.moveValidationService
                            .IsMoveLegalConsideringCheck(selectedPiece, selectedPositionOnGrid))
                        {
                            Debug.Log("Movimiento ilegal: deja al rey en jaque");
                            return;
                        }

                        ChessPiece capturedPiece = null;
                        if (piece != null && piece.team != selectedPiece.team)
                        {
                            capturedPiece = piece;
                            piece.Eat();
                        }

                        selectedPiece.MovePiece(selectedPositionOnGrid, capturedPiece);
                        ChangeTurn();
                    }
                    else
                    {
                        Debug.Log("No se puede mover la pieza a esa celda.");
                        SelectPiece(selectedPiece, false);
                    }
                }
            }
            #endregion
        }
    }

    public void ChangeTurn()
    {
        // Cambiar turno
        gridSystem.turn++;
        if (!gridSystem.debugMode)
        {
            currentTurnTeam = Teams.TeamName.Black;
        }
        ////////////////////////
        if (gridSystem.checkService.IsKingInCheck(Teams.TeamName.White))
        {
            Debug.Log("Rey BLANCO en jaque");
        }

        if (gridSystem.checkService.IsKingInCheck(Teams.TeamName.Black))
        {
            Debug.Log("Rey NEGRO en jaque");
        }

        if (gridSystem.checkmateService.IsCheckmate(Teams.TeamName.Black))
        {
            Debug.Log("JAQUE MATE — GANAN LAS BLANCAS");
            GameManager.Instance.EndGame(Teams.TeamName.White);
            return;
        }
        //////////////////////
        // Mostrar última jugada del jugador
        highlightManager.ShowLastMove(Teams.TeamName.White);

        if (!gridSystem.debugMode)
        {
            Debug.Log("InputManager: turno del jugador terminado, empieza IA");
            // Arrancar IA
            chessAI.StartTurn();
        }
    }

    //Registra el movimiento en el historial de movimientos.
    public void RegisterMoveInHistory(ChessPiece piece, Vector2Int from, Vector2Int destination, ChessPiece capturedPiece)
    {
        if (piece != null && history != null && piece.hasMoved)
        {
            MoveData moveData = new MoveData
            {
                piece = piece,
                from = from,
                to = destination,
                turn = gridSystem.turn,
                capturedPiece = capturedPiece,
                team = piece.team
            };

            string text = $"Turno {gridSystem.turn}: Jugador {piece.team} movió {piece.pieceName} a {destination}";

            history.AddMove(moveData);
        }
    }

    //Envía un rayo desde la cámara principal hacia la posición del mouse.
    private Ray SendRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        return ray;
    }

    //Selecciona la celda en la que se hizo click usando el Ray.
    private void SelectCell(Ray ray)
    {
        Vector3 selectedPositionOnWorld;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            selectedPositionOnWorld = hit.point;
            selectedPositionOnGrid = gridSystem.GetGridPosition(selectedPositionOnWorld);

            //Debug.Log($"Click en mundo: {selectedPositionOnWorld}, convertido a celda: {selectedPositionOnGrid}");
        }
    }

    //Encuentra la pieza con el Raycast.
    private ChessPiece FindPiece(Ray ray)
    {
        ChessPiece foundPiece;

        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        foundPiece = hit.collider.GetComponent<ChessPiece>();

        if (foundPiece != null)
        {
            foundPiece.inputManager = this;
        }

        return foundPiece;
    }

    // Selecciona o deselecciona una pieza.
    public void SelectPiece(ChessPiece piece, Boolean select)
    {
        ///////////////////////
        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;
        /////////////////////////
        
        if (piece == null)
        {
            selectedPiece = null;
            return;
        }

        piece.isSelected = select;

        if (select)
        {
            selectedPiecesCount++;
        }

        if (!select)
        {
            selectedPiecesCount--;
            selectedPiece = null;
            highlightManager.ClearMoveHighlights();
            highlightManager.ShowLastMove(Teams.TeamName.White);
            return;
        }

        //Guardar la pieza seleccionada anteriormente si se selecciona una segunda pieza.
        if (selectedPiecesCount == 2)
        {
            oldSelectedPiece = selectedPiece;
        }

        selectedPiece = piece;

        highlightManager.ShowHighlights(piece);
    }
}
