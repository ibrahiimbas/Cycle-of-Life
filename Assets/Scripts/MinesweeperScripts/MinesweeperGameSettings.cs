using UnityEngine;

[CreateAssetMenu(menuName = "Cycle of Life/MinesweeperSettings")]
public class MinesweeperGameSettings : ScriptableObject
{
  public int widthHeight;
  public int mineCount;
  public float cameraFov;
}
