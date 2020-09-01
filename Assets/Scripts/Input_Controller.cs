using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Input_Controller : MonoBehaviour
{

    [Header("GameObject References")]
    [SerializeField] private GameObject _ball;

    public bool _shootStarted;
    public float _startShootTime;
    public Vector2 _startShootScreenPos;
    public float _endShootTime;
    public Vector2 _endShootScreenPos;
    public List<Vector2> _shootTrace = new List<Vector2>();

    public float _totalAngle;
    public float _totalDistance;
    public float _totalTimeInterval;
    public Vector2 _shootDirection;

    void Start()
    {
        _shootStarted = false;
    }

    void Update()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if(Input.GetMouseButton(1))
        {
            //Slow time(slowmotion)
        }
        else if(Input.GetMouseButton(0))
        {
            //Shoot procedure
            if(!_shootStarted)
            {
                _shootStarted = true;
                _startShootTime = Time.time;
                _startShootScreenPos = Input.mousePosition;
            }
            else
            {
                //Save positions to get a trace
                if(!_shootTrace.Contains(Input.mousePosition))
                    _shootTrace.Add(Input.mousePosition);

            }
        }
        else if(Input.GetMouseButtonUp(0) && _shootStarted)
        {
            //Shoot end
            _shootStarted = false;
            _endShootTime = Time.time;
            _endShootScreenPos = Input.mousePosition;

            GetTotalAngle();
        }
    }

    private void GetTotalAngle()
    {
        //Reset total angle
        _totalAngle = 0;

        //Process data
        for(int i=0; i<_shootTrace.Count-2; i++)        //Use the -2 to evade OutOfIdexException
        {
            Vector2 v1 = _shootTrace[i + 1] - _shootTrace[i];
            Vector2 v2 = _shootTrace[i + 2] - _shootTrace[i+1];

            _totalAngle += Vector2.SignedAngle(v1, v2);
        }
        _totalDistance = (_endShootScreenPos - _startShootScreenPos).magnitude;
        _totalTimeInterval = _endShootTime - _startShootTime;
        _shootDirection = _endShootScreenPos - _startShootScreenPos;

        //Shoot the ball
        //_ball.GetComponent<Ball_Controller>().ShootBall(MaxDistanceFromStraightLineShoot(), _shootDirection, _totalTimeInterval, _totalDistance, _totalAngle, _shootTrace);
        _ball.GetComponent<Ball_Controller>().ShootBall(MiddleTraceShoot(), _shootDirection, _totalTimeInterval, _totalDistance, _totalAngle, _shootTrace);

        //Clear trace list
        _shootTrace.Clear();
    }

    private int MaxDistanceFromStraightLineShoot()      
    {
        int indexMaxDist = 0;
        float maxD = 0;

        int cont = 0;
        foreach (Vector2 v in _shootTrace)
        {
            if (Mathf.Abs(_startShootScreenPos.x-v.x) > maxD)
            {
                indexMaxDist = cont;
                maxD = Mathf.Abs(_startShootScreenPos.x - v.x);
            }
            cont++;
        }

        //Debug.Log(maxD + "//" + indexMaxDist);

        return indexMaxDist;
    }

    private int MiddleTraceShoot()
    {
        return _shootTrace.Count/2;
    }
}
