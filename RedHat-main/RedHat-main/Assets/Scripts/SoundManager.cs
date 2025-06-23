using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Player Sounds")]
    public AudioClip wolfAttackSound;
    public AudioClip hedgehogAttackSound;
    public AudioClip footstepsSound;
    public AudioClip throwSound;
    public AudioClip buttonClickSound;
    public AudioClip heartbeatSound;
    public AudioClip hitSound;
    [Header("Music")]
    public AudioClip backgroundMusic;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource footstepsSource; // ��������� �������� ��� �����

    // ����� ��� �������� playing
    private bool isFootstepsPlaying = false;
    private bool isHeartbeatPlaying = false;

    private void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        footstepsSource = gameObject.AddComponent<AudioSource>(); // ��� �����

        // ��������� ����������
        musicSource.loop = true;
        musicSource.clip = backgroundMusic;

        footstepsSource.loop = true; // ���� ���������
        footstepsSource.clip = footstepsSound;
    }

    // ������� ����� (��� ��������)
    public void PlayWolfAttack() => sfxSource.PlayOneShot(wolfAttackSound);
    public void PlayHedgehogAttack() => sfxSource.PlayOneShot(hedgehogAttackSound);
    public void PlayThrow() => sfxSource.PlayOneShot(throwSound);
    public void PlayHit() => sfxSource.PlayOneShot(hitSound);
    public void PlayButtonClick() => sfxSource.PlayOneShot(buttonClickSound);

    // ���� � ���������
    public void PlayFootsteps()
    {
        if (!isFootstepsPlaying)
        {
            footstepsSource.Play();
            isFootstepsPlaying = true;
        }
    }

    public void StopFootsteps()
    {
        footstepsSource.Stop();
        isFootstepsPlaying = false;
    }

    // ������������ � ���������
    public void PlayHeartbeat()
    {
        if (!isHeartbeatPlaying)
        {
            sfxSource.PlayOneShot(heartbeatSound);
            isHeartbeatPlaying = true;
            Invoke(nameof(ResetHeartbeat), heartbeatSound.length);
        }
    }

    private void ResetHeartbeat() => isHeartbeatPlaying = false;

    // ������
    public void PlayMusic() => musicSource.Play();
    public void StopMusic() => musicSource.Stop();
}