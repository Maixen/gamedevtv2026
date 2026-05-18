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
            KeineÄnderung, Normal, Happy, Angry,
        }

        private enum MCFaces
        {
            KeineÄnderung, Normal, Happy, Angry, Sad, Suprised
        }
        [SerializeField]
        private string boxContent;
        [SerializeField]
        private BossFaces bossFace;
        [SerializeField]
        private MCFaces mCFace;
        [SerializeField]
        private bool isBossTalking;

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

    [SerializeField]
    private GameObject dialogBox;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private UnityEngine.UI.Image boss;
    [SerializeField] 
    private UnityEngine.UI.Image mC;

    [SerializeField]
    [Range(10f, 100f)]
    private float charsPerSecond;
    [SerializeField]
    private bool isTyping;
    [SerializeField] 
    private bool skip;

    [SerializeField] 
    private Dialogue[] dialogues;

    private void Start()
    {
        StartDialogue(0);
    }

    public void StartDialogue( int dialogueNum)
    {
        if (dialogueNum >= dialogues.Length)
            return;
        //Spiel pausieren
        isTyping = false;
        skip = false;
        print("Uiiai");
        StartCoroutine(DialogRoutine(dialogueNum));
    }

    public void SkipText()
    {
        skip = true;
    }

    IEnumerator DialogRoutine(int index)
    {
        int currentBox = 0;
        int boxCount = dialogues[index].GetBoxCount();
        print(boxCount);
        while (currentBox < boxCount) 
        {
            print(currentBox);
            text.text = "";
            isTyping = true;
            skip = false;
            DialogueScreen currentScreen = dialogues[index].GetBox(currentBox);
            currentBox++;
            for(int i = 0; i < currentScreen.GetMessage().Length; i++)
            {
                if (skip)
                    break;
                string displayedText = currentScreen.GetMessage().Insert(i, alpha);
                text.text = displayedText;
                print(displayedText);
                yield return new WaitForSeconds(1f / charsPerSecond);
            }
            print("writing done");
            text.text = currentScreen.GetMessage();
            skip = false;
            while (!skip)
            {
                yield return null;
            }
        }
        text.text = "";
        print("Dialog fertig");
    }
}
