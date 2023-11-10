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
    void Update()
    {
        print(BoxCollider);
        print(audioSource);
        print(rigidbody2);
        print(meshFilter);
        print(meshRenderer);
    }
}
