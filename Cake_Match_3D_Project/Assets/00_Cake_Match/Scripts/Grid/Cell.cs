using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    [SerializeField] private bool _isHaveCake;
    [SerializeField] private CakeBase _currentCakeInCell;
    [SerializeField] private MeshRenderer _renderer;

    [SerializeField] private Material _cubeMaterials;
    [SerializeField] private Material _cubePickupMaterials;


    public int x => _x;
    public int y => _y;
    public CakeBase CurrentCakeInCell => _currentCakeInCell;


    public void InitDataGrid(int x, int y)
    {
        _x = x;
        _y = y;

        InitDataCell();
    }

    private void InitDataCell()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void isSetCake(CakeBase cake)
    {
        _isHaveCake = true;
        _currentCakeInCell = cake;
        cake.transform.parent = transform;
        cake.transform.localPosition = new Vector3(0, 0, -0.55f);
    }

    public void EnterCell()
    {
        _renderer.material = _cubePickupMaterials;
    }

    public void ExitCell()
    {
        _renderer.material = _cubeMaterials;
    }

    public bool isHaveCake()
    {
        if (_isHaveCake && _currentCakeInCell)
        {
            return true;
        }

        return false;
    }

    public bool IsSameTypeCakeCanMerge(CakeBase cake)
    {
        if (!CurrentCakeInCell) return false;

        if (CurrentCakeInCell.E_typeCake == cake.E_typeCake)
        {
            return true;
        }

        return false;
    }

    public bool isSameTypeCakeCanMergeIncrease(CakeBase cake)
    {
        if (CurrentCakeInCell.GetNextCakeType(_currentCakeInCell.E_typeCake) == cake.E_typeCake)
        {
            return true;
        }

        return false;
    }

    public void InscreaseCakeInCell(CakeBase cake)
    {
        _currentCakeInCell.GotoMerge(cake);
        ClearCellAfterMerge();
    }

    private void ClearCellAfterMerge()
    {
        _isHaveCake = false;
        _currentCakeInCell = null;
        ExitCell();
    }
}
