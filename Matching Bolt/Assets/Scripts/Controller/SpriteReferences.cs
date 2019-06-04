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
    public Sprite interestMusic;
    public Sprite interestGames;
    public Sprite interestBaseball;
    public Sprite interestBowling;

    // Hearts
    public Sprite HeartFull;
    public Sprite HeartFractured;
    public Sprite HeartHalf;

    // Persons
    public RuntimeAnimatorController Person1;
    public RuntimeAnimatorController Person2;
    public RuntimeAnimatorController Person3;
    public RuntimeAnimatorController Person4;
    public RuntimeAnimatorController Person5;
    public RuntimeAnimatorController Person6;
    public RuntimeAnimatorController Person7;
    public RuntimeAnimatorController Person8;
    public RuntimeAnimatorController Person9;
    public RuntimeAnimatorController Person10;

    // Despairs
    public RuntimeAnimatorController Despair1;
    public RuntimeAnimatorController Despair2;
    public RuntimeAnimatorController Despair3;
    public RuntimeAnimatorController Despair4;

    // Despair projectiles
    public Sprite despairProjectile;
    public Sprite despairProjectileWarning;

    // Popup tutorial messages
    public Sprite popupLover;
    public Sprite popupInterest;
    public Sprite popupDespair;

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

            case 6:
                return interestMusic;

            case 7:
                return interestGames;

            case 8:
                return interestBaseball;

            case 9:
                return interestBowling;

            default:
                return interestRocket;
        }
    }

    public int GetMaxInterests()
    {
        return 10;
    }

    public RuntimeAnimatorController GetPerson(int number)
    {
        switch(number)
        {
            case 0:
                return Person2;

            case 1:
                return Person3;

            case 2:
                return Person4;

            case 3:
                return Person5;

            case 4:
                return Person6;

            case 5:
                return Person7;

            case 6:
                return Person8;

            case 7:
                return Person9;

            case 8:
                return Person10;

            default:
                return Person1;
        }
    }

    public RuntimeAnimatorController GetRandomPerson()
    {
        return GetPerson(Mathf.FloorToInt(Random.Range(0, GetMaxPersons() + 0.999f)));
    }

    public int GetMaxPersons()
    {
        return 9;
    }

    public RuntimeAnimatorController GetDespair(int number)
    {
        switch (number)
        {
            case 0:
                return Despair2;

            case 1:
                return Despair3;

            case 2:
                return Despair4;

            default:
                return Despair1;
        }
    }

    public RuntimeAnimatorController GetRandomDespair(out int number)
    {
        number = Mathf.FloorToInt(Random.Range(0, GetMaxDespairs()));
        return GetDespair(number);
    }

    public int GetMaxDespairs()
    {
        return 4;
    }

    public Sprite GetDespairProjectile(string type)
    {
        switch (type)
        {
            case "Normal":
                return despairProjectile;

            case "Warning":
                return despairProjectileWarning;

            default:
                return despairProjectile;
        }
    }

    public Sprite GetHeart(int i)
    {
        switch (i)
        {
            case 0:
                return HeartHalf;

            case 1:
                return HeartFractured;

            default:
                return HeartFull;
        }
    }

    public Sprite GetPopup(string message)
    {
        switch (message)
        {
            case "Lover":
                return popupLover;

            case "Interest":
                return popupInterest;

            case "Despair":
                return popupDespair;

            default:
                return popupLover;
        }
    }
}
