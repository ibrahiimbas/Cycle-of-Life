using UnityEngine;

[CreateAssetMenu(menuName = "Cycle of Life/MinesweeperSettings")]
public class MinesweeperGameSettings : ScriptableObject
{
  public int width;
  public int height;
  public int mineCount;
  public float cameraFov;
  public float cameraOffset;
}
