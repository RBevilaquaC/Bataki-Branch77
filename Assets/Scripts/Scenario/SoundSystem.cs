using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip buttonClickUI;
    [SerializeField] private AudioClip countdownBeep;
    [SerializeField] private AudioClip IncreaseCombo;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlayIncreaseCombo()
    {
        audioSource.PlayOneShot(IncreaseCombo);
    }

    public void PlayButtonClickUI()
    {
        audioSource.PlayOneShot(buttonClickUI);
    }

    public void PlayCountdownBeep()
    {
        audioSource.PlayOneShot(countdownBeep);
    }
}