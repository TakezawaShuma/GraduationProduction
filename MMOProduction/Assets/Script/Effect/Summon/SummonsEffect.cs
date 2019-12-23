//
// SummonsEffect.cs
//
// Author: 
//

using UnityEngine;

public class SummonsEffect : MonoBehaviour
{

    private ParticleSystem _particle;


    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public void PlayEffect()
    {
        _particle.Play();
    }
}
