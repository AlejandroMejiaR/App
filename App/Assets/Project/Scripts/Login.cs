using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class Login : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInputField;
    public Button actionButton;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI titleText;

    [Header("Configuration")]
    [SerializeField] private int maxUsernameLength = 15;
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string defaultUsername = "Jugador";

    private bool isChangingUsername = false;
    private bool isAuthenticated = false;

    private async void Start()
    {
        ConfigureUI();
        await InitializeServices();
        LoadExistingUser();
    }

    private void ConfigureUI()
    {
        warningText.gameObject.SetActive(false);
        nameInputField.characterLimit = maxUsernameLength;
        
        isChangingUsername = PlayerPrefs.HasKey("ChangingUser");
        
        titleText.text = isChangingUsername ? "CAMBIAR NOMBRE" : "CREAR USUARIO";
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = isChangingUsername ? "ACTUALIZAR" : "JUGAR";
        
        if(isChangingUsername)
        {
            PlayerPrefs.DeleteKey("ChangingUser");
            PlayerPrefs.Save();
        }
    }

    private async Task InitializeServices()
    {
        try
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Token: {AuthenticationService.Instance.PlayerId}");
            }
            isAuthenticated = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Error en servicios: {ex.Message}");
            ShowWarning("Modo local activado. Datos no se sincronizarán.");
        }
    }

    private void LoadExistingUser()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            string jsonData = PlayerPrefs.GetString("UserData");
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);
            nameInputField.text = userData.username;
            Debug.Log($"Nombre actual: {userData.username}");
        }
    }

    private void SaveUserData(string username)
    {
        UserData userData = new UserData()
        {
            username = username,
            playerId = isAuthenticated ? AuthenticationService.Instance.PlayerId : "local-user",
            lastLogin = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        string jsonData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData", jsonData);
        PlayerPrefs.Save();

        if(isChangingUsername)
        {
            Debug.Log($"Nombre cambiado a: {username}");
            Debug.Log($"Nuevo token: {userData.playerId}");
        }
    }

    public void OnActionButtonClicked()
    {
        string userName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(userName))
        {
            userName = defaultUsername;
            nameInputField.text = userName;
        }

        if (!ValidateUsername(userName)) return;

        actionButton.interactable = false;
        
        SaveUserData(userName);

        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        try
        {
            if (!string.IsNullOrEmpty(mainMenuScene))
            {
                if (Application.CanStreamedLevelBeLoaded(mainMenuScene))
                {
                    SceneManager.LoadScene(mainMenuScene);
                }
                else
                {
                    ShowWarning("Error: Escena no configurada");
                    actionButton.interactable = true;
                }
            }
            else
            {
                ShowWarning("Error: No se puede cargar el juego");
                actionButton.interactable = true;
            }
        }
        catch (System.Exception ex)
        {
            ShowWarning("Error al iniciar el juego");
            actionButton.interactable = true;
        }
    }

    private bool ValidateUsername(string username)
    {
        if (username.Length > maxUsernameLength)
        {
            ShowWarning($"Máximo {maxUsernameLength} caracteres");
            return false;
        }
        return true;
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        CancelInvoke(nameof(HideWarning));
        Invoke(nameof(HideWarning), 3f);
    }

    private void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class UserData
{
    public string username;
    public string playerId;
    public string lastLogin;
}