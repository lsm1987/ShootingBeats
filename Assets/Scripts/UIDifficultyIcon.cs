using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 난이도 표시
/// </summary>
public class UIDifficultyIcon : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Text _text;

    private readonly Color[] _backgroundColors =
        {
            new Color32(0x21, 0x96, 0xF3, 0xFF)
            , new Color32(0xFF, 0x98, 0x00, 0xFF)
            , new Color32(0xF4, 0x43, 0x36, 0xFF)
        };

    public void SetDifficulty(Difficulty difficulty)
    {
        _background.color = _backgroundColors[(int)difficulty];
        _text.text = difficulty.ToString();
    }
}
