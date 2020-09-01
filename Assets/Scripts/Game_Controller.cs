using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    [Header("Debug Variables")]
    [SerializeField] private bool _isDebug;

    [Header("Components")]
    [SerializeField] private GameObject _ball;

    void Start()
    {
        
    }
    void Update()
    {
        CheckForBallReset();
    }

    private void CheckForBallReset()
    {
        if(Input.GetKeyDown(KeyCode.Q) && _isDebug)
        {
            _ball.GetComponent<Ball_Controller>().ResetDebugPosition();
        }
    }
}
