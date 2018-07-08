using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioAgent : MonoBehaviour {

    public AudioSource voiceAudioSource;
    public AudioSource effecAudioSource;
    public AudioSource weaponAudioSource;
    public AudioClip attack;
    public AudioClip damage;
    public AudioClip death;
    public AudioClip weapon;
    public AudioClip hit;

    void Start()
    {
        if (GameSettingManager.Instance)
        {
            GameSettingManager.Instance.voiceList.Add(voiceAudioSource);
            GameSettingManager.Instance.fxList.Add(effecAudioSource);
            GameSettingManager.Instance.fxList.Add(weaponAudioSource);
        }
    }

    public void AttackVoice()
    {
        if (!attack || !voiceAudioSource) return;
        voiceAudioSource.clip = attack;
        voiceAudioSource.Play();
    }

    public void DamageVoice()
    {
        if (!damage || !voiceAudioSource) return;
        voiceAudioSource.clip = damage;
        voiceAudioSource.Play();
    }

    public void DeathVoice()
    {
        if (!death || !voiceAudioSource) return;
        voiceAudioSource.clip = death;
        voiceAudioSource.Play();
    }

    public void HitSound()
    {
        if (!hit || !effecAudioSource) return;
        effecAudioSource.clip = hit;
        effecAudioSource.Play();
    }

    public void WeaponSound()
    {
        if (!weapon || !weaponAudioSource) return;
        weaponAudioSource.clip = weapon;
        weaponAudioSource.Play();
    }
}
