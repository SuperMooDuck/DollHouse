using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
