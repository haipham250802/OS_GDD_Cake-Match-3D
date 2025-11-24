using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private List<Cell> l_cells = new List<Cell>();
    public List<Cell> L_Cells => l_cells;


    [Button("Init Data Cell")]
    public void InitDataCell()
    {
        _grid = GetComponent<Grid>();

        l_cells.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).GetComponent<Cell>();
            if (cell != null)
                l_cells.Add(cell);
        }

        int total = l_cells.Count;
        if (total < 36)
        {
            Debug.LogError("Thiếu cell! Cần đủ 36 child (6x6).");
            return;
        }

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                int index = x * 6 + y;
                var cell = l_cells[index];

                cell.InitDataGrid(x, y);

                Vector3Int gridPos = new Vector3Int(x, y, 0);
                Vector3 pos = _grid.CellToWorld(gridPos);

                pos.y = -pos.y;

                cell.transform.position = pos;
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        for (int i = 0; i < l_cells.Count; i++)
        {
            if (l_cells[i].x == x && l_cells[i].y == y)
            {
                return l_cells[i];
            }
        }

        return null;
    }
}
