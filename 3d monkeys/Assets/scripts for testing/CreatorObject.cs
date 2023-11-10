using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public int numberOfPrefab;
    void Start()
    {
        for(int i = 0; i < numberOfPrefab; i++)
        {
            Instantiate(prefab);
        }
    }
}
