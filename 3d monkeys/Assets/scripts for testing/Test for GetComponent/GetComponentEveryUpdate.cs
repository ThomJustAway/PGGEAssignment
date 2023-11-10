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

    // Update is called once per frame
    void Update()
    {
        GetComponentOfElement();
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
