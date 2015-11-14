using UnityEngine;
using UnityEngine.UI;

public class Cell: MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    int index;
    GameMaster gm;

    public void Setup(GameMaster gm)
    {
        this.gm = gm;
    }

    public void OnClick()
    {
        gm.OnClick(index);
    }

    void Update()
    {
        image.color = gm.GetColor(index);
    }
}
