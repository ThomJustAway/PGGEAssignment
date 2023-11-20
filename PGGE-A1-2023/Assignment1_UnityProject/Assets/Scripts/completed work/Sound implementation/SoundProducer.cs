using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundProducer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private AudioSource audioSource;
    private Dictionary<string, Surface> keySurfaceValues = new Dictionary<string, Surface>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PopulateValues();
    }

    private void PopulateValues()
    {
        keySurfaceValues.Add("Grass pattern 01", Surface.grass);
        keySurfaceValues.Add("wall", Surface.concrete);
        keySurfaceValues.Add("Ground & rocks pattern 01", Surface.stone);
        keySurfaceValues.Add("Ground & weeds", Surface.grassWeed);
    }
    private enum Surface
    {
        grass,
        concrete,
        grassWeed,
        stone
    }

    private Surface GetsurfaceOfPlayer()
    {
        Ray ray = new Ray(playerTransform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 10f))
        {
            
            if(hit.collider.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                print(renderer.material.name);
                if (keySurfaceValues.TryGetValue(renderer.material.name,out Surface surface))
                {
                    print(surface);
                }
            }
        }
        return Surface.grass;
    }

    public void WalkingSoundPlay()
    {
        GetsurfaceOfPlayer();
    }

    public void RunningSoundPlay()
    {

    }
}
