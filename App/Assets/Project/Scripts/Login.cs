using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    private const string CLOUD_SAVE_USER_DATA_KEY = "player_profile"; 

    private bool isChangingUsername = false;
    private bool isAuthenticated = false; // Indicates if Unity Authentication is successful

    private async void Start()
    {
        ConfigureUI();
        await InitializeServices();
        await LoadExistingUser(); 
    }

    private void ConfigureUI()
    {
        warningText.gameObject.SetActive(false);
        nameInputField.characterLimit = maxUsernameLength;
        
        isChangingUsername = PlayerPrefs.HasKey("ChangingUser");
        
        titleText.text = isChangingUsername ? "CAMBIAR NOMBRE" : "CREAR USUARIO";
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = isChangingUsername ? "ACTUALIZAR" : "INGRESAR";
        
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
                Debug.Log("Unity Services initialized.");
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in anonymously. Player ID: {AuthenticationService.Instance.PlayerId}");
            }
            isAuthenticated = true; // Set to true only if authentication is successful
        }
        catch (System.Exception ex)
        {
            isAuthenticated = false; // Authentication failed
            Debug.LogWarning($"Error en servicios de Unity: {ex.Message}. Operando en modo local.");
            // Esta advertencia es aceptable porque informa al usuario que no habrá sincronización.
            ShowWarning("Modo local activado. Datos no se sincronizarán."); 
        }
    }

    private async Task LoadExistingUser()
    {
        UserData userData = null;

        // --- 1. Intentar cargar desde Cloud Save si está autenticado ---
        if (isAuthenticated)
        {
            try
            {
                var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_SAVE_USER_DATA_KEY });
                
                if (savedData != null && savedData.ContainsKey(CLOUD_SAVE_USER_DATA_KEY))
                {
                    string jsonData = savedData[CLOUD_SAVE_USER_DATA_KEY].Value.ToString(); 
                    userData = JsonUtility.FromJson<UserData>(jsonData);
                    Debug.Log($"Nombre actual (Cloud Save): {userData.username}");
                    // Mantener PlayerPrefs sincronizado con los datos de la nube
                    PlayerPrefs.SetString("UserData", jsonData);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("No se encontraron datos de usuario en Cloud Save. Intentando cargar de PlayerPrefs...");
                }
            }
            catch (System.Exception ex)
            {
                // Un error al cargar de Cloud Save. NO mostrar un ShowWarning al usuario.
                // Simplemente loguear la advertencia para el desarrollador y continuar con PlayerPrefs.
                Debug.LogWarning($"Error al cargar datos de Cloud Save: {ex.Message}. Recurriendo a PlayerPrefs.");
            }
        }
        else
        {
            Debug.Log("Servicios no autenticados, no se intentará cargar de Cloud Save.");
        }

        // --- 2. Fallback: Intentar cargar desde PlayerPrefs si Cloud Save falló o no se intentó/encontró datos ---
        if (userData == null && PlayerPrefs.HasKey("UserData"))
        {
            string jsonData = PlayerPrefs.GetString("UserData");
            userData = JsonUtility.FromJson<UserData>(jsonData);
            Debug.Log($"Nombre actual (PlayerPrefs): {userData.username}");
        }

        // --- 3. Aplicar los datos cargados a la UI ---
        if (userData != null)
        {
            nameInputField.text = userData.username;
        }
        // Si userData es null aquí, significa que ni Cloud Save ni PlayerPrefs tenían datos.
        // Esto es normal para un usuario completamente nuevo, el defaultUsername lo manejará.
        // NO se necesita una advertencia al usuario.
    }

    private async Task SaveUserData(string username)
    {
        UserData userData = new UserData()
        {
            username = username,
            playerId = isAuthenticated ? AuthenticationService.Instance.PlayerId : "local-user",
            lastLogin = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        string jsonData = JsonUtility.ToJson(userData);

        // --- 1. Siempre guardar en PlayerPrefs como caché local/fallback ---
        PlayerPrefs.SetString("UserData", jsonData);
        PlayerPrefs.Save();
        Debug.Log($"Datos guardados localmente: {username}");

        // --- 2. Guardar en Cloud Save si está autenticado ---
        if (isAuthenticated)
        {
            try
            {
                var dataToSave = new Dictionary<string, object>
                {
                    { CLOUD_SAVE_USER_DATA_KEY, jsonData }
                };
                await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
                Debug.Log($"Datos de usuario guardados en Cloud Save: {username}");
            }
            catch (System.Exception ex)
            {
                // Un error al guardar en Cloud Save. NO mostrar un ShowWarning al usuario.
                // La operación local ya tuvo éxito. Loguear para el desarrollador.
                Debug.LogWarning($"Error al guardar datos en Cloud Save: {ex.Message}. Datos guardados solo localmente.");
            }
        }
        else
        {
            Debug.LogWarning("No autenticado. No se puede guardar en Cloud Save.");
            // NO mostrar ShowWarning al usuario. La operación local ya tuvo éxito.
        }

        if(isChangingUsername)
        {
            Debug.Log($"Nombre cambiado a: {username}");
            Debug.Log($"Nuevo token: {userData.playerId}");
        }
    }

    public async void OnActionButtonClicked()
    {
        string userName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(userName))
        {
            userName = defaultUsername;
            nameInputField.text = userName;
        }

        if (!ValidateUsername(userName)) return;

        actionButton.interactable = false;
        
        await SaveUserData(userName); 
        
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
                    ShowWarning($"Error: Escena '{mainMenuScene}' no configurada o no encontrada.");
                    actionButton.interactable = true;
                }
            }
            else
            {
                ShowWarning("Error: No se puede cargar el juego. Nombre de escena no especificado.");
                actionButton.interactable = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al iniciar el juego: {ex.Message}");
            ShowWarning("Error al iniciar el juego");
            actionButton.interactable = true;
        }
    }

    private bool ValidateUsername(string username)
    {
        if (username.Length > maxUsernameLength)
        {
            ShowWarning($"Máximo {maxUsernameLength} caracteres para el nombre de usuario.");
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