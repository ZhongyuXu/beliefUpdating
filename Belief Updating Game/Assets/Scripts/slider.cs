using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _sliderText;
    [SerializeField] private Transform _cursor, _fill;
    void Start()
    {
        // Initially hide the cursor and slider text
        InitialCursorVisible(false);

        // Add listener to show cursor and slider text when the slider value changes
        _slider.onValueChanged.AddListener((v) => {
            _sliderText.text = v.ToString() + "%";
            InitialCursorVisible(true);
        });
    }
    void InitialCursorVisible(bool TF)
    {
        _cursor.gameObject.SetActive(TF);
        _fill.gameObject.SetActive(TF);
        _sliderText.gameObject.SetActive(TF);
    }
}
