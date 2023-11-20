using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundProducer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private AudioSource audioSource;
    private Dictionary<string, Surface> keySurfaceValues = new Dictionary<string, Surface>();

    #region walking clips and range of audio
    //adjusting the min/ max volumn range
    [SerializeField] private float minWalkVolumnRange = 0.1f;
    [SerializeField]private float maxWalkVolumnRange = 1.0f;
    
    //adjusting the min/max pitch range
    [SerializeField]private float minWalkPitchRange = 0.1f;
    [SerializeField] private float maxWalkPitchRange = 1.0f;

    #region grass walking
    [SerializeField] private AudioClip[] walkingAudioClipGrass;
    #endregion


    #endregion

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PopulateValues();
    }

    private void PopulateValues()
    {
        //have to add (Instance) at the back.
        keySurfaceValues.Add("Grass pattern 01 (Instance)", Surface.grass);
        keySurfaceValues.Add("wall (Instance)", Surface.concrete);
        keySurfaceValues.Add("Ground & rocks pattern 01 (Instance)", Surface.stone);
        keySurfaceValues.Add("Ground & weeds (Instance)", Surface.grassWeed);
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
        Vector3 offsetPosition = new Vector3(transform.position.x, 
            transform.position.y + 1f , 
            transform.position.z); //offset the height for raycasting 

        Ray ray = new Ray(offsetPosition, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            
            if(hit.collider.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                print(renderer.material.name);
                if (keySurfaceValues.TryGetValue(renderer.material.name , out Surface surface))
                {
                    print(surface);
                    return surface;
                }
            }
        }
        return Surface.grass;
    }

    public void WalkingSoundPlay()
    {
        Surface surface= GetsurfaceOfPlayer();

    }

    public void RunningSoundPlay()
    {

    }
}
