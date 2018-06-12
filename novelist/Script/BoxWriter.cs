using UnityEngine;

public class BoxWriter : MonoBehaviour {

    private GameController gameController;

    [Header("Define text velocity here.")]
    public float velocity = 0.1f;
    private float time;
    private string completeText;
    private int currentIndexInText;

    [Header("Do not change.")]
    public bool isTextEnd = true;
    public bool waitForInput;

	void Start () {
        gameController = FindObjectOfType<GameController>() as GameController;
	}
	
	void Update () {
        if (gameController.isPaused || isTextEnd)
            return;

        time += Time.deltaTime;

        if (time <= velocity)
            return;

        if (currentIndexInText < completeText.Length)
        {
            gameController.textLabel.text += completeText[currentIndexInText].ToString();
            time = 0;
            currentIndexInText++;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.End))
                JumpText();
        }
        else
        {
            isTextEnd = true;
            waitForInput = true;
        }
    }

    public void SetLabelText(string text)
    {
        if (!gameController.labels.activeSelf)
            return;
        gameController.textLabel.text = "";
        ResetText();
        completeText = text;
        velocity = 0.1f;
        isTextEnd = false;
    }

    public void JumpText()
    {
        gameController.textLabel.text = completeText;
        isTextEnd = true;
    }

    public void ResetText()
    {
        currentIndexInText = 0;
        completeText = "";
    }
}
