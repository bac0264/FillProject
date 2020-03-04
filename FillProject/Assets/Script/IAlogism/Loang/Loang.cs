using UnityEngine;
using System.Collections;

public class Loang : IAlogism
{
    public void SetupAlogism(Dot dot, Dot[,] dots, int MAX_ROW, int MAX_COL)
    {
        int row = dot.row;
        int col = dot.col;
        int i, j;
        dot.marked = 1;
        for (i = -20; i < 10; i++)
        {
            for (j = -20; j < 10; j++)
            {
                if (i != 0 || j != 0)
                {
                    if (row + i >= 0 && row + i < MAX_ROW && col + j >= 0 && col + j < MAX_COL)
                    { // Nếu ô nằm trong bàn
                        Debug.Log(row + i);
                        Debug.Log(col + j);
                        if (/*dots[col + j, row + i].marked != 1 && */dots[col + j, row + i].color == dot.color)
                        {
                            // Nếu ô chưa được mở
                            //open_empty_pos(row + i, col + j);
                            //dots[col + j, row + i].marked = 1;
                            //if (dots[col + j, row + i].color == dot.color)
                            //{
                                dots[col + j, row + i].RemoveOldAndSetupNewDot();
                            //}
                            //else dots[col + j, row + i].marked = 0;
                        }
                    }
                }
            }
        }
         dot.RemoveOldAndSetupNewDot();
    }

}
