using TMPro;
using UnityEngine;

public class ShadowTextController : MonoBehaviour
{
    [SerializeField] private TMP_Text original;
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = original.text;
    }
}
