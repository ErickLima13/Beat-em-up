using UnityEngine;
using UnityEngine.UI;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.UI.Base
{
    public abstract class ToggleBase : MonoBehaviour
    {
        protected Toggle toggle;
        [SerializeField] protected BoolDefaultValueData defaultValue;

        protected virtual void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(ToggleAction);
        }

        protected abstract void ToggleAction(bool value);
    }

}