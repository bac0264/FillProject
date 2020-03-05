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
    public int fill;
    // Setup Dot
    #region
    public void Setup(int column, int row)
    {
        SetupPos(column, row);
        SetupColor();
        SetupColider();
        SetupFill();
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
    void SetupFill()
    {
        fill = 1;
    }
    #endregion
    // Check relative dot
    public void Arranging(Dot _dot, bool check)
    {
        StartCoroutine(Move(_dot, check));
    }
    IEnumerator Move(Dot _dot, bool check)
    {
        Vector2 oldPos = _dot.transform.position;
        _dot.dot.color = dot.color;
        _dot.dot.transform.position = dot.transform.position;
        _dot.color = color;
        _dot.fill = 1;
        _dot.dot.enabled = true;
        Tween tween = _dot.transform.DOLocalMoveY(oldPos.y, 0.3f);
        dot.enabled = false;
        color = -1;
        fill = 0;
        // yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => check);
        tween.Complete();
        //yield return null;
        //Destroy(_dot);
    }
    public void RemoveOldAndSetupNewDot()
    {
        dot.enabled = false;
        color = -1;
        marked = 0;
        fill = 0;
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
