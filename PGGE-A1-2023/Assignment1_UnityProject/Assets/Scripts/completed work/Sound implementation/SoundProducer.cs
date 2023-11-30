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

    #region  range of audio
    //adjusting the min/ max volumn range
    [SerializeField] private float minWalkVolumnRange = 0.1f;
    [SerializeField]private float maxWalkVolumnRange = 1.0f;
    
    //adjusting the min/max pitch range
    [SerializeField]private float minWalkPitchRange = 0.1f;
    [SerializeField] private float maxWalkPitchRange = 1.0f;

    //adjust the min/max volumn range of running
    [SerializeField] private float minRunningVolumnRange = 0.1f;
    [SerializeField] private float maxRunningVolumnRange = 1f;

    //adjust the min/max pitch range of running
    [SerializeField] private float minRunningPitchRange = 0.1f;
    [SerializeField] private float maxRunningPitchRange = 1f;

    #endregion

    #region steps walking
    [SerializeField] private AudioClip[] steppingAudioClipGrass;
    [SerializeField] private AudioClip[] steppingAudioClipConcrete;
    [SerializeField] private AudioClip[] steppingAudioClipGrassWeed;
    [SerializeField] private AudioClip[] steppingAudioClipStone;
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
            {//if the raycast hit, find it's meshrenderer component as it tells the material of the game object.
                //use a dictionary to find the surface enum based of the string of the material name
                if (keySurfaceValues.TryGetValue(renderer.material.name , out Surface surface))
                {
                    return surface;
                }
            }
        }
        return Surface.grass;
    }

    public void WalkingSoundPlay()
    {
        //vary the volumn and pitch of the walking animation
        float volumn = Random.Range(minWalkVolumnRange, maxWalkVolumnRange);
        float pitch = Random.Range(minWalkPitchRange, maxWalkPitchRange);
        PlaySteppingSound(volumn, pitch);
    }
    public void RunningSoundPlay()
    {
        //vary the volumn and pitch of the running animation
        float volumn = Random.Range(minRunningVolumnRange, maxRunningVolumnRange);
        float pitch = Random.Range(minRunningPitchRange, maxRunningPitchRange);
        PlaySteppingSound(volumn, pitch);
    }

    private void PlaySteppingSound(float volumn, float pitch)
    {
        Surface surface = GetsurfaceOfPlayer();
        AudioClip audioClip = GetAudioClip(surface);
        audioSource.clip = audioClip;
        audioSource.pitch = pitch;
        audioSource.volume = volumn;
        audioSource.Play();
    }

    private AudioClip GetAudioClip(Surface surface)
    {
        AudioClip audioClip;
        switch (surface)
        {
            case Surface.grass:
                audioClip = steppingAudioClipGrass[Random.Range(0, steppingAudioClipGrass.Length - 1)];
                break;
            case Surface.concrete:
                audioClip = steppingAudioClipConcrete[Random.Range(0, steppingAudioClipConcrete.Length - 1)];
                break;
            case Surface.grassWeed:
                audioClip = steppingAudioClipGrassWeed[Random.Range(0, steppingAudioClipGrassWeed.Length - 1)];
                break;
            case Surface.stone:
                audioClip = steppingAudioClipStone[(Random.Range(0, steppingAudioClipStone.Length - 1))];
                break;
            default:
                audioClip = null;
                break;
        }

        return audioClip;
    }

}
