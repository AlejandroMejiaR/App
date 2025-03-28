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
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    public TextMeshProUGUI warningText;
    //public GameObject loadingIndicator;

    [Header("Configuration")]
    [SerializeField] private int passwordLength = 4;
    [SerializeField] private float timeBetweenAttempts = 1f;

    private float lastAttemptTime;
    private int failedAttempts = 0;
    private const int MaxAttempts = 5;

    private void Start()
    {
        warningText.gameObject.SetActive(false);
        //loadingIndicator.SetActive(false);
        
        if (PlayerPrefs.HasKey("LastUsername"))
        {
            nameInputField.text = PlayerPrefs.GetString("LastUsername");
            passwordInputField.Select();
        }
        else
        {
            nameInputField.Select();
        }

        loginButton.onClick.AddListener(() => _ = LoginUser());
        registerButton.onClick.AddListener(GoToRegister);
        
        // Cambiamos a un método separado para la inicialización asíncrona
        _ = InitializeAndCheckSession();
    }

    // Nuevo método para manejar la inicialización asíncrona
    private async Task InitializeAndCheckSession()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (AuthenticationService.Instance.IsSignedIn)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Session check error: {ex}");
        }
    }

    private void GoToRegister()
    {
        SceneManager.LoadScene("Register");
    }

    private async Task LoginUser()
    {
        if (Time.time - lastAttemptTime < timeBetweenAttempts) return;
        lastAttemptTime = Time.time;

        string userName = nameInputField.text.Trim();
        string password = passwordInputField.text.Trim();

        if (!ValidateInputs(userName, password)) return;

        //loadingIndicator.SetActive(true);
        loginButton.interactable = false;
        warningText.gameObject.SetActive(false);

        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userName, password);
            
            PlayerPrefs.SetString("LastUsername", userName);
            SceneManager.LoadScene("MainMenu");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == 10006) // AccountNotFound
        {
            ShowWarning("Usuario no registrado. Por favor regístrese.");
            registerButton.gameObject.SetActive(true);
        }
        catch (AuthenticationException ex)
        {
            failedAttempts++;
            HandleLoginError(ex);
            
            if (failedAttempts >= MaxAttempts)
            {
                loginButton.interactable = false;
                ShowWarning($"Demasiados intentos. Espere {timeBetweenAttempts} segundos.");
                await Task.Delay((int)(timeBetweenAttempts * 1000));
                loginButton.interactable = true;
                failedAttempts = 0;
            }
        }
        finally
        {
            //loadingIndicator.SetActive(false);
            loginButton.interactable = true;
        }
    }

    // Resto de los métodos permanecen igual...
    private bool ValidateInputs(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            ShowWarning("Ingrese un nombre de usuario.");
            return false;
        }

        if (password.Length != passwordLength || !IsNumeric(password))
        {
            ShowWarning($"La contraseña debe tener {passwordLength} dígitos numéricos.");
            return false;
        }

        return true;
    }

    private void HandleLoginError(AuthenticationException ex)
    {
        switch (ex.ErrorCode)
        {
            case 10000: // InvalidParameters
                ShowWarning("Credenciales inválidas.");
                break;
                
            case 10009: // InvalidSessionToken
                ShowWarning("Sesión expirada.");
                break;
                
            case 10010: // CredentialMismatch
                ShowWarning("Usuario o contraseña incorrectos.");
                break;
                
            case 10015: // NetworkError
                ShowWarning("Error de conexión. Verifique su internet.");
                break;
                
            default:
                ShowWarning($"Error al iniciar sesión: {ex.Message}");
                Debug.LogError($"Login error: {ex}");
                break;
        }
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        CancelInvoke(nameof(HideWarning));
        Invoke(nameof(HideWarning), 5f);
    }

    private void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }

    private bool IsNumeric(string value)
    {
        foreach (char c in value)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }
}