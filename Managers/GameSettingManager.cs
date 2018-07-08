using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingManager : MonoBehaviour
{
    public static GameSettingManager Instance;
    public GameObject UI;

    private void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public Button resumeButton;
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            MyTools.SetActive(resumeButton.gameObject, true);
        }
        else
        {
            Time.timeScale = 1;
            MyTools.SetActive(resumeButton.gameObject, false);
        }
    }

    // Use this for initialization
    void Start()
    {
        m_camera = Camera.main;
        ChangeCameraClippingPlane();
        ChangeRotateSpeed();
        ChangeRotateType();
        SetQualityLevel();
        resumeButton.onClick.AddListener(delegate { PauseGame(false); });
        MyTools.SetActive(resumeButton.gameObject, false);;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Confirm.Self.NewConfirm("确认退出游戏？", delegate () { Application.Quit(); });
        }
        cameraFollow.smooth = smoothChanger.isOn;
    }
    [Header("声音")]
    public Slider bgmChanger;
    public Slider fxChanger;
    public Slider voiceChanger;
    public List<AudioSource> bgmList = new List<AudioSource>();
    public List<AudioSource> fxList = new List<AudioSource>();
    public List<AudioSource> voiceList = new List<AudioSource>();

    public void ChangeBgmVolume()
    {
        foreach (AudioSource a in bgmList)
            a.volume = bgmChanger.value;
    }

    public void ChangeFxVolume()
    {
        foreach (AudioSource a in fxList)
            if(a) a.volume = fxChanger.value;
    }

    public void ChangeVoiceVolume()
    {
        foreach (AudioSource a in voiceList)
            a.volume = voiceChanger.value;
    }

    [Space]
    [Header("性能")]
    public Slider cameraClipChanger;
    public Camera m_camera;
    public uSkyPro uSkyPro;

    public void ChangeCameraClippingPlane()
    {
        m_camera.farClipPlane = cameraClipChanger.value * 100;
        RenderSettings.fogStartDistance = cameraClipChanger.value * 10;
        RenderSettings.fogEndDistance = cameraClipChanger.value * 100;
        uSkyPro.GroundOffset = 10000 - cameraClipChanger.value * 1000;
        uSkyPro.AltitudeScale = 11 - cameraClipChanger.value;
    }
    [Space]
    public Dropdown shadowResChanger;
    public Slider shadowDisChanger;

    public void ChangeShadowDistance()
    {
            QualitySettings.shadowDistance = shadowDisChanger.value * 15;
    }

    public void DisableShadows()
    {
        
            QualitySettings.shadows = ShadowQuality.Disable;
            shadowDisChanger.interactable = false;
        
    }

    public void EnableShadows()
    {
        
            QualitySettings.shadows = ShadowQuality.HardOnly;
            shadowDisChanger.interactable = true;
        
    }

    public void ChangeShadowResLevel(int level)
    {

            switch (level)
            {
                case 0: DisableShadows(); break;
                case 1:
                    EnableShadows();
                    QualitySettings.shadowResolution = ShadowResolution.Low; break;
                case 2:
                    EnableShadows();
                    QualitySettings.shadowResolution = ShadowResolution.Medium; break;
                case 3:
                    EnableShadows();
                    QualitySettings.shadowResolution = ShadowResolution.High; break;
            }
    }
    [Space]
    public Toggle cloudChanger;
    public usky.uSkyClouds2D uSkyClouds2D;

    [Space]
    public Toggle realtimeRefChanger;

    public void ChangeReflection()
    {

            QualitySettings.realtimeReflectionProbes = realtimeRefChanger.isOn;
    }

    [Space]
    public Dropdown waterLevelChanger;
    [SerializeField]
    AQUAS_Reflection[] waterRefs;

    public void ChangeWaterQuality(int level, AQUAS_Reflection water)
    {
        switch (level)
        {
            case 0:water.m_ReflectLayers = 0;break;
            case 1:water.m_ReflectLayers = ~(1 << LayerMask.NameToLayer("UI"));
                water.m_TextureSize = 4; break;
            case 2:
                water.m_ReflectLayers = ~(1 << LayerMask.NameToLayer("UI"));
                water.m_TextureSize = 16; break;
            case 3:
                water.m_ReflectLayers = ~(1 << LayerMask.NameToLayer("UI"));
                water.m_TextureSize = 64; break;
        }
    }
    public void ChangeWatersLevel(int level)
    {

            foreach (AQUAS_Reflection water in waterRefs)
                ChangeWaterQuality(level, water);
    }

    public Dropdown waterTypeChanger;
    public GameObject highWaterGroup;
    public GameObject simpleWaterGroup;
    public void ChangeWaterType(int level)
    {

        switch (level)
        {
                case 0: MyTools.SetActive(simpleWaterGroup, true); MyTools.SetActive(highWaterGroup, false); waterRefs = simpleWaterGroup.GetComponentsInChildren<AQUAS_Reflection>(); break;
                case 1: MyTools.SetActive(simpleWaterGroup, false); MyTools.SetActive(highWaterGroup, true); waterRefs = highWaterGroup.GetComponentsInChildren<AQUAS_Reflection>(); break;
        }
    }

    [Space]
    public Dropdown particleCountChanger;
    public Toggle softParticle;

    public void ChangeSoftParticle()
    {

            QualitySettings.softParticles = softParticle.isOn;
    }

    public void ParticleCountLevel(int level)
    {

            switch(level)
            {
                case 0: QualitySettings.particleRaycastBudget = 4;break;
                case 1: QualitySettings.particleRaycastBudget = 16;break;
                case 2: QualitySettings.particleRaycastBudget = 64;break;
                case 3: QualitySettings.particleRaycastBudget = 256;break;
            }
    }

    [Space]
    public Dropdown textureQualityChanger;

    [Space]
    public Toggle veryLow;
    public Toggle low;
    public Toggle medium;
    public Toggle high;
    public Toggle customize;

    public void SetQualityLevel()
    {
        if (veryLow.isOn) VeryLow();
        if (low.isOn) Low();
        if (medium.isOn) Medium();
        if (high.isOn) High();
        if (customize.isOn) Customize(textureQualityChanger.value);
        QualitySettings.softVegetation = false;
    }

    void VeryLow()
    {
        QualitySettings.SetQualityLevel(0);
        cloudChanger.isOn = false;
        cloudChanger.interactable = false;
        realtimeRefChanger.isOn = false;
        realtimeRefChanger.interactable = false;
        shadowDisChanger.value = QualitySettings.shadowDistance / 15;
        shadowDisChanger.interactable = false;
        shadowResChanger.value = 0;
        shadowResChanger.interactable = false;
        particleCountChanger.value = 0;
        particleCountChanger.interactable = false;
        softParticle.isOn = QualitySettings.softParticles;
        softParticle.interactable = false;
        textureQualityChanger.value = 0;
        textureQualityChanger.interactable = false;
        cameraClipChanger.value = 2;
        cameraClipChanger.interactable = false;
        waterLevelChanger.value = 0;
        waterLevelChanger.interactable = false;
        waterTypeChanger.value = 0;
        waterTypeChanger.interactable = false;
    }

    void Low()
    {
        QualitySettings.SetQualityLevel(1);
        cloudChanger.isOn = false;
        cloudChanger.interactable = false;
        realtimeRefChanger.isOn = false;
        realtimeRefChanger.interactable = false;
        shadowDisChanger.value = QualitySettings.shadowDistance / 15;
        shadowDisChanger.interactable = false;
        shadowResChanger.value = 1;
        shadowResChanger.interactable = false;
        particleCountChanger.value = 1;
        particleCountChanger.interactable = false;
        softParticle.isOn = QualitySettings.softParticles;
        softParticle.interactable = false;
        textureQualityChanger.onValueChanged.RemoveAllListeners();
        textureQualityChanger.value = 1;
        textureQualityChanger.interactable = false;
        cameraClipChanger.value = 4;
        cameraClipChanger.interactable = false;
        waterLevelChanger.value = 1;
        waterLevelChanger.interactable = false;
        waterTypeChanger.value = 0;
        waterTypeChanger.interactable = false;
    }

    void Medium()
    {
        QualitySettings.SetQualityLevel(2);
        cloudChanger.isOn = true;
        cloudChanger.interactable = false;
        realtimeRefChanger.isOn = QualitySettings.realtimeReflectionProbes;
        realtimeRefChanger.interactable = false;
        shadowDisChanger.value = QualitySettings.shadowDistance / 15;
        shadowDisChanger.interactable = false;
        shadowResChanger.value = 2;
        shadowResChanger.interactable = false;
        particleCountChanger.value = 2;
        particleCountChanger.interactable = false;
        softParticle.isOn = QualitySettings.softParticles;
        softParticle.interactable = false;
        textureQualityChanger.onValueChanged.RemoveAllListeners();
        textureQualityChanger.value = 2;
        textureQualityChanger.interactable = false;
        cameraClipChanger.value = 7;
        cameraClipChanger.interactable = false;
        waterLevelChanger.value = 2;
        waterLevelChanger.interactable = false;
        waterTypeChanger.value = 1;
        waterTypeChanger.interactable = false;
    }

    void High()
    {
        QualitySettings.SetQualityLevel(3);
        cloudChanger.isOn = true;
        cloudChanger.interactable = false;
        realtimeRefChanger.isOn = QualitySettings.realtimeReflectionProbes;
        realtimeRefChanger.interactable = false;
        shadowDisChanger.value = QualitySettings.shadowDistance / 15;
        shadowDisChanger.interactable = false;
        shadowResChanger.value = 3;
        shadowResChanger.interactable = false;
        particleCountChanger.value = 3;
        particleCountChanger.interactable = false;
        softParticle.isOn = QualitySettings.softParticles;
        softParticle.interactable = false;
        textureQualityChanger.onValueChanged.RemoveAllListeners();
        textureQualityChanger.value = 3;
        textureQualityChanger.interactable = false;
        cameraClipChanger.value = 10;
        cameraClipChanger.interactable = false;
        waterLevelChanger.value = 3;
        waterLevelChanger.interactable = false;
        waterTypeChanger.value = 1;
        waterTypeChanger.interactable = false;
    }

    void Customize(int level)
    {
        switch (level)
        {
            case 0: QualitySettings.SetQualityLevel(6);
                break;
            case 1: QualitySettings.SetQualityLevel(7);
                break;
            case 2: QualitySettings.SetQualityLevel(8);
                break;
            case 3: QualitySettings.SetQualityLevel(9);
                break;
        }
        uSkyClouds2D.enabled = cloudChanger.isOn;
        cloudChanger.interactable = true;
        QualitySettings.realtimeReflectionProbes = realtimeRefChanger.isOn;
        realtimeRefChanger.interactable = true;
        ChangeShadowDistance();
        shadowDisChanger.interactable = true;
        ChangeShadowResLevel(shadowResChanger.value);
        shadowResChanger.interactable = true;
        switch (particleCountChanger.value)
        {
            case 0: QualitySettings.particleRaycastBudget = 4; break;
            case 1: QualitySettings.particleRaycastBudget = 16; break;
            case 2: QualitySettings.particleRaycastBudget = 64; break;
            case 3: QualitySettings.particleRaycastBudget = 256; break;
        }
        particleCountChanger.interactable = true;
        QualitySettings.softVegetation = softParticle.isOn;
        softParticle.interactable = true;
        textureQualityChanger.onValueChanged.RemoveAllListeners();
        textureQualityChanger.onValueChanged.AddListener(Customize);
        textureQualityChanger.interactable = true;
        ChangeCameraClippingPlane();
        cameraClipChanger.interactable = true;
        ChangeWaterType(waterTypeChanger.value);
        waterLevelChanger.interactable = true;
        ChangeWatersLevel(waterLevelChanger.value);
        waterTypeChanger.interactable = true;
    }

    [Header("控制")]
    public Slider rotateSpeedChanger;
    public Dropdown rotateTypeChanger;
    public Toggle smoothChanger;
    public CameraFollow cameraFollow;

    public void ChangeRotateType()
    {
        switch(rotateTypeChanger.value)
        {
            case 0:cameraFollow.rotateType = CameraFollow.RotateType.UableRotate;break;
            case 1:cameraFollow.rotateType = CameraFollow.RotateType.HorizontalOnly;break;
            case 2:cameraFollow.rotateType = CameraFollow.RotateType.Both;break;
        }
        cameraFollow.ResetView();
    }

    public void ChangeRotateSpeed()
    {
        cameraFollow.rotateSpeed = rotateSpeedChanger.value;
    }

    public void SaveToFile()
    {
        List<string> settings = new List<string>
        {
            "QualityLevel=" + QualitySettings.GetQualityLevel(),
            "CameraClippingPlanesFar=" + cameraClipChanger.value,
            "ShadowDistance=" + QualitySettings.shadowDistance,
            "ShadowResolution=" + QualitySettings.shadowResolution,
            "Cloud=" + uSkyClouds2D.enabled
        };
        File.WriteAllLines(Application.dataPath + "/QualitySetting.config", settings.ToArray(), System.Text.Encoding.UTF8);
        Debug.Log(Application.dataPath);
    }

    public void OpenUI()
    {
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
    }
}
