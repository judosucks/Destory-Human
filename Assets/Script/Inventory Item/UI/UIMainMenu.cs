using UnityEngine;
using UnityEngine.SceneManagement;
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainLevel1";
    
    public void ContinueGame()
    {
        // 继续游戏的逻辑
        Debug.Log("Continue Game");
        // 这里可以添加继续游戏的代码，例如加载上次保存的场景
        SceneManager.LoadScene(sceneName);
    }
}
