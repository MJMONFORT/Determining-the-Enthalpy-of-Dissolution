using UnityEngine;

public class OnDrop_PlayParticle : MonoBehaviour, IOnDropSuccess
{
    [SerializeField] ParticleSystem[] particles;

    public void Execute()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i] != null)
                particles[i].Play();
        }
    }
}
