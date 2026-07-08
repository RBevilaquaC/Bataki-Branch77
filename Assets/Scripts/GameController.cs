using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    void Awake()
    {
        if(gc == null) gc = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


}
