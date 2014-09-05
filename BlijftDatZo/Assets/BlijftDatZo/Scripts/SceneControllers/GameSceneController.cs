using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class GameSceneController : MonoBehaviour 
{
    private static GameSceneController instance;
    public static GameSceneController GetProgram
    {
        get
        {
            if (instance == null)
            {
                GameObject programObject = GameObject.FindGameObjectWithTag(Constants.TagGameSceneController);
                instance = programObject.GetComponent<GameSceneController>();
            }
            return instance;
        }
    }

    private void Awake()
    {

    }
    private void Start()
    {

    }
}
