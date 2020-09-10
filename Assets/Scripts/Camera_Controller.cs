using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{

    [Header("Camera Variables")]
    [SerializeField] private Vector3 _stationaryShootDistance;

    [Header("GameObjects")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _target;


    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void PositionCameraBehindBall()
    {
        //Tener en cuenta los disparos con el balon en movimiento

        Vector3 aux = (_target.transform.position - _ball.transform.position).normalized;
        Vector3 finalPos = _ball.transform.position + (aux * _stationaryShootDistance.z);
        finalPos.y = _stationaryShootDistance.y;

        transform.position = finalPos;
        transform.LookAt(_target.transform, Vector3.up);
    }

    public void CameraShootAction()
    {

    }
}
