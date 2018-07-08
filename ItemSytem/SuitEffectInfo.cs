using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitEffectInfo {

    public string SuittID;
    public string SuitName;
    public PowerUps powerUps1;
    public PowerUps powerUps2;
    public int currentNum;
    public int suit1Num;
    public int suit2Num;

    public SuitEffectInfo(string id, string name, PowerUps p1, PowerUps p2, int s1, int s2)
    {
        SuittID = id;
        SuitName = name;
        powerUps1 = p1;
        powerUps2 = p2;
        suit1Num = s1;
        suit2Num = s2;
    }

    public void TryEffect(PlayerInfo playerInfo)
    {
        if (currentNum >= suit1Num) powerUps1.TryPowerUp(playerInfo);
        if (currentNum >= suit2Num) powerUps2.TryPowerUp(playerInfo);
    }

    public void TryUnEffect(PlayerInfo playerInfo)
    {
        if (currentNum < suit1Num) powerUps1.TryPowerDown(playerInfo);
        if (currentNum < suit2Num) powerUps2.TryPowerDown(playerInfo);
    }
}
