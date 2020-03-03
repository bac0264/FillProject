using UnityEngine;
using System.Collections;

public class Loang : IAlogism
{
    public void SetupAlogism(Dot dot, Dot[,] dots, int MAX_ROW, int MAX_COL)
    {
        int row = (int)dot.pos.y;
        int col = (int)dot.pos.x;
        int i, j;
        int count = 0;
        for (i = -1; i < 5; i++)
        {
            for (j = -1; j < 5; j++)
            {
                if (i != 0 || j != 0)
                {
                    if (row + i >= 0 && row + i < MAX_ROW && col + j >= 0 && col + j < MAX_COL)
                    { // Nếu ô nằm trong bàn
                        Debug.Log(row + i);
                        Debug.Log(col + j);
                        if (dots[col + j, row + i].color == dot.color)
                        {
                            // Nếu ô chưa được mở
                            //open_empty_pos(row + i, col + j);
                            dots[col + j, row + i].RemoveDot();
                            count++;
                        }
                    }
                }
            }
        }
        if (count != 0) dot.RemoveDot();
    }

}
