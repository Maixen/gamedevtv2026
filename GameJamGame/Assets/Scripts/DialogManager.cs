using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogManager : MonoBehaviour
{
    private string alpha = "<color=#00000000>";

    [Serializable]
    [SerializeField]
    private class DialogueScreen
    {
        private enum BossFaces
        {
            Normal, Happy, Angry,
        }
        private enum MCFaces
        {
            Normal, Happy, Angry, Sad, Suprised
        }
        [SerializeField] private string boxContent;
        [SerializeField] private BossFaces bossFace;
        [SerializeField] private MCFaces mCFace;
        [SerializeField] private bool isBossTalking;
        [SerializeField] private bool isChoice;
        [SerializeField] private string[] choiceText;
        [SerializeField] private int[] choiceBox;

        public string GetMessage()
        {
            return boxContent;
        }

        public int GetBossFace()
        {
            return (int)bossFace;
        }

        public int GetMCFace()
        {
            return (int)mCFace;
        }

        public bool HasChoice()
        {
            return isChoice;
        }

        public string GetChoiceText(int choice)
        {
            return choiceText[choice];
        }

        public int GetChoiceScreen(int choice)
        {
            return choiceBox[choice];
        }

        public bool IsBossTalking()
        {
            return isBossTalking;
        }
    }

    [Serializable]
    [SerializeField]
    private class Dialogue
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private DialogueScreen[] boxes; 

        public DialogueScreen GetBox(int boxIndex)
        {
            return boxes[boxIndex];
        }

        public int GetBoxCount()
        { 
            return boxes.Length; 
        }
    }

    public static DialogManager instance;

    [SerializeField]
    private GameObject dialogBox;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private UnityEngine.UI.Image boss;
    [SerializeField] 
    private UnityEngine.UI.Image mC;
    [SerializeField] private GameObject[] choiceButtons;

    [Space]

    [SerializeField] private Sprite[] bossSprites;
    [SerializeField] private Sprite[] bossTalkingSprites;
    [SerializeField] private Sprite[] mCSprites;
    [SerializeField] private Sprite[] mCTalkingSprites;

    [Range(10f, 100f)]
    [SerializeField] private float charsPerSecond;
    [Range(1, 50)]
    [SerializeField] private int charsPerSpriteChange;
    [SerializeField] private bool isTyping;
    [SerializeField] private bool skip;
    [SerializeField] private int choice;
    [SerializeField] private Dialogue[] dialogues;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartDialogue(0);
    }

    public void StartDialogue( int dialogueNum)
    {
        dialogBox.SetActive(true);
        if (dialogueNum >= dialogues.Length)
            return;
        //Spiel pausieren
        RefreshBox();
        print("AHA");
        GameManager.instance.GamePauseRequest();
        print("AHA");
        StartCoroutine(DialogRoutine(dialogueNum));
    }

    public void SkipText()
    {
        skip = true;
    }

    public void Choose(int option)
    {
        choice = option;
    }

    private void RefreshBox()
    {
        text.text = "";
        isTyping = true;
        skip = false;
        choice = -1;
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
            choiceButtons[i].GetComponentInChildren<Text>().text = "";
        }
    }

    IEnumerator DialogRoutine(int index)
    {
        int currentBox = 0;
        int boxCount = dialogues[index].GetBoxCount();
        print(boxCount);
        while (currentBox < boxCount) 
        {
            //besorgen der Daten der Box
            DialogueScreen currentScreen = dialogues[index].GetBox(currentBox);
            currentBox++;
            int bossSpriteIndex = currentScreen.GetBossFace();
            int mCSpriteIndex = currentScreen.GetMCFace();
            mC.sprite = mCSprites[mCSpriteIndex];
            boss.sprite = bossSprites[bossSpriteIndex];
            //Während reden
            int charsTalked = 0;
            bool spriteChanged = false;
            for (int i = 0; i < currentScreen.GetMessage().Length; i++)
            {
                if (skip)
                    break;
                string displayedText = currentScreen.GetMessage().Insert(i, alpha);
                text.text = displayedText;
                //print(displayedText);
                if(charsTalked % charsPerSpriteChange == 0)
                {
                    if (spriteChanged)
                    {
                        if(currentScreen.IsBossTalking())
                        {
                            boss.sprite = bossTalkingSprites[bossSpriteIndex];
                        }
                        else
                        {
                            mC.sprite = mCTalkingSprites[mCSpriteIndex];
                        }
                    }
                    else 
                    {
                        if (currentScreen.IsBossTalking())
                        {
                            boss.sprite = bossSprites[bossSpriteIndex];
                        }
                        else
                        {
                            mC.sprite = mCSprites[mCSpriteIndex];
                        }
                    }
                    spriteChanged = !spriteChanged;
                }
                yield return new WaitForSeconds(1f / charsPerSecond);
                charsTalked++;
            }
            //Nach reden
            mC.sprite = mCSprites[mCSpriteIndex];
            boss.sprite = bossSprites[bossSpriteIndex];
            text.text = currentScreen.GetMessage();
            skip = false;
            //Warten auf Signal für nächste Box
            if (currentScreen.HasChoice())
            {
                for (int i = 0;i < choiceButtons.Length;i++)
                {
                    choiceButtons[i].SetActive(true);
                    choiceButtons[i].GetComponentInChildren<Text>().text = currentScreen.GetChoiceText(i);
                    yield return new WaitForSeconds(1f / charsPerSecond);
                }
                while (choice < 0)
                {
                    yield return null;
                }
                currentBox = currentScreen.GetChoiceScreen(choice);
            }
            else
            {
                while (!skip)
                {
                    yield return null;
                }
            }
            RefreshBox();
        }
        dialogBox.SetActive(false);
        GameManager.instance.GameUnpauseRequest(true);
    }
}
