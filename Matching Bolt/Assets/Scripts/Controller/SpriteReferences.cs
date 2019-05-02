using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReferences : MonoBehaviour
{
    public Sprite interestColor;
    public Sprite interestRocket;
    public Sprite interestPlant;
    public Sprite interestFootball;
    public Sprite interestPuppy;
    public Sprite interestScience;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetSprite(int number)
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

    public int GetMaxSprites()
    {
        return 6;
    }
}
