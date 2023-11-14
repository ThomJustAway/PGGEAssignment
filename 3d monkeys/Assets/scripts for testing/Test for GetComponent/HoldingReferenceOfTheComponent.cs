using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingReferenceOfTheComponent : MonoBehaviour
{
    // Start is called before the first frame update

   [SerializeField] private BoxCollider2D BoxCollider;
   [SerializeField] private AudioSource audioSource;
   [SerializeField] private Rigidbody2D rigidbody2;
   [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    public int repeat;
    void Update()
    {
        //Test1();
        Test2();
    }

    private void Test2()
    {
        for(int i = 0; i < repeat; i++)
        {
            print(rigidbody2);
        }
    }
    private void Test1()
    {
        print(BoxCollider);
        print(audioSource);
        print(rigidbody2);
        print(meshFilter);
        print(meshRenderer);
    }
}
