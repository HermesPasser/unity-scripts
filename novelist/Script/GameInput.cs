using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour{

    public static bool choicesEnabled;
    public GameObject[] slot;

    public enum state
    {
        save, load
    }

    private GameController gameController;
    private BoxWriter boxWriter;
    private state status;

    public GameObject label;
    public Image sceneImage;
    public Text nameLabel;
    
    void Start()
    {
        boxWriter = FindObjectOfType<BoxWriter>() as BoxWriter;
        gameController = FindObjectOfType<GameController>() as GameController;
    }

    private void saveGame(int slotNumber)
    {
        Text dateText = slot[slotNumber - 1].transform.GetChild(1).GetComponent<Text>() as Text;
        DateTime localDate = DateTime.Now;
        dateText.text = localDate.ToString("dd/MM/yyyy h:mm");

        PlayerPrefs.SetString(slotNumber + "date", localDate.ToString("dd/MM/yyyy h:mm"));

        PlayerPrefs.SetInt(slotNumber + "choiceSelected", Novel.choiceSelected);
        PlayerPrefs.SetInt(slotNumber + "currentIndexStoryboard", Novel.currentIndexStoryboard);
        PlayerPrefs.SetInt(slotNumber + "currentIndexBg", Novel.currentIndexBg);
        PlayerPrefs.SetInt(slotNumber + "currentIndexCharL", Novel.currentIndexCharL);
        PlayerPrefs.SetInt(slotNumber + "currentIndexCharM", Novel.currentIndexCharM);
        PlayerPrefs.SetInt(slotNumber + "currentIndexCharR", Novel.currentIndexCharR);
        PlayerPrefs.SetInt(slotNumber + "currentIndexNameText", Novel.currentIndexNameText);
        PlayerPrefs.SetInt(slotNumber + "currentIndexLabelText", Novel.currentIndexLabelText);
        PlayerPrefs.SetInt(slotNumber + "currentIndexSoundMusic", Novel.currentIndexSoundMusic);
        PlayerPrefs.SetInt(slotNumber + "currentIndexSoundSFX", Novel.currentIndexSoundSFX);

        OnClickSlot(slotNumber);
        SetDateOfSlots();
    }

    private void loadGame(int slotNumber)
    {
        Novel.quicksavechoiceSelected = PlayerPrefs.GetInt(slotNumber + "choiceSelected", -1);
        Novel.quicksaveIndexBg = PlayerPrefs.GetInt(slotNumber + "currentIndexBg", -1);
        Novel.quicksaveIndexCharL = PlayerPrefs.GetInt(slotNumber + "currentIndexCharL", -1);
        Novel.quicksaveIndexCharM = PlayerPrefs.GetInt(slotNumber + "currentIndexCharM", -1);
        Novel.quicksaveIndexCharR = PlayerPrefs.GetInt(slotNumber + "currentIndexCharR", -1);
        Novel.quicksaveNameText = PlayerPrefs.GetInt(slotNumber + "currentIndexNameText", -1);
        Novel.quicksaveLabelText = PlayerPrefs.GetInt(slotNumber + "currentIndexLabelText", -1);
        Novel.quicksaveSoundMusic = PlayerPrefs.GetInt(slotNumber + "currentIndexSoundMusic", -1);
        Novel.quicksaveSoundSFX = PlayerPrefs.GetInt(slotNumber + "currentIndexSoundSFX", -1);
        Novel.quicksaveIndexStoryboard = PlayerPrefs.GetInt(slotNumber + "currentIndexStoryboard", -1);

        gameController.load();
        print("Passou");

        // Reset quick save
        Novel.quicksavechoiceSelected = Novel.quicksaveIndexStoryboard = -1;
        Novel.quicksaveIndexBg = Novel.quicksaveIndexCharL = -1;
        Novel.quicksaveIndexCharM = Novel.quicksaveIndexCharR = -1;
        Novel.quicksaveNameText = Novel.quicksaveLabelText = -1;
        Novel.quicksaveSoundMusic = Novel.quicksaveSoundSFX = -1;
    }

    // Game Choices

    public void OnClickChoice(int choiceNumber)
    {
        Novel.choiceSelected = choiceNumber;
        gameController.SetActiveChoices(gameController.choices.Length, false);
        choicesEnabled = false;
    }

    // Game Label

    public void OnClickQuickSave()
    {
        if (gameController.isPaused)
            return;

        Novel.quicksavechoiceSelected = Novel.choiceSelected;

        Novel.quicksaveIndexStoryboard = Novel.currentIndexStoryboard;
        Novel.quicksaveIndexBg = Novel.currentIndexBg;
        Novel.quicksaveIndexCharL = Novel.currentIndexCharL;
        Novel.quicksaveIndexCharM = Novel.currentIndexCharM;
        Novel.quicksaveIndexCharR = Novel.currentIndexCharR;
        Novel.quicksaveNameText = Novel.currentIndexNameText;
        Novel.quicksaveLabelText = Novel.currentIndexLabelText;
        Novel.quicksaveSoundMusic = Novel.currentIndexSoundMusic;
        Novel.quicksaveSoundSFX = Novel.currentIndexSoundSFX;

        boxWriter.isTextEnd = true;
        //GameInput.choicesEnabled
    }

    public void OnClickQuickLoad()
    {
        if (!gameController.isPaused)
            gameController.load();
    }


    public void OnClickOffOfButtons()
    {
        if (!boxWriter.isTextEnd)
            boxWriter.JumpText();
        else
            boxWriter.waitForInput = false;
    }

    // Save-load Label

    public void CallSaveLoad()
    {
        SetDateOfSlots();
        sceneImage.sprite = null;
        gameController.isPaused = true;
        label.SetActive(true);

        if (status == state.save)
            nameLabel.text = "Save";
        else
            nameLabel.text = "Load";
            
        for (int i = 0; i < slot.Length; i++)
        {
            Button b = slot[i].transform.GetChild(2).GetComponent<Button>() as Button;
            Text t = b.transform.GetChild(0).GetComponent<Text>() as Text;
            if (status == state.save)
                t.text = "Save/Override";
            else
                t.text = "Load";
        }
    }

    public void OnClickSave()
    {
        status = state.save;
        CallSaveLoad();
    }

    public void OnClickLoad()
    {
        status = state.load;
        CallSaveLoad();
    }


    public void OnClickSlot(int slotNumber)
    {
        if (PlayerPrefs.HasKey(slotNumber + "currentIndexBg"))
            sceneImage.sprite = gameController.images[PlayerPrefs.GetInt(slotNumber + "currentIndexBg")];
        else
            sceneImage.sprite = null;

        print(PlayerPrefs.GetInt(slotNumber + "currentIndexBg"));
    }

    public void OnClickSlotChildButton(int slotNumber)
    {
        if (status == state.save)
            saveGame(slotNumber);
        else
            loadGame(slotNumber);
    }

    public void OnClickButtonX()
    {
        gameController.isPaused = false;
        label.SetActive(false);
    }

    private void SetDateOfSlots()
    {
        for (int i = 0; i < slot.Length; i++)
        {
            Text dateText = slot[i].transform.GetChild(1).GetComponent<Text>() as Text;
            DateTime localDate = DateTime.Now;
            if (PlayerPrefs.HasKey(i + 1 + "date"))
                dateText.text = PlayerPrefs.GetString(i + 1 + "date");
        }
    }
}

