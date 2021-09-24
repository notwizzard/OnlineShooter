using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField] private string prefsName;
    [SerializeField] private string defualtValue;

    private TMP_InputField input;

    private void Start()
    {
        input = gameObject.GetComponent<TMP_InputField>();
        if (PlayerPrefs.GetString(prefsName, defualtValue) != defualtValue)
        {
            input.text = PlayerPrefs.GetString(prefsName, defualtValue);
        }
    }

    public void OnInputChange()
    {
        PlayerPrefs.SetString(prefsName, input.text);
    }

}

