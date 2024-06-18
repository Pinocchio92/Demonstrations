using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawer : MonoBehaviour
{
    private LevelData currentLevelData;

    [SerializeField]
    GameObject cellPrefab;

    #region MonoBehaviour
    private void OnEnable()
    {
        GameManager.Instance.OnLevelDataLoaded += Instance_OnLevelDataLoaded;
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnLevelDataLoaded -= Instance_OnLevelDataLoaded;
        GameManager.Instance.OnGameStateChanged -= Instance_OnGameStateChanged;
    }
    #endregion

    #region local methods
    private void GenerateGrid(LevelData leveldata)
    {
        currentLevelData = leveldata;
        if (currentLevelData != null)
        {
            Rect gridBoundry = new Rect();
            gridBoundry.center = Vector3.zero;
            gridBoundry.size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-30 , Screen.height-30 ))*2;
            Vector3 cellScale = CalculateCellScale(gridBoundry);
            float XOffset = CalculateXOffset(gridBoundry, cellScale);
            Cell[,] grid = new Cell [currentLevelData.rows , currentLevelData.columns];
            for (int i = 0; i < currentLevelData.rows; i++)
            {
                for (int j = 0; j < currentLevelData.columns; j++)
                {
                    var cell = CreateCell();
                    cell.gameObject.SetActive(true);
                    cell.InitializeCell(currentLevelData.data[i][j], j ,i); 
                    cell.transform.localScale = cellScale;
                    cell.transform.localPosition =  new Vector3( (j * cellScale.x) - (gridBoundry.size.x/2)  + (cellScale.x/2) + XOffset,  ( -i * cellScale.y) + cellScale.y/2, 0);
                    grid[i, j] = cell;
                }
            }
        }
    }

    Cell CreateCell()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).TryGetComponent<Cell>(out var cell))
            {
                return cell;
            }
        }
        return Instantiate(cellPrefab, transform).GetComponent<Cell>();
    }

    void ReleaseGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private Vector3 CalculateCellScale( Rect boundry)
    {
        
        var cellRect = cellPrefab.transform.localScale;
        while (cellRect.x * currentLevelData.columns > boundry.size.x || cellRect.y * currentLevelData.rows> boundry.size.y )
        {
            cellRect *= .95f;
        }
        return cellRect;
    }
    private float CalculateXOffset(Rect _boundry , Vector3 cellScale)
    {
       return Mathf.Abs((currentLevelData.columns * cellScale.x) - (_boundry.size.x)) / 2;

    }
    #endregion

    #region ActionCallbacks
    private void Instance_OnLevelDataLoaded(LevelData obj)
    {
        GenerateGrid(obj);
    }
    private void Instance_OnGameStateChanged(GameState state)
    {
        if (state == GameState.LevelCompleted)
            ReleaseGrid(); 
    }

    #endregion
}
