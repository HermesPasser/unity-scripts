using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isPaused;

    // User changes
    [Header("Paste you storyboard here")]
    public string[] storyboard;

    // User assets
    [Header("You assets here")]
    public Sprite[] images;
    public AudioClip[] sounds;

    [Header("Name of scene loaded when the game ends.")]
    public string endGameSeneName;

    [Header("If you want add a new choices")]
    public GameObject[] choices;

    // Visible references in the unity
    [Header("References, do not change")]
    public Image background;
    public Image characterLeft;
    public Image characterMiddle;
    public Image characterRight;
    public Text textName;
    public Text textLabel;
    public AudioSource audioMusic;
    public AudioSource audioSFX;
    public GameObject labels;
    
   // private AudioClip currentMusic;
   // private AudioClip currentSFX;
    private BoxWriter boxWriter;
    private GameInput gameInput;

    private bool begin;
    private int indexStorydoard = 0;

    void Start()
    {
        boxWriter = FindObjectOfType<BoxWriter>() as BoxWriter;
        gameInput = FindObjectOfType<GameInput>() as GameInput;
    }

    void Update()
    {
        /*  para cada dialogo dar um numero, e se usar o numero para salvar e carregar no quick save,
            ele armazena em uma var o numero da ultima imagem e do ultimo som

            ele vai lendo o indice e quando a um comando de texto ele atualiza o current texto com o indice, idem com sound e image
        
            criar sistema de if que determine o numero de escolhas e o que acontece a cada uma deles
         */

        if (!isPaused && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
            gameInput.OnClickOffOfButtons();

        // Remove do index se tiver parado de tocar (se playInLoop for false)
        if (audioMusic != null && audioMusic.isPlaying)
        {
            audioMusic = null;
            Novel.currentIndexSoundMusic = -1;
        }
        if (audioSFX != null && audioSFX.isPlaying)
        {
            audioSFX = null;
            Novel.currentIndexSoundSFX = -1;

        }

        // Impede que continue enquanto o texto se escreve, quando aparecem escolhas
        if (!boxWriter.isTextEnd || boxWriter.waitForInput || GameInput.choicesEnabled)
            return;

        if (indexStorydoard < storyboard.Length)
        {
            string[] currentPosition = storyboard[indexStorydoard].Replace("  ", " #$").Split(' ');
            string[] removeLastPosOThreePosArray = currentPosition.Reverse().Skip(1).Reverse().ToArray(); // Is necessary remove last position of index for work some commands

            switch (currentPosition[0])
           {
               case "nameText":
                    Novel.currentIndexNameText = indexStorydoard;
                    textName.text = SetTextOFAArray(currentPosition);
                   break;

               case "labelText":
                    Novel.currentIndexLabelText = indexStorydoard;
                    boxWriter.SetLabelText(SetTextOFAArray(currentPosition));
                   break;

               case "backgroundImage":
                    Novel.currentIndexBg = indexStorydoard;
                    SetImage(currentPosition, background);
                   break;

               case "soundMusic":
                    Novel.currentIndexSoundMusic = indexStorydoard;
                    if (currentPosition.Length < 2)
                        break;
                    if (currentPosition[2] == "true")
                        SetSound(removeLastPosOThreePosArray, audioMusic, true);
                    else if (currentPosition[2] == "false")
                        SetSound(removeLastPosOThreePosArray, audioMusic);
                    break;

               case "soundSFX":
                    Novel.currentIndexSoundSFX = indexStorydoard;
                    if (currentPosition.Length < 2)
                        break;
                    if (currentPosition[2] == "true")
                        SetSound(removeLastPosOThreePosArray, audioSFX, true);
                    else if (currentPosition[2] == "false")
                        SetSound(removeLastPosOThreePosArray, audioSFX);
                    break;

               case "characterImage":
                    if (currentPosition[2] == "left")
                    {
                        Novel.currentIndexCharL = indexStorydoard;
                        SetImage(removeLastPosOThreePosArray, characterLeft);
                    }
                    else if (currentPosition[2] == "middle")
                    {
                        Novel.currentIndexCharM = indexStorydoard;
                        SetImage(removeLastPosOThreePosArray, characterMiddle);
                    }
                    else if (currentPosition[2] == "right")
                    {
                        Novel.currentIndexCharR = indexStorydoard;
                        SetImage(removeLastPosOThreePosArray, characterRight);
                    }
                    break;

                case "interface":
                    if (currentPosition[1] == "true")
                        SetActiveLabels(true);
                    else if (currentPosition[1] == "false")
                        SetActiveLabels(false);
                    break;

                case "answerText":
                    setAnswerText(currentPosition);
                    break;

                case "callChoice":
                    int num;
                    if (currentPosition.Length >= 2)
                        if (int.TryParse(currentPosition[1], out num))
                            SetActiveChoices(num, true);
                    break;

                case "ifChoiceThenJump":
                    if (VerifyIndex(currentPosition) && (VerifyChoice(currentPosition) != -1))
                    {
                        indexStorydoard = VerifyChoice(currentPosition);
                        return;
                    }
                    break;
                    
                case "ifChoiceThenLoadScene":
                    if (VerifyChoice(currentPosition) != -1)
                        SceneManager.LoadScene(SetTextOFAArray(currentPosition, 2));
                    break;

                case "endGame":
                    endGame();
                    break;
            }
            Novel.currentIndexStoryboard = indexStorydoard;
            indexStorydoard++;
        }
        else
            endGame();
    }

    public void load()
    {
        //aparentemente funcionou
        boxWriter.ResetText();

        if (Novel.quicksavechoiceSelected != -1)
            Novel.choiceSelected = Novel.quicksavechoiceSelected;

        if (Novel.quicksaveIndexBg != -1)
        {
            string[] c = storyboard[Novel.quicksaveIndexBg].Replace("  ", " #$").Split(' ');
            SetImage(c, background);
            Novel.currentIndexBg = Novel.quicksaveIndexBg;
        }

        if (Novel.quicksaveIndexCharL != -1)
        {
            string[] c = storyboard[Novel.quicksaveIndexCharL].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            SetImage(r, characterLeft);
            Novel.currentIndexCharL = Novel.quicksaveIndexCharL;
        }

        if (Novel.quicksaveIndexCharM != -1)
        {
            string[] c = storyboard[Novel.quicksaveIndexCharM].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            SetImage(r, characterMiddle);
            Novel.currentIndexCharM = Novel.quicksaveIndexCharM;
        }

        if (Novel.quicksaveIndexCharR != -1)
        {
            string[] c = storyboard[Novel.quicksaveIndexCharR].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            SetImage(r, characterMiddle);
            Novel.currentIndexCharR = Novel.quicksaveIndexCharR;
        }

        if (Novel.quicksaveIndexCharR != -1)
        {
            string[] c = storyboard[Novel.quicksaveIndexCharR].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            SetImage(r, characterMiddle);
            Novel.currentIndexCharR = Novel.quicksaveIndexCharR;
        }

        if (Novel.quicksaveSoundMusic != -1)
        {
            string[] c = storyboard[Novel.quicksaveSoundMusic].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            if (c[2] == "true")
                SetSound(r, audioMusic, true);
            else if (c[2] == "false")
                SetSound(r, audioMusic);
            Novel.currentIndexSoundMusic = Novel.quicksaveSoundMusic;
        }

        if (Novel.quicksaveSoundSFX != -1)
        {
            string[] c = storyboard[Novel.quicksaveSoundSFX].Replace("  ", " #$").Split(' ');
            string[] r = c.Reverse().Skip(1).Reverse().ToArray();
            if (c[2] == "true")
                SetSound(r, audioSFX, true);
            else if (c[2] == "false")
                SetSound(r, audioSFX);
            Novel.currentIndexSoundSFX = Novel.quicksaveSoundSFX;
        }

        if (Novel.quicksaveLabelText != -1)
        {
            string[] c = storyboard[Novel.quicksaveLabelText].Replace("  ", " #$").Split(' ');
            boxWriter.SetLabelText(SetTextOFAArray(c));
            Novel.currentIndexLabelText = Novel.quicksaveLabelText;
        }

        if (Novel.quicksaveIndexStoryboard != -1)
        {
            Novel.currentIndexStoryboard = Novel.quicksaveIndexStoryboard;
            indexStorydoard = Novel.quicksaveIndexStoryboard;
        }
    }

    public void SetActiveChoices(int numberOfChoices, bool isActive)
    {
        if (numberOfChoices > choices.Length)
            numberOfChoices = choices.Length;

        for (int i = 0; i < numberOfChoices; i++)
        {
            choices[i].SetActive(isActive);
            GameInput.choicesEnabled = true;
        }

    }

    private bool VerifyIndex(string[] text)
    {
        int index = -1;
        if (int.TryParse(text[2], out index))
             if (index <= storyboard.Length)
                return true;
        return false;
    }

    private int VerifyChoice(string[] text)
    {
        if (text.Length < 3)
            return -1;

        int choice = -1;

        if (int.TryParse(text[1], out choice))
            if (Novel.choiceSelected == choice)
                return choice;

        return -1;
    }

    private void SetImage(string[] text, Image image)
    {
        int imageIndex;

        if (text[1] == "nullValue" || text[1] == "")
            imageIndex = -1;
        else 
            int.TryParse(text[1], out imageIndex);

        if (imageIndex == -1)
        {
            image.sprite = null;
            image.color = new Color(255, 255, 255, 0);
            return;
        }

        if (imageIndex < images.Length)
        {
            image.sprite = images[imageIndex];
            image.color = new Color(255, 255, 255, 255);
        }
    }

    private void SetSound(string[] text, AudioSource output, bool playInLoop = false)
    {
        int soundIndex;

        if (text[1] == "nullValue" || text[1] == "")
            soundIndex = -1;
        else
            int.TryParse(text[1], out soundIndex);

        if (soundIndex == -1)
        {
            output.Stop();
            output.clip = null;
            return;
        }

        if (soundIndex < sounds.Length)
        {
            output.clip = sounds[soundIndex];
            output.loop = playInLoop;
            output.Play();
        }
    }

    private void endGame()
    {
        if (endGameSeneName != "")
           SceneManager.LoadScene(endGameSeneName);
    }

    private void SetActiveLabels(bool isActive)
    {
        labels.SetActive(isActive);
    }


    private void setAnswerText(string[] text)
    {
        int num;
        int.TryParse(text[1], out num);
        num -= 1;
        if (num > 0 || num < choices.Length)
        {
            Text t = choices[num].transform.GetChild(0).GetComponent<Text>();
            t.text = SetTextOFAArray(text.Skip(1).ToArray());
        }

    }

    private string SetTextOFAArray(string[] text, int initialIndex = 1)
    {
        string completeText = "";

        if (initialIndex < 0 || initialIndex >= storyboard.Length)
            initialIndex = 1;

        for (int i = initialIndex; i < text.Length; i++)
            completeText += " " + text[i];

        return completeText.Replace("#$", " ").Trim();
    }
}