using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes;
    public Tilemap tilemap {get; private set;}
    public Piece activePiece {get; private set;}
    public Vector3Int spawnPosition;

    public Vector2Int boardSize = new Vector2Int(10, 20);
    public RectInt Bounds{
        get{
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        } 
    }

    private void Awake(){
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        for(int i = 0; i < tetrominoes.Length; i++){
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start(){
        SpawnPiece();
    }

    public void SpawnPiece(){
        int index = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[index];
        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(activePiece);
    }

    public void Set(Piece piece){
        for(int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public bool isValidPosition(Piece piece, Vector3Int position){
        RectInt bounds = this.Bounds;
        for(int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + position;
            if(!bounds.Contains((Vector2Int)tilePosition)){
                return false;
            }
            if(this.tilemap.HasTile(tilePosition)){
                return false;
            }
        }
        return true;
    }

    
    public void Clear(Piece piece){
        for(int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void ClearLines(){

        RectInt bound = this.Bounds;
        int row = Bounds.yMin; 

        while (row < bound.yMax){

            if(IsLineFull(row)){
                LineClear(row);
            } else{

                row++;
            }

        }

    }

    private bool IsLineFull(int row){
        
        RectInt bound = this.Bounds;

        for(int col = bound.xMin; col < Bounds.xMax; col++){

            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position)){
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row){
        RectInt bound = this.Bounds;

        for(int col = bound.xMin; col < Bounds.xMax; col++){

            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);

        }

        while(row < Bounds.yMax){
            for( int col = Bounds.xMin; col < Bounds.xMax; col++){

                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
                
            }

            row ++;

        }

    }


}
