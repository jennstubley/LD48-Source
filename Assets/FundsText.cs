using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FundsText : MonoBehaviour
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
        text.text = "Funds: $" + (int)digger.Funds;
    }
}
