using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Layer : MonoBehaviour
{
    private SpriteMask mask;
    private Color[] colors;
    private Sprite picture;
    private Color[] pictureColors;

    // Start is called before the first frame update
    void Start()
    {
        mask = GetComponent<SpriteMask>();
        picture = transform.parent.gameObject.transform.Find("picture").GetComponent<SpriteRenderer>().sprite;
        pictureColors = picture.texture.GetPixels((int)picture.rect.x, (int)picture.rect.y, 400,300);
        Texture2D text = new Texture2D(400, 300);
        mask.sprite = Sprite.Create(text, new Rect(0, 0, 400, 300), new Vector2(0.5f, 0.5f), 32);
        for (int i = 0; i < 400; i++)
        {
            for (int j = 0; j < 300; j++)
            {
                mask.sprite.texture.SetPixel(i, j, Color.clear);
            }
        }
        mask.sprite.texture.Apply(false);
        colors = mask.sprite.texture.GetPixels();
    }

    public void DrawOnMask(Vector2 coords, int r)
    {
        //Normalize to the texture coodinates
        int cy = (int)(coords.y * 32) + 150;
        int cx = (int)(coords.x * 32) + 200;

        int px, nx, py, ny, d, a, b, c, e;

        for (int x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));

            for (int y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                a = Mathf.Min(colors.Length - 1, Mathf.Max(0, py * 400 + px));
                b = Mathf.Min(colors.Length - 1, Mathf.Max(0, py * 400 + nx));
                c = Mathf.Min(colors.Length - 1, Mathf.Max(0, ny * 400 + px));
                e = Mathf.Min(colors.Length - 1, Mathf.Max(0, ny * 400 + nx));

                colors[a] = new Color(1, 1, 1, 1);
                colors[b] = new Color(1, 1, 1, 1);
                colors[c] = new Color(1, 1, 1, 1);
                colors[e] = new Color(1, 1, 1, 1);
            }
        }

        mask.sprite.texture.SetPixels(colors);
        mask.sprite.texture.Apply(false);
    }

    public bool HasPicture(int imagIdx)
    {
        return pictureColors[imagIdx].a != 0;
    }

    public bool Uncovered(int imagIdx)
    {
        return colors[imagIdx].Equals(Color.white);
    }

    public bool Uncovered(Vector2 coords, int r, float percent = 1.0f)
    {
        //Normalize to the texture coodinates
        int cy = (int)(coords.y * 32) + 150;
        int cx = (int)(coords.x * 32) + 200;


        int px, nx, py, ny, d, a, b, c, e;

        int stillCovered = 0;
        float stillCoveredMax = percent * Mathf.PI * r * r;
        for (int x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));

            for (int y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                a = Mathf.Min(colors.Length - 1, Mathf.Max(0, py * 400 + px));
                b = Mathf.Min(colors.Length - 1, Mathf.Max(0, py * 400 + nx));
                c = Mathf.Min(colors.Length - 1, Mathf.Max(0, ny * 400 + px));
                e = Mathf.Min(colors.Length - 1, Mathf.Max(0, ny * 400 + nx));

                if (colors[a].a != 1 ||
                colors[b].a != 1 || 
                colors[c].a != 1 ||
                colors[e].a != 1)
                {
                    stillCovered+=4;
                    if (percent >= 1.0f) return false;
                    else if (stillCovered >= stillCoveredMax) return false;
                }
            }
        }
        return stillCovered / (r*r*Mathf.PI) < percent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
