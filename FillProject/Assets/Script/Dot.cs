using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class Dot : MonoBehaviour
{
    public int index;
    public int col;
    public int row;
    public SpriteRenderer dot;
    CircleCollider2D colider;
    public int marked;
    public int color;
    // Setup Dot
    #region
    public void Setup(int column, int row)
    {
        SetupPos(column, row);
        SetupColor();
        SetupColider();
    }
    void SetupPos(int col, int row)
    {
        // pos.x index colum, pos.y index row
        this.col = col;
        this.row = row;
        name = "dot[" + col + "," + row + "]";
    }
    void SetupColor()
    {
        if (dot == null)
            dot = GetComponent<SpriteRenderer>();
        int random = Random.Range(0, 3);
        if (random == 0)
        {
            color = 0;
            dot.color = Color.red;
        }
        else if (random == 1)
        {
            color = 1;
            dot.color = Color.blue;
        }
        else
        {
            dot.color = Color.green;
            color = 2;
        }
        dot.enabled = true;

    }
    void SetupColider()
    {
        if (colider == null)
            colider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
    }
    #endregion
    // Check relative dot
    public void Arranging(Dot _dot)
    {
        StartCoroutine(Move(_dot));
    }
    IEnumerator Move(Dot _dot)
    {
        Vector2 oldPos = _dot.transform.position;
        Tween tween = _dot.transform.DOLocalMoveY(this.transform.position.y, 0.3f);
        // yield return new WaitForSeconds(1f);
        yield return tween.WaitForCompletion();
        dot.color = _dot.dot.color;
        color = _dot.color;
        _dot.color = -1;
        _dot.dot.enabled = false;
        _dot.dot.transform.position = oldPos;
        dot.enabled = true;
        //yield return null;
        //Destroy(_dot);
    }
    public void RemoveOldAndSetupNewDot()
    {
        dot.enabled = false;
        color = -1;
        marked = 0;
        //Destroy(this);
    }
    public void Check(Dot[,] dots)
    {
        marked = 1;
        checkRight(dots);
        checkLeft(dots);
        checkUp(dots);
        checkDown(dots);
        RemoveOldAndSetupNewDot();
        //Debug.Log(this.index);
    }
    public void checkRight(Dot[,] dots)
    {
        int col = this.col;
        int row = this.row + 1;

        if (row >= BoardGame.MAX_ROW) return;
        // Debug.Log(dots[col, row].marked);
        if (dots[col, row].marked != 1 && dots[col, row].color == color && color != -1)
        {
            dots[col, row].Check(dots);
        }
    }
    public void checkLeft(Dot[,] dots)
    {
        int col = this.col;
        int row = this.row - 1;

        if (row < 0) return;
        if (dots[col, row].marked != 1 && dots[col, row].color == color)
        {
            dots[col, row].Check(dots);
        }
    }
    public void checkUp(Dot[,] dots)
    {
        int col = this.col - 1;
        int row = this.row;

        if (col < 0) return;
        if (dots[col, row].marked != 1 && dots[col, row].color == color)
        {
            dots[col, row].Check(dots);
        }
    }
    public void checkDown(Dot[,] dots)
    {
        int col = this.col + 1;
        int row = this.row;

        if (col >= BoardGame.MAX_COL) return;
        if (dots[col, row].marked != 1 && dots[col, row].color == color)
        {
            dots[col, row].Check(dots);
        }
    }
}
