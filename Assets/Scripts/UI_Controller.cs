using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private GameObject _homeButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _resetButton;
    [SerializeField] private GameObject _debugButton;
    [SerializeField] private GameObject _debugPanel;

    [Header("Debug Panel Ball Position")]
    [SerializeField] private InputField _xPosition;
    [SerializeField] private InputField _yPosition;
    [SerializeField] private InputField _zPosition;
    [SerializeField] private GameObject _resetBallPosition;

    [Header("Debug Panel Forces")]
    [SerializeField] private InputField _minShootForce;
    [SerializeField] private InputField _maxShootForce;
    [SerializeField] private InputField _minCurveForce;
    [SerializeField] private InputField _maxCurveForce;
    [SerializeField] private InputField _angleDeviation;


    [Header("GameObjects")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _camera;

    private Game_Controller _gameController;
    private Ball_Controller _ballController;
    private Camera_Controller _cameraController;
    private bool _showDebugTools;


    void Start()
    {
        GetAllComponents();
        CheckDebugMode();
        GetInitialValues();
    }
    void Update()
    {
        
    }

    private void GetAllComponents()
    {
        _gameController = GetComponent<Game_Controller>();
        _ballController = _ball.GetComponent<Ball_Controller>();
        _cameraController = _camera.GetComponent<Camera_Controller>();
    }

    private void CheckDebugMode()
    {
        if (_gameController._isDebug)
        {
            _debugButton.SetActive(true);
        }
    }

    private void GetInitialValues()
    {
        if(_ball != null)
        {
            //Get all the data set on the inspector and write it down in all the fields
            _xPosition.text = _ballController.GetStartPos().x.ToString();
            _yPosition.text = _ballController.GetStartPos().y.ToString();
            _zPosition.text = _ballController.GetStartPos().z.ToString();

            _minShootForce.text = _ballController.GetMinForce().ToString();
            _maxShootForce.text = _ballController.GetMaxForce().ToString();
            _minCurveForce.text = _ballController.GetMinCurveForce().ToString();
            _maxCurveForce.text = _ballController.GetMaxCurveForce().ToString();
            _angleDeviation.text = _ballController.GetAngleDeviation().ToString();
        }
    }
    public void HomeButtonClick()
    {
        //Return Main Menu
    }
    public void PauseButtonClick()
    {
        //Provisional...
        SceneManager.LoadScene("Football");
    }

    public void ResetButtonClick()
    {
        _ballController.ResetDebugPosition();
        _cameraController.PositionCameraBehindBall();
    }

    public void DebugButtonClick()
    {
        _showDebugTools = !_showDebugTools;
        _debugPanel.SetActive(_showDebugTools);
    }

    #region Debug Panel
    public void ResetBallPosition()
    {
        _ballController.SetStartPos(new Vector3(float.Parse(_xPosition.text), float.Parse(_yPosition.text), float.Parse(_zPosition.text)));
        _ballController.ResetDebugPosition();
        _cameraController.PositionCameraBehindBall();
    }

    public void SetNewPhysicValues()
    {
        _ballController.SetMinForce(float.Parse(_minShootForce.text));
        _ballController.SetMaxForce(float.Parse(_maxShootForce.text));
        _ballController.SetMinCurveForce(float.Parse(_minCurveForce.text));
        _ballController.SetMaxCurveForce(float.Parse(_maxCurveForce.text));
        _ballController.SetAngleDeviation(float.Parse(_angleDeviation.text));
    }

    #endregion
}
