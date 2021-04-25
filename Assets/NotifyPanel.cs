using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NotifyPanel : MonoBehaviour
{
    public GameObject NotifyPrefab;
    public float NotifyLength;
    private Dictionary<GameObject, float> timers;

    // Start is called before the first frame update
    void Start()
    {
        timers = new Dictionary<GameObject, float>();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> toRemove = new List<GameObject>();
        GameObject[] objs = timers.Keys.ToArray();
        foreach (var obj in objs)
        {
            timers[obj] = timers[obj] - Time.deltaTime;
            if (timers[obj]  <= 0)
            {
                toRemove.Add(obj);
            }
        }
        foreach (GameObject obj in toRemove)
        {
            timers.Remove(obj);
            Destroy(obj);
        }
    }

    public void Notify(string message)
    {
        GameObject obj = Instantiate(NotifyPrefab);
        obj.transform.Find("Text").GetComponent<Text>().text = message;
        timers.Add(obj, NotifyLength);
        obj.transform.SetParent(transform);

    }
}
