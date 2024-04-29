using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] public GameObject[] characters;

    private int _currentCharacter = 0;

    private void SwitchCharacter(int index) {
        if (index <= -1) {
            _currentCharacter = characters.Length-1;
        } else if (index > characters.Length-1) {
            _currentCharacter = 0;
        }
        characters[_currentCharacter].SetActive(true);
    }

    public void SwitchCharacterLeft() {
        characters[_currentCharacter].SetActive(false);
        _currentCharacter -= 1;
        SwitchCharacter(_currentCharacter);
    }

    public void SwitchCharacterRight() {
        characters[_currentCharacter].SetActive(false);
        _currentCharacter += 1;
        SwitchCharacter(_currentCharacter);
    }

}
