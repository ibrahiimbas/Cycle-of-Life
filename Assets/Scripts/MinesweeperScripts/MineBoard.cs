using UnityEngine;
using UnityEngine.Tilemaps;

public class MineBoard : MonoBehaviour
{
   public Tilemap tilemap {get; private set;}

   public Tile tileUnknown;
   public Tile tileEmpty;
   public Tile tileMine;
   public Tile tileExploded;
   public Tile tileFlag;
   public Tile tileNum1;
   public Tile tileNum2;
   public Tile tileNum3;
   public Tile tileNum4;
   public Tile tileNum5;
   public Tile tileNum6;
   public Tile tileNum7;
   public Tile tileNum8;
   
   private void Awake()
   {
      tilemap = GetComponent<Tilemap>();
   }

   public void Draw(MineCell[,] state)
   {
      int width = state.GetLength(0);
      int height = state.GetLength(1);

      for (int x = 0; x < width; x++)
      {
         for (int y = 0; y < height; y++)
         {
            MineCell cell = state[x, y];
            tilemap.SetTile(cell.position, GetTile(cell));
         }
      }
   }

   private Tile GetTile(MineCell cell)
   {
      if (cell.revealed)
      {
         return GetRevealedTile(cell);
      }
      else if (cell.flagged)
      {
         return tileFlag;
      }
      else
      {
         return tileUnknown;
      }
   }

   private Tile GetRevealedTile(MineCell cell)
   {
      switch (cell.type)
      {
         case MineCell.Type.Empty: return tileEmpty;
         case MineCell.Type.Mine: return cell.exploded ? tileExploded : tileMine;
         case MineCell.Type.Number: return GetNumberTile(cell);
         default: return null;
      }
   }

   private Tile GetNumberTile(MineCell cell)
   {
      switch (cell.number)
      {
         case 1: return tileNum1;
         case 2: return tileNum2;
         case 3: return tileNum3;
         case 4: return tileNum4;
         case 5: return tileNum5;
         case 6: return tileNum6;
         case 7: return tileNum7;
         case 8: return tileNum8;
         default: return null;
      }
   }
   
}
