using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Resolution
    public List<ResolutionClass> resClass = new List<ResolutionClass>();
    List<Dropdown.OptionData> dataList = new List<Dropdown.OptionData>();
    public Dropdown dropdown;
    Dropdown.OptionData optionData;

    #endregion

    private void OnEnable()
    {
        DropDownResolution();
        dropdown.onValueChanged.AddListener(ChangeRes);
    }

    void DropDownResolution()
    {
        for (int i = 0; i < resClass.Count; i++)
        {
            optionData = new Dropdown.OptionData();
            optionData.text = $"{resClass[i].horizontal} x {resClass[i].vertical}";

            dataList.Add(optionData);

            if (i == resClass.Count - 1)
            {
                optionData = new Dropdown.OptionData();
                optionData.text = "Full Screen";
                dataList.Add(optionData);
            }
        }

        dropdown.AddOptions(dataList);

    }

    void ChangeRes(int dropMenuCount)
    {
        if(dataList[dropMenuCount].text == "Full Screen")
        {
            Debug.Log("Full screen");
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
        else
        {
            int horizontal = resClass[dropMenuCount].horizontal;
            int vertical = resClass[dropMenuCount].vertical;

            Screen.SetResolution(horizontal, vertical, false);

        }

    }



}
[System.Serializable]
public class ResolutionClass
{
    public int horizontal, vertical;
}