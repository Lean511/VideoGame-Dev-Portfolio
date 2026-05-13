using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    #region REFERENCIAS
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;
    public GameObject bishopPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;
    public CheckService checkService;
    public MoveValidationService moveValidationService;
    public Teams.TeamName lastMoveTeam;
    [HideInInspector] public CheckmateService checkmateService;
    #endregion
    #region VARIABLES
    [Header("Alturas visuales")]
    public float boardY = 0f;
    public float highlightY = 0.01f;
    public float pieceY = 0.02f;
    public float indicatorY = 0.03f;
    //Ancho de grilla en cantidad de celdas
    public int width;
    //Alto de grilla en cantidad de celdas
    public int height;
    //Tamaño de cada celda
    public float cellSize = 1f;
    //Posición de origen de la grilla en el mundo
    public Vector3 originPosition = Vector3.zero;
    public Vector2Int lastMoveFrom = Vector2Int.zero;
    public Vector2Int lastMoveTo = Vector2Int.zero;
    //Contador para los turnos.
    public int turn = 0;
    //Matriz que representa los valores almacenados en cada celda de la grilla
    private int[,] grid;
    //Matriz que representa si una celda está ocupada o no
    private bool[,] occupiedCells;
    // Modo de depuración para mostrar información adicional
    public bool debugMode;


    #endregion

    void Start()
    {
        checkService = new CheckService(this);
        moveValidationService = new MoveValidationService(this, checkService);
        checkmateService = new CheckmateService(
            this,
            checkService,
            moveValidationService
        );

        // Inicializa las matrices
        grid = new int[width, height];
        occupiedCells = new bool[width, height];

        // Dibuja líneas verticales de la grilla en la escena para visualización
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = originPosition + new Vector3(x * cellSize, 0, 0);
            Vector3 end = originPosition + new Vector3(x * cellSize, 0, height * cellSize);
            Debug.DrawLine(start, end, Color.blue, 100f);
        }

        // Dibuja las líneas horizontales de la grilla
        for (int y = 0; y <= height; y++)
        {
            Vector3 start = originPosition + new Vector3(0, 0, y * cellSize);
            Vector3 end = originPosition + new Vector3(width * cellSize, 0, y * cellSize);
            Debug.DrawLine(start, end, Color.blue, 100f);
        }

        // Inicializa piezas
        SetupBoard();
    }

    public void RegisterMove(Vector2Int from, Vector2Int to, Teams.TeamName team)
    {
        lastMoveFrom = from;
        lastMoveTo = to;
        lastMoveTeam = team;
    }

    void SetupBoard()
    {
        // Peones blancos (fila 1)
        for (int x = 0; x < 8; x++)
        {
            SpawnPiece(pawnPrefab, x, 1, Teams.TeamName.White);
        }

        // Peones negros (fila 6)
        for (int x = 0; x < 8; x++)
        {
            SpawnPiece(pawnPrefab, x, 6, Teams.TeamName.Black);
        }

        // Torres
        SpawnPiece(rookPrefab, 0, 0, Teams.TeamName.White); // blanca izquierda
        SpawnPiece(rookPrefab, 7, 0, Teams.TeamName.White); // blanca derecha
        SpawnPiece(rookPrefab, 0, 7, Teams.TeamName.Black); // negra izquierda
        SpawnPiece(rookPrefab, 7, 7, Teams.TeamName.Black); // negra derecha

        // Caballos
        SpawnPiece(knightPrefab, 1, 0, Teams.TeamName.White);
        SpawnPiece(knightPrefab, 6, 0, Teams.TeamName.White);
        SpawnPiece(knightPrefab, 1, 7, Teams.TeamName.Black);
        SpawnPiece(knightPrefab, 6, 7, Teams.TeamName.Black);

        // Alfiles
        SpawnPiece(bishopPrefab, 2, 0, Teams.TeamName.White);
        SpawnPiece(bishopPrefab, 5, 0, Teams.TeamName.White);
        SpawnPiece(bishopPrefab, 2, 7, Teams.TeamName.Black);
        SpawnPiece(bishopPrefab, 5, 7, Teams.TeamName.Black);

        // Reina
        SpawnPiece(queenPrefab, 3, 0, Teams.TeamName.White);
        SpawnPiece(queenPrefab, 3, 7, Teams.TeamName.Black);

        // Rey
        SpawnPiece(kingPrefab, 4, 0, Teams.TeamName.White);
        SpawnPiece(kingPrefab, 4, 7, Teams.TeamName.Black);
    }

    public void SpawnPiece(GameObject prefab, int x, int y, Teams.TeamName team)
    {
        Vector3 worldPos = GetWorldPosition(x, y);
        Vector2Int spawnPosition = new Vector2Int(x, y);

        GameObject pieceObj = Instantiate(prefab, worldPos, Quaternion.identity);

        ChessPiece piece = pieceObj.GetComponent<ChessPiece>();
        piece.SetGridSystem(this);
        piece.SetPosition(spawnPosition);
        piece.team = team;
        piece.SetupAbilities(); //Carga las habilidades de la pieza.

        //Asigna el playerId según el equipo.
        if (piece.team == Teams.TeamName.White)
        {
            piece.playerId = 0;
        }
        else if (piece.team == Teams.TeamName.Black)
        {
            piece.playerId = 1;
        }
        SetCellAsOccupied(spawnPosition, true);
    }

    public void DespawnPiece(ChessPiece piece)
    {
        if (IsCellOccupied(piece.position))
        {
            piece.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Error: No hay ninguna pieza eliminable.");
        }
    }

    #region CONVERSION DE POSICIONES(MUNDO/CELDA)
    // Convierte una posición del mundo en una posición dentro del grid (coordenadas X,Y)
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
        return new Vector2Int(x, y);
    }

    // Convierte coordenadas del grid en una posición del mundo (centro de la celda)
    public Vector3 GetWorldPosition(int x, int y)
    {
        return originPosition + new Vector3(
        x * cellSize + cellSize / 2f,
        pieceY,
        y * cellSize + cellSize / 2f
    );
    }


    #endregion

    #region LECTURA Y ESCRITURA DE CELDAS

    // Asigna un valor entero a una celda del grid, si está dentro de los límites
    public void SetCell(int x, int y, int value)
    {

        if (grid != null && x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y] = value;
        }
    }

    #endregion

    #region SISTEMA DE OCUPACIÓN DE CELDAS
    // Devuelve true si la celda está ocupada o si las coordenadas son inválidas (fuera del grid)
    public bool IsCellOccupied(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int y = gridPosition.y;

        if (occupiedCells == null)
            return false;

        // Si está fuera de los límites del grid, se considera ocupada por seguridad
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return true;
        }

        // Verifica si la celda está ocupada
        if (occupiedCells[x, y])
        {
            return true;
        }

        return false;
    }

    // Marca o desmarca una celda como ocupada
    public void SetCellAsOccupied(Vector2Int cell, bool occupied)
    {
        int x = cell.x;
        int y = cell.y;

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            occupiedCells[x, y] = occupied;
        }
        else
        {
        }
    }

    // Devuelve true si las coordenadas están dentro de los límites del grid
    public bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    #endregion

    public ChessPiece GetPieceOnGridPosition(Vector2Int GridPosition)
    {
        ChessPiece[] piece = FindObjectsByType<ChessPiece>(FindObjectsSortMode.None);

        foreach (ChessPiece p in piece)
        {
            if (p.position == GridPosition)
            {
                return p;
            }
        }

        return piece[0] ;
    }

    public List<ChessPiece> GetAllPieces()
    {
        return new List<ChessPiece>(FindObjectsByType<ChessPiece>(FindObjectsSortMode.None));
    }
}
