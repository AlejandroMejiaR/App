using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button loginButton;
    public TextMeshProUGUI warningText;

    [Header("Configuration")]
    [SerializeField] private int minPasswordLength = 8;
    [SerializeField] private int maxPasswordLength = 30;
    [SerializeField] private int minUsernameLength = 3;
    [TextArea]
    [SerializeField] private string passwordRequirements = "Requisitos:\n- 8-30 caracteres\n- 1 mayúscula\n- 1 minúscula\n- 1 número\n- 1 símbolo especial (!@#$%^&*)";

    private async void Start()
    {
        warningText.gameObject.SetActive(false);
        
        // Configurar el placeholder con los requisitos
        if (passwordInputField.placeholder != null)
        {
            passwordInputField.placeholder.GetComponent<TextMeshProUGUI>().text = passwordRequirements;
        }
        
        // Configurar límite de caracteres
        passwordInputField.characterLimit = maxPasswordLength;
        passwordInputField.contentType = TMP_InputField.ContentType.Standard;

        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }
        }
        catch (System.Exception ex)
        {
            ShowWarning("Error de conexión. Intente nuevamente.");
            Debug.LogError($"Initialization error: {ex}");
            return;
        }

        registerButton.onClick.AddListener(() => _ = RegisterUser());
        loginButton.onClick.AddListener(GoToLogin);
    }

    private void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    private async Task RegisterUser()
    {
        string userName = nameInputField.text.Trim();
        string password = passwordInputField.text.Trim();

        if (!ValidateInputs(userName, password)) return;

        registerButton.interactable = false;
        warningText.gameObject.SetActive(false);

        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userName, password);
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userName, password);
            
            PlayerPrefs.SetString("LastUsername", userName);
            SceneManager.LoadScene("MainMenu");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == 10007) // AccountAlreadyExists
        {
            ShowWarning("Usuario ya existe. Por favor inicia sesión.");
            loginButton.gameObject.SetActive(true);
        }
        catch (AuthenticationException ex)
        {
            HandleRegistrationError(ex);
        }
        finally
        {
            registerButton.interactable = true;
        }
    }

    private bool ValidateInputs(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            ShowWarning("Ingrese un nombre de usuario.");
            return false;
        }

        if (username.Length < minUsernameLength)
        {
            ShowWarning($"El nombre debe tener al menos {minUsernameLength} caracteres.");
            return false;
        }

        if (password.Length < minPasswordLength || password.Length > maxPasswordLength)
        {
            ShowWarning($"La contraseña debe tener entre {minPasswordLength} y {maxPasswordLength} caracteres.");
            return false;
        }

        var passwordError = CheckPasswordRequirements(password);
        if (!string.IsNullOrEmpty(passwordError))
        {
            ShowWarning(passwordError);
            return false;
        }

        return true;
    }

    private string CheckPasswordRequirements(string password)
    {
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return "La contraseña necesita al menos 1 letra mayúscula (A-Z)";
        
        if (!Regex.IsMatch(password, @"[a-z]"))
            return "La contraseña necesita al menos 1 letra minúscula (a-z)";
        
        if (!Regex.IsMatch(password, @"[0-9]"))
            return "La contraseña necesita al menos 1 número (0-9)";
        
        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            return "La contraseña necesita al menos 1 símbolo especial (!@#$%^&*)";

        return null;
    }

    private void HandleRegistrationError(AuthenticationException ex)
    {
        switch (ex.ErrorCode)
        {
            case 10001: // ClientInvalidUserState
                ShowWarning("Ya existe una sesión activa.");
                break;
                
            case 10000: // InvalidParameters
                ShowWarning("Error: " + ex.Message); // Muestra el mensaje del servidor
                break;
                
            case 10015: // NetworkError
                ShowWarning("Error de conexión. Verifique su internet.");
                break;
                
            default:
                ShowWarning($"Error al registrar: {ex.Message}");
                Debug.LogError($"Registration error: {ex}");
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
}