using EasyTransition;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TransitionSettings settings;

    [SerializeField] private float startDelay;

    [SerializeField] private string sceneName;

    public void StartGameButton()
    {
        TransitionManager.Instance().Transition(sceneName, settings, startDelay);
    }
}
