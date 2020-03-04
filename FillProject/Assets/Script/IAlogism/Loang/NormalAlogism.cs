using UnityEngine;
using System.Collections;

public class NormalAlogism : IAlogism
{
    public void SetupAlogism(Dot dot, Dot[,] dots, int MAX_ROW, int MAX_CO)
    {
        dot.Check(dots);
    }
}
