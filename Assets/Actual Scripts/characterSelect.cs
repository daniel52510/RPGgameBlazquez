using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class characterSelect : MonoBehaviour
{
    public int selectedCharacterIndex;
    [Header("List Of Characters")]
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterSplash;

    private void UpdateCharacterSelectionUI()
    {
        //Splash, Name
        characterSplash.sprite = characterList[selectedCharacterIndex].splash;
        characterName.text = characterList[selectedCharacterIndex].characterName;
    }

    [System.Serializable]
    public class CharacterSelectObject
    {
        public Sprite splash;
        public string characterName;
    }

    public void leftButt()
    {
        selectedCharacterIndex--;
        if(selectedCharacterIndex < 0)
            selectedCharacterIndex = characterList.Count - 1;


        UpdateCharacterSelectionUI();
    }
    public void rightButt()
    {
        selectedCharacterIndex++;
        if (selectedCharacterIndex == characterList.Count)
            selectedCharacterIndex = 0;


        UpdateCharacterSelectionUI();
    }
    private void Start()
    {
        UpdateCharacterSelectionUI();
        Debug.Log("characterList Element: " + characterList[selectedCharacterIndex].characterName);
    }
}
