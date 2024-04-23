
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
