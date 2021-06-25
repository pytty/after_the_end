using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBehaviour : MonoBehaviour
{
    [SerializeField] public SpecialResource res;
    public TMP_Text statText;
    public Image image;

    public void ChangeType(SpecialResource.SpecialResourceType newRes)
    {
        res.type = newRes;
        statText.text = res.type.ToString();
        Color newColor = new Color();

        //TO DO: tämä järkevämpään paikkaan
        if (res.type == SpecialResource.SpecialResourceType.MED)
        {
            newColor = Color.yellow;
        }
        else if (res.type == SpecialResource.SpecialResourceType.MOVE)
        {
            newColor = Color.green;
        }

        image.color = newColor;
    }

    public void ClickFPPool()
    {
        //TO DO: paskasti toteutettu
        GameObject.Find("GameManager").GetComponent<UIManager>().ClickFPPool(statText.text);
    }
}
