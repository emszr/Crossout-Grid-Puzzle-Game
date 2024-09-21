using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using ObjectPoolManagement;
using UnityEngine;
using Zenject;

public class MainMenuSceneLoader : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private GameManager _gameManager;
    private SignalManager _signalManager;
    
    private List<Manager> _managers;

    [Inject]
    public void InstallDependencies(ObjectPoolManager objectPoolManager, SignalManager signalManager, GameManager gameManager)
    {
        _objectPoolManager = objectPoolManager;
        _gameManager = gameManager;
        _signalManager = signalManager;
    }
    
    private void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _managers = new();
        

        _managers.Add(_objectPoolManager);

        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        await UniTask.WaitUntil(AreAllManagersInitialized);
        
        _gameManager.SetGameState(GameState.OnMenu);
        
        return;
        

        bool AreAllManagersInitialized()
        {
            foreach (var manager in _managers)
            {
                if (!manager.IsInitialized)
                {
                    return false;
                }
            }

            return true;
        }
    }
}