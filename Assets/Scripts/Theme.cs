using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Cycle of Life/Theme")]
public class Theme : ScriptableObject
{
   public Tile aliveVisual;
   public Tile deadVisual;
   public Color backgroundColor = new Color(77f,77f,77f, 255f);
}
