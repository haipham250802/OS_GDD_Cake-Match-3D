using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum E_TypeCake
{
    Cake_1, Cake_2, Cake_3, Cake_4, Cake_5, Cake_6, Cake_7
}
public class CakeBase : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] private E_TypeCake e_typeCake;
    [SerializeField] private TMP_Text Point_txt;
    [SerializeField] private Cell _currentCell;
    [SerializeField] private MeshFilter _meshFiter;
    [SerializeField] private MeshRenderer _meshRenderer;



    public E_TypeCake E_typeCake => e_typeCake;
    public Cell CurrentCell => _currentCell;

    private void Start()
    {
        _gameManager = GameManager.instance;

        initDataCake();
    }

    public void initDataCake()
    {
        if (!_meshFiter) _meshFiter = GetComponent<MeshFilter>();
        if (!_meshRenderer) _meshRenderer = GetComponent<MeshRenderer>();

        CAKE cake = GameManager.instance.DataManager.DataCake.getCakeByType(e_typeCake);

        if (cake != null)
        {
            e_typeCake = cake._TypeCake;
            _meshFiter.mesh = cake._Mesh;
            _meshRenderer.sharedMaterial = cake._Material;
            Point_txt.text = cake.PointLevel.ToString();
            transform.localScale = Vector3.one * 0.1f;
            transform.localRotation = Quaternion.Euler(new Vector3(90, -90, 90));
        }
    }

    public void InitIncreaseCake()
    {
        int next = (int)e_typeCake + 1;
        e_typeCake = (E_TypeCake)next;
        initDataCake();

        CheckCakeCanMerge();
    }

    public void CheckCakeCanMerge()
    {
        if (!CurrentCell) return;

        int cx = _currentCell.x;
        int cy = _currentCell.y;

        for (int x = 0; x < 6; x++)
        {
            Cell cell = _gameManager.GridManager.GetCell(x, cy);
            if (cell == null) continue;

            if (cell.isHaveCake() && cell != _currentCell)
            {
                if (cell.IsSameTypeCakeCanMerge(_currentCell.CurrentCakeInCell))
                {
                    if (cell.CurrentCakeInCell.isCakeCanMergeIncrease())
                    {
                        Debug.Log($"Merge Vertical Nor: ({cell.x}, {cell.y})");
                        _currentCell.InscreaseCakeInCell(cell.CurrentCakeInCell);
                    }
                    else
                    {
                        Debug.Log($"Merge Vertical DontNor: ({cell.x}, {cell.y})");
                        cell.InscreaseCakeInCell(_currentCell.CurrentCakeInCell);
                    }
                    return;
                }
            }
        }

        for (int y = 0; y < 6; y++)
        {
            Cell cell = _gameManager.GridManager.GetCell(cx, y);
            if (cell == null) continue;

            if (cell.isHaveCake() && cell != _currentCell)
            {
                if (cell.IsSameTypeCakeCanMerge(_currentCell.CurrentCakeInCell))
                {
                    if (cell.CurrentCakeInCell.isCakeCanMergeIncrease())
                    {
                        Debug.Log($"Merge Hori Nor: ({cell.x}, {cell.y})");
                        _currentCell.InscreaseCakeInCell(cell.CurrentCakeInCell);
                    }
                    else
                    {
                        Debug.Log($"Merge Hori DontNor: ({cell.x}, {cell.y})");
                        cell.InscreaseCakeInCell(_currentCell.CurrentCakeInCell);
                    }
                    return;
                }
            }
        }
    }

    public bool isCakeCanMergeIncrease()
    {
        if (!CurrentCell) return false;

        int cx = _currentCell.x;
        int cy = _currentCell.y;

        for (int x = 0; x < 6; x++)
        {
            Cell cell = _gameManager.GridManager.GetCell(x, cy);
            if (cell == null) continue;

            if (cell.isHaveCake() && cell != _currentCell)
            {
                if (_currentCell.isSameTypeCakeCanMergeIncrease(cell.CurrentCakeInCell))
                {
                    Debug.Log($"Merge Increase: ({cell.x}, {cell.y})");
                    return true;
                }
            }
        }

        for (int y = 0; y < 6; y++)
        {
            Cell cell = _gameManager.GridManager.GetCell(cx, y);
            if (cell == null) continue;

            if (cell.isHaveCake() && cell != _currentCell)
            {
                if (_currentCell.isSameTypeCakeCanMergeIncrease(cell.CurrentCakeInCell))
                {
                    Debug.Log($"Merge Increase: ({cell.x}, {cell.y})");
                    return true;
                }
            }
        }

        return false;
    }


    public E_TypeCake GetNextCakeType(E_TypeCake type)
    {
        int next = (int)type + 1;
        if (next > (int)E_TypeCake.Cake_6)
            next = (int)E_TypeCake.Cake_6;

        return (E_TypeCake)next;
    }


    public void GotoMerge(CakeBase Cake)
    {
        transform.DOMove(Cake.transform.position, 0.2f).OnComplete(() =>
        {
            Cake.InitIncreaseCake();
            gameObject.SetActive(false);
        });
    }

    public void CheckCell()
    {
        Ray ray = new Ray(transform.position, - transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Cell hitCell = hit.collider.GetComponent<Cell>();
            if (hitCell == null) return;

            if (_currentCell)
            {
                if (hitCell != _currentCell && hitCell.isHaveCake() && !_currentCell.isHaveCake())
                {
                    _currentCell.ExitCell();
                    _currentCell = null;
                }
            }

            if (hitCell != _currentCell && !hitCell.isHaveCake())
            {
                if (_currentCell != null)
                {
                    _currentCell.ExitCell();
                }

                _currentCell = hitCell;

                if (!_currentCell.isHaveCake())
                {
                    _currentCell.EnterCell();
                }
            }
        }
        else
        {
            if (_currentCell != null)
            {
                _currentCell.ExitCell();
                _currentCell = null;
            }
        }
    }

    public bool isHaveCakeInCell()
    {
        Ray ray = new Ray(transform.position, - transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Cell cell = hit.collider.GetComponent<Cell>();

            if (cell != null)
            {
                if (cell.isHaveCake())
                {
                    return true;
                }

                SetCell(cell);
                return false;
            }
        }
        return true;
    }

    private void SetCell(Cell cell)
    {
        _currentCell = cell;
    }

    public void SetCake()
    {
        _currentCell.isSetCake(this);
    }

    public void ReStartPoint()
    {
        if (!_currentCell) return;
        if (_currentCell.isHaveCake())
        {
            _currentCell = null;
            return;
        }

        _currentCell.ExitCell();
        _currentCell = null;
    }
}
