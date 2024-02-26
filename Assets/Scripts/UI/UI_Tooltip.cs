using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{

    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 640;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;
        
        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }


    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
        {
            _text.fontSize = _text.fontSize * 0.8f;
        }
    }
}
