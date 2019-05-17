using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public Animator animator;

    public GameObject sprite;

    enum Direction { Left, Right };
    private Direction direction;

    enum State { Still, Walking, Throwing };
    private State state;

    void Awake()
    {
        direction = Direction.Left;
        state = State.Still;
    }

    void Update()
    {
        switch(state)
        {
            case State.Still:
                sprite.transform.Find("Sprite").gameObject.SetActive(true);
                sprite.transform.Find("Walk").gameObject.SetActive(false);
                break;

            case State.Walking:

                break;

            case State.Throwing:

                break;
        }
    }

    private void Turn()
    {
        switch(direction)
        {
            case Direction.Left:
                sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.x, 0, sprite.transform.rotation.z));
                break;

            default:
                sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.x, 180, sprite.transform.rotation.z));
                break;
        }
    }

    public void SetDirection(string s)
    {
        switch(s)
        {
            case "Left":
                direction = Direction.Left;
                break;

            default:
                direction = Direction.Right;
                break;
        }
    }
}
