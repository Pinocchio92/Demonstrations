using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class Cell : MonoBehaviour ,IPointerDownHandler,IPointerEnterHandler,IPointerUpHandler
{
    SpriteRenderer renderer;
    CellState state = CellState.UNUSED;

    public char letter { get; private set; }
    public int row { get; private set; }
    public int coloumn { get; private set; }

    [SerializeField]
    TMP_Text letterText;
    [SerializeField]
    Color HighlightedColor;
    [SerializeField]
    Color UnusedColor;
    [SerializeField]
    Color UtilizedColor;



    public static event Action<Cell> OnCellClick;
    public static event Action<Cell> OnCellEnter;
    public static event Action<Cell> OnCellExit;




    public void InitializeCell( char _letter, int _row =-1 , int _col =-1 )
    {
        letter =char.ToUpper( _letter);
        row = _row;
        coloumn = _col;
        letterText.text = letter.ToString();
        if (_row == -1 || coloumn == -1)
        {
            SetState(CellState.NONE);
        }
        else
        SetState(CellState.UNUSED);
        TryGetComponent<SpriteRenderer>(out renderer);
    }

    public void SetState(CellState _state)
    {
        state = _state;
        if (renderer != null)
        {
            switch (_state)
            {
                case CellState.NONE:
                    break;
                case CellState.UNUSED:
                    renderer.color = UnusedColor;
                    break;
                case CellState.HIGHLIGHTED:
                    renderer.color = HighlightedColor;
                    break;
                case CellState.UTILIZED:
                    renderer.color = UtilizedColor;
                    break;
                default:
                    break;
            }
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == CellState.UNUSED)
            OnCellClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == CellState.UNUSED)
            OnCellEnter(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnCellExit(this);
    }
}

public enum CellState
{
    NONE = -1,
    UNUSED = 0,
    HIGHLIGHTED =1,
    UTILIZED=2
}

