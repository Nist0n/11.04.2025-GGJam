using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    private void Start()
    {
        var _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collision>();
        Debug.Log(_player);
    }

    public void OnCollisionEnter(Collision _player)
    {
        Debug.Log("Wark");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
