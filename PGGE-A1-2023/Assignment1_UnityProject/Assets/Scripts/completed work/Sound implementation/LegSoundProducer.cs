using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegSoundProducer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Collider Foot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        print("stepping");
    }
}
