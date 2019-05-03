using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReferences : MonoBehaviour
{
    // Interests
    public Sprite interestColor;
    public Sprite interestRocket;
    public Sprite interestPlant;
    public Sprite interestFootball;
    public Sprite interestPuppy;
    public Sprite interestScience;

    // Persons
    public Sprite Person1;
    public Sprite Person2;
    public Sprite Person3;
    public Sprite Person4;

    public Sprite GetInterest(int number)
    {
        switch(number)
        {
            case 0:
                return interestColor;

            case 1:
                return interestPuppy;

            case 2:
                return interestPlant;

            case 3:
                return interestFootball;

            case 4:
                return interestScience;

            case 5:
                return interestRocket;

            default:
                return interestRocket;
        }
    }

    public int GetMaxInterests()
    {
        return 6;
    }

    public Sprite GetPerson(int number)
    {
        Debug.Log("Sprite: " + number);
        switch(number)
        {
            case 0:
                return Person2;

            case 1:
                return Person3;

            case 2:
                return Person4;

            default:
                return Person1;
        }
    }

    public Sprite GetRandomPerson()
    {
        return GetPerson(Mathf.FloorToInt(Random.Range(0, GetMaxPersons())));
    }

    public int GetMaxPersons()
    {
        return 4;
    }
}
