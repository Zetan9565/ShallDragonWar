using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataAgent : MonoBehaviour
{

    [Range(1, 3)]
    public int dataIndex;
    Button save;
    Button load;
    Text date;
    Text level;
    Text character;

    // Use this for initialization
    void Start()
    {
        save = transform.Find("Save").GetComponent<Button>();
        save.onClick.AddListener(OnSaveButtonClick);
        load = transform.Find("Load").GetComponent<Button>();
        load.onClick.AddListener(OnLoadButtonClick);
        date = transform.Find("Date").GetComponent<Text>();
        level = transform.Find("Level/level").GetComponent<Text>();
        character = transform.Find("Character/character").GetComponent<Text>();
        ShowData();
    }

    // Update is called once per frame
    void Update()
    {
        ShowData();
    }

    public void ShowData()
    {
        if (!GameDataManager.Instance.isSaving)
        {
            MyTools.SetActive(save.gameObject, false);
            MyTools.SetActive(load.gameObject, true);
        }
        else
        {
            MyTools.SetActive(save.gameObject, true);
            MyTools.SetActive(load.gameObject, false);
        }
        string[] infos;
        if (GameDataManager.Instance.GetDataInfo(dataIndex, out infos))
        {
            string date = infos[0].Insert(4, "年").Insert(7, "月").Insert(10,"日 ").Insert(14,":").Insert(17,":");
            this.date.text = date;
            level.text = infos[1] + "级";
            character.text = infos[2];
            load.interactable = true;
            //Debug.Log(date);
        }
        else
        {
            //Debug.Log("无数据");
            date.text = "无数据";
            level.text = "";
            character.text = "";
            load.interactable = false;
        }
    }

    public void OnLoadButtonClick()
    {
        GameDataManager.Instance.Load(dataIndex);
    }

    public void OnSaveButtonClick()
    {
        GameDataManager.Instance.Save(dataIndex);
    }
}
