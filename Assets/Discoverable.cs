using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discoverable : MonoBehaviour
{
    public bool Discovered;
    public NotifyPanel NotifyPanel;
    public string Name;
    public int Reward = 50;
    private Digger digger;
    private Sprite sprite;
    private Layer[] layers;
    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        digger = FindObjectOfType<Digger>();
        sprite = GetComponent<SpriteRenderer>().sprite;
        layers = digger.GetLayers();
        colors = sprite.texture.GetPixels();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Discovered && CheckDiscovered())
        {
            NotifyPanel.Notify("Discovered " + Name + "! +$" + Reward); 
            Discovered = true;
            digger.UpdateFunds(Reward);
            digger.RecordFind(true);
        }
    }

    private bool CheckDiscovered()
    {
        int stillCoveredCount = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].a == 0) continue;

            for (int ilayer = 0; ilayer < layers.Length; ilayer++)
            {
                if (layers[ilayer].HasPicture(i))
                {
                    if (ilayer > 0 && !layers[ilayer-1].Uncovered(i))
                    {
                        stillCoveredCount++;
                        if (stillCoveredCount >= 5)
                        {
                            return false; // not discovered
                        }
                        break;
                    }
                    else
                    {
                        // Continue to the next pixel since we found
                        // drawings on this layer.
                        break;
                    }
                }
                //  else continue to the next layer.
            }
        }

        return true;
    }
}
