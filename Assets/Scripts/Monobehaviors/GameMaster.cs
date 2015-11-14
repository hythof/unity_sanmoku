using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameMaster: MonoBehaviour
{
    [SerializeField]
    GameObject result;
    [SerializeField]
    Text message;

    const int PLAYER = 1;
    const int ENEMY = 2;
    Board board = new Board();
    Cell[] cells;

    void Start()
    {
        cells = gameObject.GetComponentsInChildren<Cell>();
        foreach(var cell in cells)
        {
            cell.Setup(this);
        }
        result.SetActive(false);
    }

    public void OnClick(int n)
    {
        if(hand(n, PLAYER))
        {
            hand(ai(), ENEMY);
        }
    }

    public void OnReset()
    {
        result.SetActive(false);
        board.Reset();
        Debug.Log("tap: Reset");
    }

    public Color GetColor(int n)
    {
        var v = board.Get(n);
        switch(v)
        {
            case PLAYER:
                return Color.blue;
            case ENEMY:
                return Color.red;
            default:
                return Color.white;
        }
    }

    void showMessage(string text)
    {
        result.SetActive(true);
        message.text = text;
    }

    bool hand(int n, int v)
    {
        if(!board.Set(n, v))
        {
            Debug.LogError("invalid set n=" + n);
            return false;
        }

        Debug.Log("tap: player=" + n + " v=" + v + " " + board.ToString());
        return !fin();
    }

    bool fin()
    {
        var v = board.Judge();
        if (v == PLAYER)
        {
            showMessage("Win");
            return true;
        }
        else if (v == ENEMY)
        {
            showMessage("Lose");
            return true;
        }
        else if (!board.Continue())
        {
            showMessage("Draw");
            return true;
        }
        return false;
    }

    int ai()
    {
        var x = board.Vacancy();
        return x.First();
    }
}