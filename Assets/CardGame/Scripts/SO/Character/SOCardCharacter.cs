using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardCharacter", menuName = "CardCharacters/character")]
public class SOCardCharacter : ScriptableObject
{
    public CardCharacterStats stats;
    public GameObject character;
}
