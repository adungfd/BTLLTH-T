using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;


public class FormController : MonoBehaviour
{
    public GameObject loginform, signupform, profileform,forgetPassform,notification,MainMenu;
    public TMP_InputField  loginUser, loginPassword, signupUser, signupPassword, signupConfirm;
    public TMP_Text noti_message;
    bool yesno_noti=false;
    public void RunLogin()
    {
        loginform.SetActive(true);
        signupform.SetActive(false); 
        profileform.SetActive(false);    
        forgetPassform.SetActive(false);
        MainMenu.SetActive(false);  
        
    }
    public void Runsignup()
    {
        loginform.SetActive(false);
        signupform.SetActive(true);
        profileform.SetActive(false);
        forgetPassform.SetActive(false);
        
    }
    public void Runprofile()
    {
        loginform.SetActive(false);
        signupform.SetActive(false);
        profileform.SetActive(true);
        forgetPassform.SetActive(false);
        MainMenu.SetActive(false);
       
    }
    public void RunForgetPass()
    {
        loginform.SetActive(false);
        signupform.SetActive(false);        
        profileform.SetActive(false);
        forgetPassform.SetActive (true);
    }
    public void LoginUser()
    {
        //Kiem tra truong ten va mat khau co trong ko
        if (string.IsNullOrEmpty(loginUser.text) || string.IsNullOrEmpty(loginPassword.text)) {
            showNotification("Fields not empty");
            return;
        }
        loginform.SetActive(false);
        MainMenu.SetActive(true);
        //Dang nhap

    }
    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupUser.text) || string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupConfirm.text))
        {
            showNotification("Fields not empty");
            return;
        }
        //Dang ki
    }
    private void showNotification(string message)
    {
        if (yesno_noti)
        {
            SelectButton(buttons[2]);
            yesno_noti = false;
        }
        else {
            SelectButton(buttons[2]);
            
        }
        
        notification.SetActive(true);
        noti_message.text = " " + message;
        
    }
    public void Noti_button()
    {
        notification.SetActive(false);   
    }
    public Button[] buttons;

    public void SelectButton(Button selectedButton)
    {
        if (yesno_noti) {
            foreach (Button button in buttons)
            {
                if (button == selectedButton)
                {
                    button.interactable = false;
                    button.gameObject.SetActive(false);
                }
                else
                {
                    button.interactable = true;
                    button.gameObject.SetActive(true);
                }

            }
        }
        else {
            foreach (Button button in buttons)
            {
                if (button == selectedButton)
                {
                    button.interactable = true;
                    button.gameObject.SetActive(true);
                }
                else
                {
                    button.interactable = false;
                    button.gameObject.SetActive(false);
                }

            }
        }

    }

    public void Exit()
    {
        yesno_noti=true;
        showNotification("Exit game?");
        Debug.Log("exit");
        buttons[1].onClick = new Button.ButtonClickedEvent();
        buttons[1].onClick.AddListener(Application.Quit);
    }
    public void Logout()
    {   
        yesno_noti = true;
        showNotification("Are you sure you want to log out ");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
