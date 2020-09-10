using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    [Header("Debug Variables")]
    [SerializeField] public bool _isDebug;

    [Header("GameObjects")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _camera;

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
            _camera.GetComponent<Camera_Controller>().PositionCameraBehindBall();
        }
    }
}
