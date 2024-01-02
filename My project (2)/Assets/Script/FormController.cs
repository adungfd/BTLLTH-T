using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using UnityEngine.EventSystems;
using System;


public class FormController : MonoBehaviour
{
    public GameObject loginform, signupform, profileform, forgetPassform, notification, MainMenu;
    [SerializeField] public TMP_InputField loginUser, loginPassword, signupEmail, signupUser, signupPassword, signupConfirm,Age_txt,Name_txt;
    public Button submitLogin, submitSignup;
    public TMP_Text noti_message, email_used, user_used,Email_txt,Username_txt,ID_text,changes;  
    //tao list chua thong tin de khi chay truong se doc ko can dung append
    List<PlayerData> playerlist = new List<PlayerData>();
    
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
        

    }
    public void RunForgetPass()
    {
        loginform.SetActive(false);
        signupform.SetActive(false);
        profileform.SetActive(false);
        forgetPassform.SetActive(true);
    }
    public void LoginUser()
    {
        //Kiem tra truong ten va mat khau co trong ko
        if (string.IsNullOrEmpty(loginUser.text) || string.IsNullOrEmpty(loginPassword.text)) {
            showNotification("Fields not empty", false);
            return;
        }
        //Dang nhap
        List<PlayerData> playerexisted = Function.Readinfo<PlayerData>();
        PlayerData playerdata = playerexisted.Find(player => player.Username == loginUser.text && player.Password == loginPassword.text);
        if (playerdata != null)
        {
            Debug.Log("Login successfully");
            loginPassword.text = "";
            //Load data cua ng choi khi dang nhap thanh cong
            Name_txt.text = playerdata.Name;
            Age_txt.text = playerdata.Age.ToString();
            Email_txt.text = playerdata.Email;
            Username_txt.text = playerdata.Username;
            ID_text.text = playerdata.ID;
            //mo menu
            loginform.SetActive(false);
            MainMenu.SetActive(true);
        }
        else
        {
            showNotification("Username or password not correct",false);
            loginPassword.text = "";
            return;
        }

    }
    public void UpdateInfo()
    {
        PlayerData playerdata = playerlist.Find(player => player.Username == Username_txt.text);
        playerdata.Name = Name_txt.text;
        playerdata.Age = int.Parse(Age_txt.text);
        changes.text="Changes save";
        Function.Saveinfo<PlayerData>(playerlist);
        StartCoroutine(ShowMessage("Changes saved",2));
       
    }
    public void SignUpUser()
    {
        
        //submitSignup.interactable = (signupUser.text.Length >= 6 && signupPassword.text.Length >= 6);
        if (string.IsNullOrEmpty(signupUser.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupConfirm.text)||string.IsNullOrEmpty(signupEmail.text))
        {
            showNotification("Fields not empty",false);
            return;
        }
        List<PlayerData> playerexisted = Function.Readinfo<PlayerData>();
        bool emailisused = playerexisted.Any(existed => existed.Email == signupEmail.text);
        bool userisused = playerexisted.Any(existed => existed.Username == signupUser.text);      
        if (userisused||emailisused)
        {   

            if (userisused)
            {
                user_used.text = "Username have already been used";
            }
            if (emailisused)
            {
                email_used.text = "Email have already been used";
            }
            if (!emailisused)
            {
                email_used.text = "";
            }
            if (!userisused)
            {
                user_used.text = "";
            }
            signupPassword.text = "";
            signupConfirm.text = "";
            Debug.Log("existed");
            return;
        }
        playerlist.Add(new PlayerData(signupUser.text, signupPassword.text, signupEmail.text, "PL" + Guid.NewGuid().ToString().Substring(0, 4)));
        email_used.text = "";
        user_used.text = "";
        signupPassword.text = "";
        signupConfirm.text = "";
        Function.Saveinfo<PlayerData>(playerlist);
        showNotification("Account created successfully",false);
        RunLogin();
        //Dang ki
    }
    private void showNotification(string message,bool yesno)
    {
        if (yesno)
        {
            SelectButton(buttons[2],true);
            
        }
        else {
            SelectButton(buttons[2], false);
            
        }
        
        notification.SetActive(true);
        noti_message.text = " " + message;
        
    }
    IEnumerator ShowMessage(string message,float delay)
    {
        changes.text=message;
        yield return new WaitForSeconds(delay);
        changes.text = "";
    }
    public void Noti_button()
    {
        notification.SetActive(false);   
    }
    public Button[] buttons;

    public void SelectButton(Button selectedButton,bool yesno)
    {
        if (yesno) {
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
        showNotification("Exit game?",true);
        Debug.Log("exit");
        buttons[1].onClick = new Button.ButtonClickedEvent();
        buttons[1].onClick.AddListener(Application.Quit);
    }
    public void Logout()
    {         
        showNotification("Are you sure you want to log out ",true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    private void Start()
    {
        playerlist = Function.Readinfo<PlayerData>();
        RunLogin();
    }
}
