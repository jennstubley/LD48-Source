using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolText : MonoBehaviour
{
    private Digger digger;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        digger = FindObjectOfType<Digger>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Tool Level: " + digger.MaxToolLevel + " Upgrade Cost: $" + digger.ToolIncreasePrice();
    }
}
