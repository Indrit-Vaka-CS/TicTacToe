using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class Board : MonoBehaviour
{

    [Header("Input Settings : ")]
    [SerializeField] private LayerMask boxesLayerMask;
    [SerializeField] private float touchRadius;

    [Header("Marl Sprites:")]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Header("Mark Colors: ")]
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorO;
    /// <summary>
    /// Line Render used to write a line when ones wons
    /// </summary>
    [SerializeField] private LineRenderer lineRenderer;
    public UnityAction<TeamMark, Color> OnWinAction;

    /// <summary>
    /// Mark is used for the team assignment sing
    /// </summary>
    public TeamMark[] marks;
    private bool canPlay;
    private int marksCount = 0;
    /// <summary>
    /// If we are playing vs CPU, Enabled or disabled on the Unity Editor
    /// </summary>
    public bool boot;

    private Camera cam;
    /// <summary>
    /// The current assignment mark of the team
    /// </summary>
    internal TeamMark currentMark;
    public static Board instance;
    private bool mobile;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cam = Camera.main;
        currentMark = TeamMark.X;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        marks = new TeamMark[9];
        canPlay = true;
        mobile = Application.platform == RuntimePlatform.Android;
    }
    private void Update()
    {
        if (!mobile)
        {
            if (canPlay && Input.GetMouseButtonDown(0))
            {
                Vector2 touchesPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapCircle(touchesPosition, touchRadius, boxesLayerMask);

                if (hit)
                {
                    HitBox(hit.GetComponent<Box>());
                }
            }
        }
        else
        {
            if (canPlay && Input.touchCount !=0)
            {
                Vector2 touchesPosition = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                Collider2D hit = Physics2D.OverlapCircle(touchesPosition, touchRadius, boxesLayerMask);

                if (hit)
                {
                    HitBox(hit.GetComponent<Box>());
                }
            }
        }
    }

    /// <summary>
    /// Responsible for checking the win conditions and applying the game logic
    /// </summary>
    /// <param name="box">The box we hit</param>
    private void HitBox(Box box)
    {
        if (!box.isMarked)
        {
            marksCount++;
            marks[box.index] = currentMark;
            box.SetAsMarked(GetSprite(), currentMark, GetColor());
            bool won = CheckIfWin();

            if (boot && marksCount != 9 && !won)
            {
                BootPlayer.MakeMoveAction.Invoke();
                marksCount++;
            }

            SwitchPlayer();
            //Check if anybody wins:
        }
    }

    private void Menu()
    {
        SceneManager.LoadScene(0);
    }

    internal void RestartGame()
    {
        if (!boot)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Checks if one has won or not
    /// </summary>
    /// <returns>If won true</returns>
    internal bool CheckIfWin()
    {
        //Pattern
        //  0   1   2
        //  3   4   5
        //  6   7   8

        bool won =
        AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
        if (won)
        {

            OnWinAction?.Invoke(currentMark, GetColor());
            print(currentMark.ToString() + " Won.");
            canPlay = false;
        }
        if (marksCount == 9)
        {
            OnWinAction?.Invoke(TeamMark.None, Color.white);
            canPlay = false;
        }
        return won;
    }
    /// <summary>
    /// Gets 3 parameters and check if marks[parameters] are all the same
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    private bool AreBoxesMatched(int i, int j, int k)
    {
        TeamMark m = currentMark;
        bool matched = marks[i] == m && marks[j] == m && marks[k] == m;
        if (matched)
            DrawLine(i, k);
        return matched;
    }
    /// <summary>
    /// Sets the position of the draw line
    /// </summary>
    /// <param name="start">Is the position of the beginning of the line</param>
    /// <param name="end">Is position of the end of the line</param>
    private void DrawLine(int start, int end)
    {
        lineRenderer.SetPosition(0, transform.GetChild(start).position);
        lineRenderer.SetPosition(1, transform.GetChild(end).position);
        Color color = GetColor();
        color.a = .3f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.enabled = true;
    }

    /// <summary>
    /// Switches between players
    /// </summary>
    internal void SwitchPlayer()
    {
        currentMark = (currentMark == TeamMark.X) ? TeamMark.O : TeamMark.X;
    }

    /// <summary>
    /// Gets color based on the CurrentMark (team)
    /// </summary>
    /// <returns>The color</returns>
    internal Color GetColor()
    {
        return (currentMark == TeamMark.X) ? colorX : colorO;
    }
    /// <summary>
    /// Gets and returns the sprite based on the team (mark)
    /// </summary>
    /// <returns></returns>
    internal Sprite GetSprite()
    {
        return (currentMark == TeamMark.X) ? spriteX : spriteO;
    }

    private void OnDestroy()
    {

    }
}
