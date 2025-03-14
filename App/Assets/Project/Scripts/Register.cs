using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UserRegister : MonoBehaviour
{
    public TMP_InputField nameInputField;   // Campo de nombre de usuario
    public TMP_InputField passwordInputField; // Campo de clave numérica
    public Button saveButton;
    public TextMeshProUGUI warningText; // Mensaje de advertencia

    private void Start()
    {
        warningText.gameObject.SetActive(false);

        saveButton.onClick.AddListener(SaveUserData);
    }

    public void SaveUserData()
    {
        string userName = nameInputField.text.Trim();
        string password = passwordInputField.text.Trim();

        // Validar nombre de usuario
        if (string.IsNullOrEmpty(userName) || userName == nameInputField.placeholder.GetComponent<TextMeshProUGUI>().text)
        {
            ShowWarning("Ingresa un nombre válido.");
            return;
        }

        // Validar clave (debe ser numérica y de 4 dígitos)
        if (password.Length != 4 || !IsNumeric(password))
        {
            ShowWarning("La clave debe ser de 4 dígitos numéricos.");
            return;
        }

        // Guardar usuario y clave en PlayerPrefs
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.SetString("UserPassword", password);
        PlayerPrefs.Save();

        Debug.Log($"Usuario registrado: {userName} con clave: {password}");

        // Cargar la siguiente pantalla (por ejemplo, el menú principal)
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
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

