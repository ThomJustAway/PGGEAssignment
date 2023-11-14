using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponentEveryUpdate : MonoBehaviour
{

    BoxCollider2D BoxCollider;
    AudioSource audioSource;
    Rigidbody2D rigidbody2;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    public int repeat = 1;
    // Update is called once per frame
    void Update()
    {
        //Test1();
        Test2();
    }

    private void Test2()
    {
        for(int i = 0; i < repeat; i++)
        {
            rigidbody2= GetComponent<Rigidbody2D>();
            print(rigidbody2);
        }
    }

    private void Test1()
    {
        GetComponentOfElement();
        PrintingComponent();
    }

    private void PrintingComponent()
    {
        print(BoxCollider);
        print(audioSource);
        print(rigidbody2);
        print(meshFilter);
        print(meshRenderer);
    }

    void GetComponentOfElement()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
