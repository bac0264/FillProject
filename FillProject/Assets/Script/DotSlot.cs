using UnityEngine;
using System.Collections;

public class DotSlot : MonoBehaviour
{
    Vector2 pos;
    public void SetupPos(Vector2 pos)
    {
        // pos.x index colum, pos.y index row
        this.pos = pos;
    }
}
