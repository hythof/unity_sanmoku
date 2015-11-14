using System.Linq;
using System.Collections.Generic;

public class Board
{
    readonly int[] map = new int[9];
    readonly int[] lines = new int[]
    {
        0, 1, 2,
        3, 4, 5,
        6, 7, 8,

        0, 3, 6,
        1, 4, 7,
        2, 5, 8,

        0, 4, 8,
        2, 4, 6,
    };

    public void Reset()
    {
        for (var i = 0; i < map.Length; ++i)
        {
            map[i] = 0;
        }
    }

    public bool Set(int n, int v)
    {
        if(map[n] != 0)
        {
            return false;
        }
        map[n] = v;
        return true;
    }

    public bool Continue()
    {
        return map.Any(x => x == 0);
    }

    public int Judge()
    {
        for(var i=0; i<lines.Length; i+=3)
        {
            var v1 = map[lines[i]];
            var v2 = map[lines[i+1]];
            var v3 = map[lines[i+2]];
            if (v1 != 0 &&  v1 == v2 && v1 == v3)
            {
                return v1;
            }
        }
        return 0;
    }

    public IEnumerable<int> Vacancy()
    {
        for(var i=0; i<map.Length; ++i)
        {
            if(map[i] == 0)
            {
                yield return i;
            }
        }
    }

    public int Get(int n)
    {
        return map[n];
    }

    public override string ToString()
    {
        return "board[" + string.Join(", ", map.Select(x => x.ToString()).ToArray()) + "]";
    }
}