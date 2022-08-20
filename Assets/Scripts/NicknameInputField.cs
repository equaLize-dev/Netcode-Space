using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button[] continueButtons;
    [SerializeField] private int minCharacters;
    [SerializeField] private int maxCharacters;

    private void Start()
    {
        inputField.onValueChanged.AddListener(HandleInput);
        HandleInput(inputField.text);
    }

    private void HandleInput(string input)
    {
        if (input.Length > maxCharacters)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
        }
            
        if (input.Length < minCharacters)
        {
            foreach (var button in continueButtons)
            {
                button.interactable = false;
            }
                
            return;
        }
            
        foreach (var button in continueButtons)
        {
            button.interactable = true;
        }
    }
}