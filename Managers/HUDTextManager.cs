using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTextManager : MonoBehaviour {

    public static HUDTextManager Instance;

    public GameObject HPBarPrafab;
    public GameObject HUDTextPrefab;
    public List<HPBarAgent> HPBars;
    public EnemyInfoAgent boss;
    public GameObject BossHPBar;
    public Text bossName;
    public Image bossHP;
    float goAwayBossTime;

    private bl_HUDText HUDRoot;

    // Use this for initialization
    void Awake () {
        Instance = this;
        HUDRoot = bl_UHTUtils.GetHUDText;
        HPBars = new List<HPBarAgent>();
        MyTools.SetActive(BossHPBar, false);
    }

    private void Update()
    {
        if(boss)
        {
            bossHP.fillAmount = boss.Current_HP / boss.HP;
            if (boss.IsAlive)
            {
                if (Vector3.Distance(PlayerLocomotionManager.Instance.playerController.transform.position, boss.transform.position) > 15)
                {
                    MyTools.SetActive(BossHPBar, false);
                    goAwayBossTime += Time.deltaTime;
                    if (goAwayBossTime > 10)
                    {
                        HideBossHPBar();
                    }
                }
                else
                {
                    goAwayBossTime = 0;
                    MyTools.SetActive(BossHPBar, true);
                }
            }
            else HideBossHPBar();
        }
    }

    public void NewHPBar(EnemyInfoAgent enemyInfo)
    {
        if (!enemyInfo) return;
        if (HPBars.Count < 0)
        {
            GameObject HPBar = Instantiate(HPBarPrafab, transform) as GameObject;
            HPBar.GetComponent<HPBarAgent>().SetEnermy(enemyInfo);
            HPBars.Add(HPBar.GetComponent<HPBarAgent>());
        }
        else
        {
            if (HPBars.Find(h => h.enemyInfoAgent == enemyInfo)) return;
            else
            {
                HPBarAgent hbAgent = HPBars.Find(h => !h.enemyInfoAgent);
                if (hbAgent)
                    hbAgent.SetEnermy(enemyInfo);
                else
                {
                    GameObject bar = Instantiate(HPBarPrafab, transform) as GameObject;
                    bar.GetComponent<HPBarAgent>().SetEnermy(enemyInfo);
                    HPBars.Add(bar.GetComponent<HPBarAgent>());
                }
            }
        }
    }

    public void SetBossHPBar(EnemyInfoAgent bossInfo)
    {
        if (!bossInfo)
        {
            return;
        }
        boss = bossInfo;
        bossName.text = bossInfo.Name;
        MyTools.SetActive(BossHPBar, true);
    }

    public void HideBossHPBar()
    {
        boss = null;
        goAwayBossTime = 0;
        MyTools.SetActive(BossHPBar, false);
    }

    public void NewStatu(Transform targetTran, Statu statu)
    {
        string text = "";
        switch(statu)
        {
            case Statu.Giddy:text = "眩晕";break;
            case Statu.Rigidity:text = "硬直";break;
            case Statu.Falled:text = "倒地";break;
        }
        HUDTextInfo info = new HUDTextInfo(targetTran, text);
        info.Color = Color.white;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = 1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.Side = bl_Guidance.Up;
        info.VerticalPositionOffset = 0;
        info.AnimationSpeed = 0.5f;
        info.ExtraDelayTime = 2;
        info.FadeSpeed = 400;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewBlock(Transform targetTran)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "格挡");
        info.Color = Color.cyan;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = 1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.Side = bl_Guidance.Up;
        info.VerticalPositionOffset = 0;
        info.AnimationSpeed = 0.5f;
        info.ExtraDelayTime = 2;
        info.FadeSpeed = 400;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewBlockBroken(Transform targetTran)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "破甲");
        info.Color = Color.white;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = 1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.Side = bl_Guidance.Up;
        info.VerticalPositionOffset = 0;
        info.AnimationSpeed = 0.5f;
        info.ExtraDelayTime = 2;
        info.FadeSpeed = 400;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewMiss(Transform targetTran)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "闪避");
        info.Color = Color.white;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = 1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.Side = bl_Guidance.Up;
        info.VerticalPositionOffset = 0;
        info.AnimationSpeed = 0.5f;
        info.ExtraDelayTime = 2;
        info.FadeSpeed = 400;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewDamageValue(Transform targetTran, int value)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "- " + value);
        info.Color = Color.red;
        info.Size = Random.Range(30, 40);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = -1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.VerticalPositionOffset = 0;
        info.Side = (Random.Range(0, 2) == 1) ? bl_Guidance.RightDown : bl_Guidance.LeftDown;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewCritDamageValue(Transform targetTran, int value)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "会心- " + value);
        info.Color = Color.yellow;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = -1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.VerticalPositionOffset = 0;
        info.Side = (Random.Range(0, 2) == 1) ? bl_Guidance.RightDown : bl_Guidance.LeftDown;
        //Send the information
        HUDRoot.NewText(info);
    }

    public void NewBackDamageValue(Transform targetTran, int value)
    {
        HUDTextInfo info = new HUDTextInfo(targetTran, "背击- " + value);
        info.Color = Color.magenta;
        info.Size = Random.Range(40, 50);
        info.Speed = Random.Range(10, 20);
        info.VerticalAceleration = -1;
        info.VerticalFactorScale = Random.Range(1.2f, 3);
        info.VerticalPositionOffset = 0;
        info.Side = (Random.Range(0, 2) == 1) ? bl_Guidance.RightDown : bl_Guidance.LeftDown;
        //Send the information
        HUDRoot.NewText(info);
    }
}
