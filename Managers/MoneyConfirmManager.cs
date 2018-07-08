using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyConfirmManager : MonoBehaviour {
    public static MoneyConfirmManager Instance;
    public GameObject UI;
    [Space]
    public Button YesButton;
    public Button NoButton;
    public Button AddButton;
    public InputField Number;
    public Button SubButton;
    [HideInInspector]
    public int MaxNumber;
    int number;
    public int ItemNumber
    {
        get { return number; }
        set
        {
            if (value > MaxNumber) number = MaxNumber;
            else if (value < 0) number = 0;
            else number = value;
        }
    }

    private void Awake()
    {
        Number.text = 1.ToString();
        Instance = this;
        CloseUI();
    }

    // Use this for initialization
    void Start()
    {
        AddButton.onClick.AddListener(Add);
        SubButton.onClick.AddListener(Sub);
        Number.onEndEdit.AddListener(ConfirmNumber);
        ItemNumber = 1;
    }

    /*private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }*/

    public void ConfirmNumber(string arg)
    {
        int confirmNumber;
        if (int.TryParse(Number.text, out confirmNumber))
            ItemNumber = confirmNumber;
        else ItemNumber = 0;
        Number.text = ItemNumber.ToString();
    }

    public void Add()
    {
        ItemNumber++;
        Number.text = ItemNumber.ToString();
    }

    public void Sub()
    {
        ItemNumber--;
        Number.text = ItemNumber.ToString();
    }

    public void OpenUI()
    {
        Number.text = 1.ToString();
        ItemNumber = int.Parse(Number.text);
        Number.text = ItemNumber.ToString();
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
        YesButton.onClick.RemoveAllListeners();
    }
}
