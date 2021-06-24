using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPBehaviour : MonoBehaviour
{
    [SerializeField] public FP fp;
    public TMP_Text statText;
    public Image image;

    public void ChangeType(Hero.Stat newStat)
    {
        fp.stat = newStat;
        statText.text = fp.stat.ToString();
        Color newColor = new Color();

        //TO DO: tämä järkevämpään paikkaan
        if (fp.stat == Hero.Stat.STR)
        {
            newColor = Color.red;
        }
        else if (fp.stat == Hero.Stat.AGI)
        {
            newColor = Color.blue;
        }
        else if (fp.stat == Hero.Stat.WILL)
        {
            newColor = Color.white;
        }

        image.color = newColor;
    }

    public void ClickFPPool()
    {
        //TO DO: paskasti toteutettu
        GameObject.Find("GameManager").GetComponent<UIManager>().ClickFPPool(statText.text);
    }
}
