using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum DragDirection { None, Up, Down, Left, Right }

public class CakeBox : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] private Slot _slot;
    [SerializeField] private List<CakeBase> l_cakesInBox;
    [SerializeField] private Cell _currentCell;
    [SerializeField] private Vector2 _lastMouseWorldPos;
    [SerializeField] private DragDirection dragDirection;
    [SerializeField] private bool isInTray;


    private Vector3 _offset;
    private Camera _cam;

    public List<CakeBase> L_CakesInBox => l_cakesInBox;
    public bool IsInTray => isInTray;

    private void Start()
    {
        _gameManager = GameManager.instance;
        InitCakeInBox();
    }

    private void OnMouseDown()
    {
        if (!isInTray) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(_cam.transform.position, transform.position);
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);

        _offset = transform.position - mouseWorldPos;

        StartInTray();
    }

    private void OnMouseDrag()
    {
        if (!isInTray) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(_cam.transform.position, transform.position);
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);

        Vector2 delta = (Vector2)mouseWorldPos - _lastMouseWorldPos;

        const float threshold = 0.01f;
        if (delta.magnitude < threshold)
        {
            dragDirection = DragDirection.None;
        }
        else
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                dragDirection = delta.x > 0 ? DragDirection.Right : DragDirection.Left;
            else
                dragDirection = delta.y > 0 ? DragDirection.Up : DragDirection.Down;
        }

        _lastMouseWorldPos = mouseWorldPos;

        transform.position = new Vector3(
            mouseWorldPos.x + _offset.x,
            mouseWorldPos.y + _offset.y,
            transform.position.z
        );

        CheckCells();
    }

    private void OnMouseUp()
    {
        if (!isInTray) return;

        if (isHaveCakeInCell())
        {
            RestartInTray();
        }
        else
        {
            SetCakes();
            CheckCakesCanMerge();
            ClearCakeBox();
            ProcessEmptyTray();
        }
    }

    private void InitCakeInBox()
    {
        _cam = Camera.main;

        if (l_cakesInBox.Count <= 0) return;

        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            l_cakesInBox[i].initDataCake();
        }
    }

    private void CheckCakesCanMerge()
    {
        foreach (var Cake in l_cakesInBox)
        {
            Cake.CheckCakeCanMerge();
        }
    }

    private void CheckCells()
    {
        switch (dragDirection)
        {
            case DragDirection.None:
                break;
            case DragDirection.Up:
                CheckCellUp();
                break;
            case DragDirection.Down:
                CheckCellDown();
                break;
            case DragDirection.Left:
                CheckCellLeft();
                break;
            case DragDirection.Right:
                CheckCellRight();
                break;
            default:
                break;
        }
    }

    private void CheckCellUp()
    {
        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            l_cakesInBox[i].CheckCell();
        }
    }

    private void CheckCellDown()
    {
        for (int i = l_cakesInBox.Count - 1; i >= 0; i--)
        {
            l_cakesInBox[i].CheckCell();
        }
    }

    private void CheckCellLeft()
    {
        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            l_cakesInBox[i].CheckCell();
        }
    }

    private void CheckCellRight()
    {
        for (int i = l_cakesInBox.Count - 1; i >= 0; i--)
        {
            l_cakesInBox[i].CheckCell();
        }
    }


    private bool isHaveCakeInCell()
    {
        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            if (l_cakesInBox[i].isHaveCakeInCell()) return true;
        }

        return false;
    }

    private void SetCakes()
    {
        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            l_cakesInBox[i].SetCake();
        }
    }


    private void RestartInTray()
    {
        for (int i = 0; i < l_cakesInBox.Count; i++)
        {
            l_cakesInBox[i].ReStartPoint();
        }

        CakeReStartTray();
        transform.localPosition = new Vector3(0, 0, -2.5f);
    }

    private void StartInTray()
    {
        CakeStartTray();
        transform.localPosition = new Vector3(0, 0, -0.2f);
    }

    private void CakeStartTray()
    {
        foreach (var item in l_cakesInBox)
        {
            item.InitStartTray();
        }
    }

    private void CakeReStartTray()
    {
        foreach (var item in l_cakesInBox)
        {
            item.RestartInTray();
        }
    }

    private void ProcessEmptyTray()
    {
        if (_gameManager.TrayManager.isEmptyCakeInTray())
        {
            Debug.Log("Ramdom ra 3 box vao slot");
        }
    }

    private void ClearCakeBox()
    {
        _slot.ClearCakeBox();
        Destroy(gameObject);
    }
}
