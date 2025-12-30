using static Helper;

public abstract class AIGM_Object
{
    public OuterParameters OuterParameters { get { return _outerParameters; } set { _outerParameters = value; } }
    public Worker NetworkWorker { get { return _networkWorker; } set { _networkWorker = value; } }
    public AIGM_Transform Transform { get { return _transform; } set { _transform = value; } }
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public int Type { get { return _type; } set { _type = value; } }
    public int ID { get { return _ID; } set { _ID = value; } }

    private OuterParameters _outerParameters;
    private Worker _networkWorker;
    private AIGM_Transform _transform;

    private bool _isActive;
    private int _type;
    private int _ID;

    public AIGM_Object(OuterParameters outerParameters)
    { 
        _outerParameters = outerParameters; 
    }

    public AIGM_Object(object outerParameters) 
    {
        if (outerParameters is OuterParameters)
            _outerParameters = outerParameters as OuterParameters;
    }

    public AIGM_Object(OuterParameters outerParameters, AIGM_Transform transform)
    {
        _outerParameters = outerParameters;
        _transform = transform;
    }

    public virtual void Init() { }
    public virtual void InitLate() { }
    public virtual void Update() { }
    public virtual void UpdateLate() { }
    public virtual void Reset() { }

    public void PerformInit() { if (!IsActive) return; Init(); }
    public void PerformInitLate() { if (!IsActive) return; InitLate(); }
    public void PerformUpdate() { if (!IsActive) return; Update(); }
    public void PerformUpdateLate() { if (!IsActive) return; UpdateLate(); }
    public void PerformReset() { if (!IsActive) return; Reset(); }
}