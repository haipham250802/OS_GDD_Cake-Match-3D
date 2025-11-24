using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public DataManager DataManager;
    public GridManager GridManager;
    public TrayManager TrayManager;

    public Material Cube;
    public Material CubePickup;

    private void Awake()
    {
        instance = this;
    }
}
