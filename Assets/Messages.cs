using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class Messages : MonoBehaviour
{
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] public Button okButton;
    [SerializeField] public Button cancelButton;

    [HideInInspector]
    public static Messages Instance;


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Pops up an message with a given message and close button
    /// </summary>
    /// <param name="message">Message to be shown</param>
    /// <param name="okButtonTex">Button text</param>
    /// <param name="delay">The amount of delay before showing the PopMessage</param>
    public void PopUpMessage(string message, string okButtonTex, float delay = 0)
    {
        PopUpMessage(message, okButtonTex, null, delay);
    }

    /// <summary>
    /// Pops up an message with a given message and with OK and cancel button
    /// </summary>
    /// <param name="message">Message to be shown</param>
    /// <param name="okButtonTex">Button text</param>
    /// <param name="cancelButtonText">Cancel Text button</param>
    /// <param name="delay">The amount of delay before showing the PopMessage</param>
    public void PopUpMessage(string message, string okButtonTex, string cancelButtonText, float delay = 0)
    {

        //first check for the cancel button value
        if (cancelButtonText == null)
            cancelButton.gameObject.SetActive(false);
        else
            cancelButton.transform.GetChild(0).GetComponent<Text>().text = cancelButtonText;

        //Assign text and enable the panels
        okButton.transform.GetChild(0).GetComponent<Text>().text = okButtonTex;
        messageText.text = message;


        
        Invoke("EnablePopUpPanel", delay);


    }
    private void EnablePopUpPanel()
    {
        popUpPanel.SetActive(true);
    }

}
