using UnityEngine;

public class PlayParticleSystemAnimationEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void Play() {
        _particleSystem.Play();
    }
}
