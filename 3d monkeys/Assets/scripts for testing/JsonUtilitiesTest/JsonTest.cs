using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class JsonTest : MonoBehaviour
{
    [Header("Drag an Asset instance in here.")]
    public BagOData PreMade;

    [Header("Leave this blank, a transient one will be made.")]
    [Header("Once made you can doubleclick to browse it while running.")]
    [Header("Object is gone when you press stop.")]
    public BagOData Generated;



    void Start()
    {
        PreMade = new BagOData();
        Generated = new BagOData();

        PreMade.data = new Dictionary<string, int>();
        PreMade.data.Add("hello", 1);
        PreMade.data.Add("monke", 2);
        PreMade.data.Add("o", 3);
        PreMade.data.Add("Yo", 4);
        string s = JsonUtility.ToJson(PreMade, prettyPrint: true);

        Debug.Log(s);

        // you must fabricate this scriptable object

        Generated.name = "MyNameIsGenerated";
        Generated.data = new Dictionary<string, int>();
        Generated.data.Add("asdf", 10);
        Generated.data.Add("sdd", 12);
        Generated.data.Add("asdfsd", 13);
        Generated.data.Add("asdsdaf", 15);
        // then populate its data
        JsonUtility.FromJsonOverwrite(s, Generated);
        Debug.Log(s);



    }
}


public class BagOData
{
    public string name;
    public Dictionary<string, int> data;
}