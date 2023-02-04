using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{    

        public void Change (string _nextScene)
        {
            SceneManager.LoadScene(_nextScene);
        }

 }
