using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [Header("UI References :")]
    [SerializeField] private GameObject uiCanvans;
    [SerializeField] private TextMeshProUGUI uiWinnerText;
    [SerializeField] private Button uiRestartButton;

    [Header("Board References :")]
    [SerializeField] private Board board;


    private void Start()
    {
        uiRestartButton.onClick.AddListener(Board.instance.RestartGame);
        board.OnWinAction += OnWinEvent;
        uiCanvans.SetActive(false);
    }

    private void OnWinEvent(TeamMark mark, Color color)
    {
        if (!Board.instance.boot)
            uiWinnerText.text = (mark == TeamMark.None) ? "Draw! \nNobody Wins!" : mark.ToString() + " Wins.";
        else
        {
            if (mark == TeamMark.None)
                uiWinnerText.text = "Draw! \nNobody Wins!";
            else if (mark == TeamMark.O)
                uiWinnerText.text = "You Lose!";
            else
                uiWinnerText.text = "You Won!";
        }

        uiWinnerText.color = color;

        uiCanvans.SetActive(true);
    }

    private void OnDestroy()
    {
        uiRestartButton.onClick.RemoveAllListeners();
        board.OnWinAction -= OnWinEvent;
    }
}
