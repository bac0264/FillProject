using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
    public Vector2 pos;
    SpriteRenderer dot;
    CircleCollider2D colider;
    public int color;
    // Setup Dot
    #region
    public void Setup(Vector2 pos)
    {
        SetupPos(pos);
        SetupColor();
        SetupColider();
    }
    void SetupPos(Vector2 pos)
    {
        // pos.x index colum, pos.y index row
        this.pos = pos;
    }
    void SetupColor()
    {
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

    }
    void SetupColider()
    {
        colider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
    }
    #endregion
    // Check relative dot
    public void RemoveDot()
    {
        dot.enabled = false;
        color = -1;
    }
}
