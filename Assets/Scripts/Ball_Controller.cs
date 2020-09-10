using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirEffect { NONE, LEFT, RIGHT};
public class Ball_Controller : MonoBehaviour
{

    [Header("Physics Variables")]
    [SerializeField] private float _baseForce;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _baseDist;
    [SerializeField] private float _baseCurveForce;
    [SerializeField] private float _minCurveForce;
    [SerializeField] private float _maxCurveForce;
    [SerializeField] private float _angleMarginToNotCurve;

    [Header("Components")]
    [SerializeField] private Rigidbody _rb;

    [Header("GameObjects")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _marker;

    private List<Vector2> _curvePath = new List<Vector2>();
    private List<Vector2> _curvePathWorldPlane = new List<Vector2>();

    private float _distanceToTarget;

    private int _intervalCount;
    private int _maxIntervalCount;

    private Vector3 _startShootPos;
    private Vector3 _shootDirection;
    private Vector3 _straightShootDirection;

    private bool _computeCurve;
    private int _contComputeCurve;
    private DirEffect _computeCurveDirection;
    private Vector3 _effectVector;
    private Vector3 _prevEffectVector;
    private float _angleDiffForCurve;
    public float _minSForce;
    public float _maxSForce;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        ApplyCurveToBall();
    }

    #region Getters & Setters
    public Vector3 GetStartPos()
    {
        return _startShootPos;
    }
    public void SetStartPos(Vector3 v)
    {
        _startShootPos = v;
    }
    public float GetMinForce()
    {
        return _minForce;
    }
    public void SetMinForce(float v)
    {
        _minForce = v;
    }
    public float GetMaxForce()
    {
        return _maxForce;
    }
    public void SetMaxForce(float v)
    {
        _maxForce = v;
    }
    public float GetMinCurveForce()
    {
        return _minCurveForce;
    }
    public void SetMinCurveForce(float v)
    {
        _minCurveForce = v;
    }
    public float GetMaxCurveForce()
    {
        return _maxCurveForce;
    }
    public void SetMaxCurveForce(float v)
    {
        _maxCurveForce = v;
    }
    public float GetAngleDeviation()
    {
        return _angleMarginToNotCurve;
    }
    public void SetAngleDeviation(float v)
    {
        _angleMarginToNotCurve = v;
    }
    #endregion

    /// <summary>
    /// Función que aplica la fuerza inicial al balón para lanzarlo contra el target
    /// </summary>
    /// <param name="indexMaxDist"></param>
    /// <param name="_sDirection"></param>
    /// <param name="_timeInterval"></param>
    /// <param name="_totalDistance"></param>
    /// <param name="_totalAngle"></param>
    /// <param name="_trace"></param>
    public void ShootBall( int indexMaxDist, Vector2 _sDirection, float _timeInterval, float _totalDistance, float _totalAngle, List<Vector2> _trace)
    {
        if(_timeInterval>=0.1f)
        {
            //Debug.DrawLine(transform.position, transform.position + transform.right * 3, Color.green, 10.5f);
            //Debug.DrawLine(transform.position, transform.position + transform.up * 3, Color.green, 10.5f);
            //Debug.DrawLine(transform.position, transform.position + transform.forward * 3, Color.green, 10.5f);

            //Debug.Log("New Base: " + transform.right + ", " + transform.up + ", " + transform.forward);
            //Debug.Log("Old Base: " + Vector3.right + ", " + Vector3.up + ", " + Vector3.forward);

            //Debug.Log(Screen.width);
            foreach (Vector2 v in _trace)
            {
                _curvePath.Add(v);
                if (v.x != Screen.width / 2)
                {
                    _curvePathWorldPlane.Add(new Vector2(v.x - (Screen.width / 2), v.y));
                }
            }

            //Debug purposes
            //foreach (Vector2 v in _curvePathWorldPlane)
            //{
            //    GameObject.Instantiate(_marker, new Vector3((v.x / 100), 0.25f, (v.y / 100)), Quaternion.identity);
            //}

            _shootDirection = new Vector3(_curvePathWorldPlane[indexMaxDist].x / 100, 0.45f, _curvePathWorldPlane[indexMaxDist].y / 100) - Vector3.zero;
            _straightShootDirection = new Vector3(_curvePathWorldPlane[_curvePathWorldPlane.Count - 1].x / 100, 0.45f, _curvePathWorldPlane[_curvePathWorldPlane.Count - 1].y / 100) - Vector3.zero;

            Debug.Log(Vector3.SignedAngle(transform.right, Vector3.right, Vector3.up));
            Quaternion q = Quaternion.AngleAxis(-Vector3.SignedAngle(transform.right, Vector3.right, Vector3.up), Vector3.up);
            _shootDirection = q * _shootDirection;
            _straightShootDirection = q * _straightShootDirection;


            //_shootDirection = new Vector3(transform.position.x + (_curvePathWorldPlane[indexMaxDist].x / 100), 0.25f, transform.position.z + (_curvePathWorldPlane[indexMaxDist].y / 100))
            //    - new Vector3(transform.position.x + (_curvePathWorldPlane[0].x / 100), 0.25f, transform.position.z + (_curvePathWorldPlane[0].y / 100))/*transform.position*/;
            //_straightShootDirection = new Vector3(transform.position.x + (_curvePathWorldPlane[_curvePathWorldPlane.Count - 1].x / 100), 0.25f, transform.position.z + (_curvePathWorldPlane[_curvePathWorldPlane.Count - 1].y / 100))
            //    - new Vector3(transform.position.x + (_curvePathWorldPlane[0].x / 100), 0.25f, transform.position.z + (_curvePathWorldPlane[0].y / 100));
            //_shootDirection = new Vector3(transform.localPosition.x + (_curvePathWorldPlane[indexMaxDist].x / 100), 0.25f, transform.localPosition.z + (_curvePathWorldPlane[indexMaxDist].y / 100))
            //    - new Vector3(transform.localPosition.x + (_curvePathWorldPlane[0].x / 100), 0.25f, transform.localPosition.z + (_curvePathWorldPlane[0].y / 100))/*transform.position*/;
            //_straightShootDirection = new Vector3(transform.localPosition.x + (_curvePathWorldPlane[_curvePathWorldPlane.Count - 1].x / 100), 0.25f, transform.localPosition.z + (_curvePathWorldPlane[_curvePathWorldPlane.Count - 1].y / 100))
            //    - new Vector3(transform.localPosition.x + (_curvePathWorldPlane[0].x / 100), 0.25f, transform.localPosition.z + (_curvePathWorldPlane[0].y / 100));

            Debug.DrawLine(transform.position, transform.position + _shootDirection.normalized * 10, Color.red, 7.5f);
            Debug.DrawLine(transform.position, transform.position + _straightShootDirection.normalized * 20, Color.blue, 7.5f);

            _distanceToTarget = (_target.transform.position - transform.position).magnitude;

            Vector2 _firstSection = _curvePath[1] - _curvePath[0];

            //Debug.Log("Raw: " + _totalDistance + " // Escalated: " + SuperLerp(_minForce, _maxForce, 0, Screen.height, _totalDistance));
            _shootDirection.Normalize();
            _shootDirection.y = 0.45f;  //Siempre disparamos en 45º en el eje Y?????

            //float _distFactor = SuperLerp((_minForce*_distanceToTarget)/_baseDist, (_maxForce*_distanceToTarget/_baseDist), 0, Screen.height, _totalDistance);

            ModifyForcesOfShoot();
            float _distFactor = SuperLerp(_minSForce, _maxSForce, 0, Screen.height, _totalDistance);
            //Debug.Log((_minForce * _distanceToTarget) / _baseDist);
            _rb.AddForce(_shootDirection.normalized * _distFactor / Mathf.Pow(_timeInterval, 1.05f) /*_timeInterval * 2*/);


            _startShootPos = transform.position;

            //Debug.Log(_timeInterval);

            Vector3 auxV1 = _straightShootDirection;
            auxV1.y = 0;
            Vector3 auxV2 = _shootDirection;
            auxV2.y = 0;
            _angleDiffForCurve = Vector3.Angle(auxV1, auxV2);


            _baseCurveForce = SuperLerp(_minCurveForce, _maxCurveForce, _angleMarginToNotCurve, 90, _angleDiffForCurve);
            //Debug.Log(_angleDiffForCurve + " // " + _baseCurveForce);
            //if (Vector3.Angle(_straightShootDirection, _shootDirection) > _angleMarginToNotCurve)
            if (_angleDiffForCurve > _angleMarginToNotCurve)
                _computeCurve = true;
        }
        else
        {
            //Intervalo de tiempo muy corto(mensaje de slide your finger to shoot!)
        }
        
    }

    /// <summary>
    /// Función que aplica una fuerza constante hacia el vector inercial del tiro
    /// </summary>
    private void ApplyCurveToBall()
    {
        if(_computeCurve)
        {
            Vector3 ballInStraightLine = ComputePointInLine(_startShootPos, _straightShootDirection, transform.position);
            ballInStraightLine.y = transform.position.y;

            if (_contComputeCurve <= 5)
            {
                if ((ballInStraightLine - transform.position).x < 0)
                    _computeCurveDirection = DirEffect.LEFT;
                else
                    _computeCurveDirection = DirEffect.RIGHT;
            }

            _effectVector = (ballInStraightLine - transform.position);

            if ((_computeCurveDirection == DirEffect.LEFT && _effectVector.x > 0) || (_computeCurveDirection == DirEffect.RIGHT && _effectVector.x < 0))
            {
                _rb.AddForce(_prevEffectVector.normalized * (_baseCurveForce /** ((ballInStraightLine - transform.position).magnitude / 100)*/));
            }
            else
            {
                _rb.AddForce(_effectVector.normalized * (_baseCurveForce /** ((ballInStraightLine - transform.position).magnitude / 100)*/));
                _prevEffectVector = _effectVector;
            }

            Debug.DrawLine(transform.position, transform.position + (ballInStraightLine - transform.position).normalized * (_baseCurveForce * ((ballInStraightLine - transform.position).magnitude / 100)), Color.cyan, 5.0f);

            _contComputeCurve++;
        }
        
    }

    /// <summary>
    /// Calcula la proyección de un vector sobre otro
    /// </summary>
    /// <param name="linePoint"></param>
    /// <param name="lineDir"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private Vector3 ComputePointInLine(Vector3 linePoint, Vector3 lineDir, Vector3 point)
    {
        //lineDir.y = 0.0f;
        lineDir.Normalize();
        Vector3 v = point - linePoint;
        float d = Vector3.Dot(v, lineDir);

        //Debug.Log("Algebra: " + (linePoint + lineDir * d));
        //Debug.Log("Unity: " + (linePoint + Vector3.Project((point-linePoint), lineDir.normalized)));

        //GameObject.Instantiate(_marker, linePoint + lineDir * d, Quaternion.identity);

        return linePoint + lineDir * d;
    }

    /// <summary>
    /// Función que permite resetear el tiro a la posición inicial
    /// </summary>
    public void ResetDebugPosition()
    {
        _rb.velocity = Vector3.zero;
        _curvePath.Clear();
        _curvePathWorldPlane.Clear();
        _computeCurve=false;
        _contComputeCurve=0;
        _computeCurveDirection=DirEffect.NONE;
        _rb.isKinematic = true;
        _rb.isKinematic = false;
        transform.position = _startShootPos;
        transform.LookAt(_target.transform, Vector3.up);
    }

    /// <summary>
    /// Función que escala un valor en otro rango de valores
    /// </summary>
    /// <param name="from">Minimo del rango hacia el que convertir</param>
    /// <param name="to">Máximo del rango hacia el que convertir</param>
    /// <param name="from2">Mínimo del rango que se convierte</param>
    /// <param name="to2">Máximo del rango que se convierte</param>
    /// <param name="value">Valor a convertir</param>
    /// <returns></returns>
    private float SuperLerp(float from, float to, float from2, float to2, float value)
    {
        if (value <= from2)
            return from;
        else if (value >= to2)
            return to;
        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    }

    /// <summary>
    /// Esta funcion escala el máximo y minimo de fuerza disponible para el balón en fucnión del target, basandonos en el base dist
    /// </summary>
    private void ModifyForcesOfShoot()
    {
        if(_distanceToTarget <= _baseDist)
        {
            _minSForce = _minForce;
            _maxSForce = _maxForce;
        }
        else
        {
            float sDifference_ = _distanceToTarget % _baseDist;     //Sumador diferencia
            float mDifference_ = _distanceToTarget / _baseDist;     //Multiplcador diferencia

            //_minSForce = (_minForce * (int)mDifference_) + (int)sDifference_;
            //_maxSForce = (_maxForce * (int)mDifference_) + (int)sDifference_;            
            _minSForce = _minForce + (int)_distanceToTarget - (int)_baseDist;
            _maxSForce = _maxForce + (int)_distanceToTarget - (int)_baseDist;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag=="Goal" || collision.gameObject.tag == "Atrezzo" || collision.gameObject.tag == "Barrier")
        _computeCurve = false;
    }
}
