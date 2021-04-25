using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallItem : MonoBehaviour
{
    public NotifyPanel NotifyPanel;
    public string Name;
    public int Reward;
    private Digger digger;
    private bool hasTriggered = false;
    private Layer layer;

    // Start is called before the first frame update
    void Start()
    {
        digger = FindObjectOfType<Digger>();
        layer = transform.parent.GetComponentInChildren<Layer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasTriggered && CheckDiscovered())
        {
            hasTriggered = true;
            digger.UpdateFunds(Reward);
            NotifyPanel.Notify("Found a " + Name + "! +$" + Reward);
            digger.RecordFind(false);
            Destroy(gameObject);
        }
    }

    private bool CheckDiscovered()
    {
        return layer.Uncovered(transform.position - layer.transform.parent.position, 10, 0.1f);
    }
}
