using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    public void LoadMainGame()
    {
        Scene.Load("GameScene");
    }
}
