using UnityEngine;

public class PlayerAudioAgent : MonoBehaviour {
    //public static PlayerAudioManager Self;

    public AudioSource voiceAudioSource;
    public AudioSource leftFootAudioSource;
    public AudioSource rightFootAudioSource;
    public AudioSource actionAudioSource;
    public AudioSource weaponAudioSource;
    [Header("脚步声")]
    public AudioClip[] normalStepClips;
    public AudioClip[] deepWaterStepClips;
    public AudioClip[] lowWaterStepClips;
    [Header("跳跃")]
    public AudioClip[] jumpClips;
    [Header("轻攻击")]
    public AudioClip[] lightAttack;
    [Header("重攻击")]
    public AudioClip[] heavyAttack;
    [Header("武器")]
    public AudioClip[] weapon;
    public AudioClip[] weaponLong;
    [Header("受击")]
    public AudioClip[] injured;
    [Header("力竭")]
    public AudioClip[] death;
    [Header("喘息")]
    public AudioClip[] gasp;
    [Header("气竭")]
    public AudioClip[] exhaustion;
    [Header("水中待机")]
    public AudioClip[] swimIdle;
    [Header("游泳")]
    public AudioClip[] swimForward;
    [Header("落水")]
    public AudioClip[] fallWater;


	// Use this for initialization
	/*void Awake () {

	}*/

    private void Start()
    {
        if (!GameSettingManager.Instance) return;
        GameSettingManager.Instance.fxList.Add(leftFootAudioSource);
        GameSettingManager.Instance.fxList.Add(rightFootAudioSource);
        GameSettingManager.Instance.fxList.Add(actionAudioSource);
        GameSettingManager.Instance.fxList.Add(weaponAudioSource);
        GameSettingManager.Instance.voiceList.Add(voiceAudioSource);
    }

    // Update is called once per frame
    /*void Update () {
		
	}*/

    public void FootStepLSound()
    {
        if (!PlayerLocomotionManager.Instance || !PlayerLocomotionManager.Instance.isInit) return;
        if (normalStepClips.Length > 0 && !PlayerLocomotionManager.Instance.playerController.IsBodyWatered)
        {
            leftFootAudioSource.clip = normalStepClips[Random.Range(0, normalStepClips.Length)];
            leftFootAudioSource.Play();  //Play the Audio
        }
        else if (lowWaterStepClips.Length > 0 && PlayerLocomotionManager.Instance.playerController.waterLayer > 0 && PlayerLocomotionManager.Instance.playerController.waterLevel < PlayerLocomotionManager.Instance.playerController.Body.height / 4)
        {
            leftFootAudioSource.clip = lowWaterStepClips[Random.Range(0, lowWaterStepClips.Length)];
            leftFootAudioSource.Play();
        }
        else if (deepWaterStepClips.Length > 0 && PlayerLocomotionManager.Instance.playerController.waterLevel >= PlayerLocomotionManager.Instance.playerController.Body.height / 4)
        {
            leftFootAudioSource.clip = deepWaterStepClips[Random.Range(0, deepWaterStepClips.Length)];
            leftFootAudioSource.Play();
        }
    }
    public void FootStepRSound()
    {
        if (!PlayerLocomotionManager.Instance || !PlayerLocomotionManager.Instance.isInit) return;
        if (normalStepClips.Length > 0 && !PlayerLocomotionManager.Instance.playerController.IsBodyWatered)
        {
            rightFootAudioSource.clip = normalStepClips[Random.Range(0, normalStepClips.Length)];
            rightFootAudioSource.Play();  //Play the Audio
        }
        else if (lowWaterStepClips.Length > 0 && PlayerLocomotionManager.Instance.playerController.waterLayer > 0 && PlayerLocomotionManager.Instance.playerController.waterLevel < PlayerLocomotionManager.Instance.playerController.Body.height / 4)
        {
            rightFootAudioSource.clip = lowWaterStepClips[Random.Range(0, lowWaterStepClips.Length)];
            rightFootAudioSource.Play();
        }
        else if (deepWaterStepClips.Length > 0 && PlayerLocomotionManager.Instance.playerController.waterLevel >= PlayerLocomotionManager.Instance.playerController.Body.height / 4)
        {
            rightFootAudioSource.clip = deepWaterStepClips[Random.Range(0, deepWaterStepClips.Length)];
            rightFootAudioSource.Play();
        }
    }
    public void JumpSound()
    {
        if (jumpClips.Length > 0)
        {
            actionAudioSource.clip = jumpClips[Random.Range(0, jumpClips.Length)];
            actionAudioSource.Play();
        }
    }

    public void LightAtkVoice()
    {
        if (Random.Range(0, 2) == 1)
        {
            if (lightAttack.Length > 0)
            {
                voiceAudioSource.clip = lightAttack[Random.Range(0, lightAttack.Length)];
                voiceAudioSource.Play();
            }
        }
    }
    public void HeavyAtkVoice()
    {
        if (Random.Range(0, 2) == 1)
        {
            if (heavyAttack.Length > 0)
            {
                voiceAudioSource.clip = heavyAttack[Random.Range(0, heavyAttack.Length)];
                voiceAudioSource.Play();
            }
        }
    }
    public void WeaponWingSound()
    {
        if (weapon.Length > 0)
        {
            weaponAudioSource.clip = weapon[Random.Range(0, weapon.Length)];
            weaponAudioSource.Play();
        }
    }
    public void WeaponWingLongSound()
    {
        if (weaponLong.Length > 0)
        {
            weaponAudioSource.clip = weapon[Random.Range(0, weaponLong.Length)];
            weaponAudioSource.Play();
        }
    }

    public void InjuredVoice()
    {
        if(injured.Length > 0)
        {
            voiceAudioSource.clip = injured[Random.Range(0, injured.Length)];
            voiceAudioSource.Play();
        }
    }
    public void DeathVoice()
    {
        if(death.Length > 0)
        {
            voiceAudioSource.clip = death[Random.Range(0, death.Length)];
            voiceAudioSource.Play();
        }
    }
    public void GaspVoice()
    {
        if(gasp.Length > 0)
        {
            voiceAudioSource.clip = gasp[Random.Range(0, gasp.Length)];
            voiceAudioSource.Play();
        }
    }
    public void ExhaustionVoice()
    {
        if(exhaustion.Length > 0)
        {
            voiceAudioSource.clip = exhaustion[Random.Range(0, exhaustion.Length)];
            voiceAudioSource.Play();
        }
    }

    public void SwimIdleSound()
    {
            if (swimIdle.Length > 0)
            {
                if (Random.Range(0, 2) == 1)
                {
                    leftFootAudioSource.clip = swimIdle[Random.Range(0, swimIdle.Length)];
                    leftFootAudioSource.Play();
                }
                else
                {
                    rightFootAudioSource.clip = swimIdle[Random.Range(0, swimIdle.Length)];
                    rightFootAudioSource.Play();
                }
            }
    }
    public void SwimForwardSound()
    {
            if (swimForward.Length > 0)
            {
                if (Random.Range(0, 2) == 1)
                {
                    leftFootAudioSource.clip = swimForward[Random.Range(0, swimForward.Length)];
                    leftFootAudioSource.Play();
                }
                else
                {
                    rightFootAudioSource.clip = swimForward[Random.Range(0, swimForward.Length)];
                    rightFootAudioSource.Play();
                }
            }
    }
    public void FallWaterSound()
    {
            if (fallWater.Length > 0)
            {
                actionAudioSource.clip = fallWater[Random.Range(0, fallWater.Length)];
                actionAudioSource.Play();
            }
    }

    public void GatherSound(AudioClip sound)
    {
        actionAudioSource.clip = sound;
        actionAudioSource.Play();
    }
}
