using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayParticleSystem(Vector3 targetTransform)
    {
        particleSystem.transform.position = targetTransform;
        particleSystem.Play();
    }
}
