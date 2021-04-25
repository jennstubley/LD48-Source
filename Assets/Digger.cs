using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Digger : MonoBehaviour
{
    public GameObject GameOverPanel;
    private Layer[] layers;
    public int MaxToolLevel = 0;
    public int SmallFound = 0;
    public int LargeFound = 0;
    public int SmallTotal = 48;
    public int LargeTotal = 12;
    public float Funds { get; private set; }
    public float StartFunds;
    private int ToolLevel = 0;
    private Vector3 prevMousePos;

    // Start is called before the first frame update
    void Awake()
    {
        layers = FindObjectsOfType<Layer>();
        Funds = StartFunds;
        Array.Sort(layers, (Layer a, Layer b) => string.Compare(a.name, b.name));
        MaxToolLevel = layers.Length - 2;
    }


    // Update is called once per frame
    void Update()
    {
        if (Funds <= 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Set tool level to the highest uncovered layer under the cursor.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition -= layers[0].transform.parent.position;
            for (int i = 0; i <= MaxToolLevel; i++)
            {
                if (!layers[i].Uncovered(mousePosition, 10))
                {
                    ToolLevel = i;
                    break;
                }
            }
            prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MaybeDraw(mousePosition);
            if (prevMousePos - mousePosition != Vector3.zero)
            {
                MaybeDraw(Vector3.Lerp(mousePosition, prevMousePos, .25f));
                MaybeDraw(Vector3.Lerp(mousePosition, prevMousePos, .5f));
                MaybeDraw(Vector3.Lerp(mousePosition, prevMousePos, .75f));
            }


            prevMousePos = mousePosition;
        }
    }

    private void MaybeDraw(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            pos -= layers[0].transform.parent.position;
            //Draw onto the mask
            // Only draw on a layer if all previous layers are uncovered at that spot
            bool shouldDraw = true;
            for (int i = 0; i < ToolLevel; i++)
            {
                if (!layers[i].Uncovered(pos, 10))
                {
                    shouldDraw = false;
                    break;
                }
            }

            if (shouldDraw && Funds > 0)
            {
                if (!layers[ToolLevel].Uncovered(pos, 10, 0.2f))
                {
                    Funds -= 1f;
                }
                CheckGameOver();
                layers[ToolLevel].DrawOnMask(pos, 10);
            }
        }
    }

    public Layer[] GetLayers()
    {
        return layers;
    }

    private void CheckGameOver()
    {
        if (Funds <= 0)
        {
            GameOverPanel.SetActive(true);
        }
    }

   /* public void IncreaseMaxToolLevel()
    {
        if (Funds < ToolIncreasePrice())
        {
            return;
        }
        Funds -= ToolIncreasePrice();
        MaxToolLevel++;
    }*/

    public int ToolIncreasePrice()
    {
        return (MaxToolLevel + 1) * 100;
    }

    public void UpdateFunds(int amount)
    {
        Funds += amount;
    }

    public void RecordFind(bool large)
    {
        if (large)
        {
            LargeFound++;
        }
        else
        {
            SmallFound++;
        }
    }

    public void TryAgain()
    {
        Debug.Log("Restarting");
        SceneManager.LoadScene(0);
    }
}
