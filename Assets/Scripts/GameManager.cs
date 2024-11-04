using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;
    public AudioSource DestroySound;
    public GameObject winUI;
    

    //Ссылки на префабы фигуп

    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;

    //вспомогательный массив фигур на доске
    private GameObject[,] pieces;
    // отслеживаем перемещение пешек
    private List<GameObject> movedPawns;

    private Player white;
    private Player black;

    // текущий игрок
    public Player currentPlayer;
    // соперник
    public Player otherPlayer;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        pieces = new GameObject[8, 8]; // создаем массив всех клеток
        movedPawns = new List<GameObject>(); // создаём пустой лист перемещённых пешек
        
        //белый и черный игрок 
        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white; // первый игрок 
        otherPlayer = black;

        InitialSetup();
    }

    private void InitialSetup()
    {
        //добавляет белые фигуры на доску
        AddPiece(whiteRook, white, 0, 0);
        AddPiece(whiteKnight, white, 1, 0);
        AddPiece(whiteBishop, white, 2, 0);
        AddPiece(whiteQueen, white, 3, 0);
        AddPiece(whiteKing, white, 4, 0);
        AddPiece(whiteBishop, white, 5, 0);
        AddPiece(whiteKnight, white, 6, 0);
        AddPiece(whiteRook, white, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
        }
        //добавляет чёрные
        AddPiece(blackRook, black, 0, 7);
        AddPiece(blackKnight, black, 1, 7);
        AddPiece(blackBishop, black, 2, 7);
        AddPiece(blackQueen, black, 3, 7);
        AddPiece(blackKing, black, 4, 7);
        AddPiece(blackBishop, black, 5, 7);
        AddPiece(blackKnight, black, 6, 7);
        AddPiece(blackRook, black, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
        }
    }

    //создать фигуру из префаба

    public void AddPiece(GameObject prefab, Player player, int col, int row) //метод принимает префаб и игрока
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }


    //подсветить выбранные области
    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        //проверка что не null
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece); //вызывает метод
        }
    }

    //вычисляет все возможные позиции для хода занной фигуры

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        // убрать то что за пределами доски
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // убирает занятое союзником
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

    // метод выполняет перемещение фигуры на указанную позицию на сетке
    public void Move(GameObject piece, Vector2Int gridPoint)
    {   
        //проверяет является ли фигура пешкой и была ли она ранее перемещена, добавляет её в список movedPawns
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        // обновдяет внутреннее представление фигур на доске
        // визуально перемезает фигуру на  доске с помощью board.MovePiece
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
    }

    // отмечает пешку перемещенной
    public void PawnMoved(GameObject pawn)
    {
        movedPawns.Add(pawn);
    }

   // проверяет перемещена ли пешка
    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }
  
   

    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        if (pieceToCapture == null)
        {
            Debug.LogWarning("Нет фигуры для захвата на позиции " + gridPoint);
            return; 
        }

        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Debug.Log(currentPlayer.name + " Ура Победа!");
            winUI.SetActive(true);
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
        }
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;

        DestroySound.PlayOneShot(DestroySound.clip);
        Destroy(pieceToCapture);
    }


    //выбрать клетку
    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    //убрать выделение
    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }
    
    //принадлежит ли фигура текущему игроку
    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    //проверка отсутствия выхода за пределы поля
    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }


   // метод находит и возвращает координаты сетки для заданной фигуры
    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    //проверяет есть ли союзная фигура на указанной клетке
    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null) {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }
    
    //передать управление другому игроку
    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
    }
}
