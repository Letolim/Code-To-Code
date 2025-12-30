using System.Collections.Generic;
using UnityEngine;

public abstract class OuterParameters
{
    public List<AIGM_Object> Objects { get { return _objects; } private set { _objects = value; } }
    public List<AIGM_Object> NetworkObjects { get { return _networkObjects; } private set { _networkObjects = value; } }

    public int CurrentStep { get { return _currentStep; } private set { _currentStep = value; } }
    public int Steps { get { return _steps; } private set { _steps = value; } }

    private List<AIGM_Object> _objects;
    private List<AIGM_Object> _networkObjects;

    private int _steps = 0;
    private int _currentStep = 0;
    private int _currentID = 0;

    public OuterParameters()
    {
        _objects = new List<AIGM_Object>();
        _networkObjects = new List<AIGM_Object>();
    }

    public OuterParameters(AIGM_Object[] aigmObjects) 
    {
        _objects = new List<AIGM_Object>();
        _networkObjects = new List<AIGM_Object>();
        AddAIGM_Objects(aigmObjects);
    }

    public void AddAIGM_Objects(AIGM_Object[] aigmObjects)
    {
        foreach(AIGM_Object aigmObject in aigmObjects)            
            AddAIGM_Object(aigmObject);
    }

    public void AddAIGM_Object(AIGM_Object aigObject)
    {
        aigObject.ID = _currentID;

        if (aigObject.NetworkWorker != null)
            _networkObjects.Add(aigObject);
        else
            _objects.Add(aigObject); 

        _currentID++;
    }

    public void RemoveAIGM_Object(int ID)
    {
        for (int i = 0; i < _objects.Count; i++)
            if (_objects[i].ID == ID)
            {
                _objects.RemoveAt(i);
                return;
            }

        for (int i = 0; i < _networkObjects.Count; i++)
            if (_networkObjects[i].ID == ID)
            { 
                _networkObjects.RemoveAt(i);
                break;
            }
    }

    public AIGM_Object GetAIGM_Object(int ID)
    {
        for (int i = 0; i < _objects.Count; i++)
            if (_objects[i].ID == ID)
                return _objects[i];

        for (int i = 0; i < _networkObjects.Count; i++)
            if (_networkObjects[i].ID == ID)
                return _networkObjects[i];

        return null;
    }

    public AIGM_Object GetAIGM_Worker_Object(int ID)
    {
        for (int i = 0; i < _networkObjects.Count; i++)
            if (_networkObjects[i].ID == ID)
                return _networkObjects[i];

        return null;
    }

    public abstract void Init();
    public abstract void Update();
    public abstract void Reset();

    public void PerformInit(bool initOuterParametersFirst) 
    {
        if (initOuterParametersFirst)
        {
            Init();

            foreach (AIGM_Object @object in Objects)
                @object.PerformInit();

            foreach (AIGM_Object @object in NetworkObjects)
                @object.PerformInit();
        }else
            {
                foreach (AIGM_Object @object in Objects)
                    @object.PerformInit();

                foreach (AIGM_Object @object in NetworkObjects)
                    @object.PerformInit();

                Init();
            }
    }

    public void PerformUpdate(int steps, int currentStep) 
    {
        _steps = steps;
        _currentStep = currentStep;

        foreach (AIGM_Object @object in Objects)
            @object.PerformUpdate();

        foreach (AIGM_Object @object in NetworkObjects)
            @object.PerformUpdate();

        Update();

        foreach (AIGM_Object @object in Objects)
            @object.PerformUpdateLate();

        foreach (AIGM_Object @object in NetworkObjects)
            @object.PerformUpdateLate();
    }

    public void PerformReset(bool resetOuterParametersFirst) 
    {
        if (resetOuterParametersFirst)
        {
            Reset();

            foreach (AIGM_Object @object in Objects)
                @object.PerformReset();

            foreach (AIGM_Object @object in NetworkObjects)
                @object.PerformReset();
        }
        else
            {
                foreach (AIGM_Object @object in Objects)
                    @object.PerformReset();

                foreach (AIGM_Object @object in NetworkObjects)
                    @object.PerformReset();

                Reset();
            }
    }

    public void SetCurrentStep(int step) { _currentStep = step; }
    public void SetSteps(int steps) { _steps = steps; }
}