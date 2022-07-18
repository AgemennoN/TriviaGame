using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    private Canvas OptionCanvas;
    private Button OptionButton;
    
    void Start()
    {
        OptionCanvas = GameObject.Find("OptionCanvas").GetComponent<Canvas>();
        OptionButton = gameObject.GetComponent<Button>();
        OptionButton.onClick.AddListener(EnableOptionCanvas);
    }

    private void EnableOptionCanvas()
    {
        OptionCanvas.enabled = true;
    }

}
