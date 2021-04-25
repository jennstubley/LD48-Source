using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindsText : MonoBehaviour
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
        text.text = string.Format("Progress:\nSmall Finds: <b>{0} / {1}</b>\nLarge Finds: <b>{2} / {3}</b>", digger.SmallFound, digger.SmallTotal, digger.LargeFound, digger.LargeTotal);
    }
}
