using UnityEngine;
using System.Collections;
using Assets.BlijftDatZo.Scripts;

public class GameSceneController : MonoBehaviour 
{
    private static GameSceneController instance;
    public static GameSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameSceneObject = GameObject.FindGameObjectWithTag(Constants.TagGameSceneController);
                instance = gameSceneObject.GetComponent<GameSceneController>();
            }
            return instance;
        }
    }

    public GameController GameController { get; private set; }

    private void Awake()
    {
        this.GameController = this.gameObject.AddComponent<GameController>();
    }
    private void Start()
    {

    }
}
