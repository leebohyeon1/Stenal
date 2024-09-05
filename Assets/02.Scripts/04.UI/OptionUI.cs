using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public GameObject[] UIGroup;
    public Button[] buttons;

    private void OnEnable()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => SetUI(button));
        }
    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUI(Button clickedButton)
    {
        for (int i = 0; i < UIGroup.Length; i++) 
        {
            Button button = buttons[i];

            bool isSelected = button == clickedButton;

            UIGroup[i].SetActive(isSelected);
        }
    }


}
