using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button continueButton;
    [SerializeField] private int minCharacters;
    [SerializeField] private int maxCharacters;

    private void Start()
    {
        inputField.onValueChanged.AddListener((value) =>
        {
            if (value.Length > maxCharacters)
            {
                inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            }
            
            if (value.Length < minCharacters)
            {
                continueButton.interactable = false;
                return;
            }
            
            continueButton.interactable = true;
        } );
    }
}
