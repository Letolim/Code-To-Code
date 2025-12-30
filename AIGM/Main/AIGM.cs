using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AIGM
{
    public string Name { get { return _name; } private set { _name = value; } }
    public OuterParameters[][] OuterParameters { get { return _outerParameters; } set { _outerParameters = value; } }
    public bool StopThreads { get { return _stopNetworkThreads; } set { _stopNetworkThreads = value; } }
    public int ThreadCount { get { return _threadCount; } private set { _threadCount = value; } }
    public CancellationToken ChancellationToken { get { return _cts; } private set { _cts = value; } }

    private string _name;
    private OuterParameters[][] _outerParameters;

    private int _threadCount = 0;

    private bool[] _networkThreadStarted;
    private bool[] _networkThreadPaused;
    private bool[] _networkThreadFinished;
    private int[] _currentThreadStep;
    private int[] _threadSteps;

    private CancellationToken _cts;
    private bool _continueNetworkThreads = false;
    private bool _stopNetworkThreads = false;
    private bool _pauseNetworkThreads = false;
    private bool _allThreadsPaused;
    private bool _allThreadsFinished;
    private string _saveNetworkPath;
    private bool _saveNetwork;

    private NetworkManager networkManager;

    public AIGM(string name, OuterParameters[][] parameters)
    {
        _networkThreadStarted = new bool[parameters.Length];
        _networkThreadPaused = new bool[parameters.Length];
        _networkThreadFinished = new bool[parameters.Length];
        _currentThreadStep = new int[parameters.Length];
        _threadSteps = new int[parameters.Length];
        _threadCount = parameters.Length;

        for (int i = 0; i < parameters.Length; i++)
        {
            _networkThreadStarted[i] = false;
            _networkThreadPaused[i] = false;
            _networkThreadFinished[i] = false;
            _currentThreadStep[i] = 0;
            _threadSteps[i] = 5000000;
        }

        OuterParameters = parameters;
        networkManager = new NetworkManager(parameters, 0);
        Init();
    }

    public void SaveNetwork(string path)
    {
        _saveNetworkPath = path;
        _saveNetwork = true;
    }

    public int StartThreads()
    {
        Task[] tasks = new Task[_threadCount];
        CancellationTokenSource ctsSource = new CancellationTokenSource();
        _cts = ctsSource.Token;

        for (int i = 0; i < _threadCount; i++)
        {
            int threadId = i;
            tasks[i] = Task.Run(() => StartWork(threadId, _cts));
        }

        return tasks.Length;
    }

    public async Task StartWork(int threadId, CancellationToken cts)
    {
        try
        {
            await DoWorkAsync(threadId, cts);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
    }

    private async Task DoWorkAsync(int theadId, CancellationToken token)
    {
        await Task.Run(() => ThreadLoop(0, token));
    }

    public int a = 0;

    public void ThreadLoop(int id, CancellationToken cts, bool resetOuterParametersFirst = true)
    {
        _networkThreadStarted[id] = true;

        while (!_stopNetworkThreads && !cts.IsCancellationRequested)
        {
            cts.ThrowIfCancellationRequested();

            if (_stopNetworkThreads)
                break;

            if (_networkThreadFinished[id])
                continue;

            if (_networkThreadPaused[id])
            {
                if (_continueNetworkThreads)
                    _networkThreadPaused[id] = false;
                continue;
            }

            if (_pauseNetworkThreads && !_networkThreadPaused[id])
            {
                _networkThreadPaused[id] = true;
                continue;
            }

            for (int n = 0; n < OuterParameters[id].Length; n++)
                OuterParameters[id][n].PerformUpdate(_currentThreadStep[id], _threadSteps[id]);

            _currentThreadStep[id]++;

            if (_currentThreadStep[id] == _threadSteps[id])
            {
                for (int n = 0; n < OuterParameters[id].Length; n++)
                    OuterParameters[id][n].PerformReset(resetOuterParametersFirst);

                _networkThreadFinished[id] = true;
            }
            a = 6;
        }
    }

    public void Update(bool pauseThreads, bool continueThreads)
    {
        if (pauseThreads)
            _pauseNetworkThreads = true;

        if (continueThreads)
            _continueNetworkThreads = true;

        if (_saveNetwork)
        {
            _pauseNetworkThreads = true;
            return;
        }

        if (_saveNetwork && _allThreadsPaused)
        {
            SaveCurrentTopNeuralNetwork(_saveNetworkPath);
            _pauseNetworkThreads = false;
            _continueNetworkThreads = true;
            _saveNetwork = false;
            return;
        }else 
            _allThreadsPaused = _networkThreadPaused.All(value => value);

        if (_allThreadsFinished && !_pauseNetworkThreads)
        {
            networkManager.CombineNetworks(OuterParameters, 100);
            networkManager.ResetNetworks();

            for (int i = 0; i < _threadCount; i++)
            {
                _currentThreadStep[i] = 0;
                _networkThreadFinished[i] = false;
            }

            return;
        }
        else 
            _allThreadsFinished = _networkThreadFinished.All(value => value);
    }

    public void Init(bool initOuterParametersFirst = true)
    {
        for (int i = 0; i < OuterParameters.Length; i++)
            for (int n = 0; n < OuterParameters[i].Length; n++)
                OuterParameters[i][n].PerformInit(initOuterParametersFirst);
    }

    public void SaveCurrentTopNeuralNetwork(string path = @"C:/Saves/") { throw new System.NotImplementedException(); }
    public void SaveCurrentTopNeuralNetwork(int count, string path = @"C:/Saves/") { throw new System.NotImplementedException(); }
    public NeuralNetwork LoadNeuralNetwork(string path) { throw new System.NotImplementedException(); }
}