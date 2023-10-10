using TMPro;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.UI.Base
{
    public abstract class DropDownBase : MonoBehaviour
    {
        protected TMP_Dropdown _dropDown;

        protected virtual void Awake()
        {
            _dropDown = GetComponent<TMP_Dropdown>();
            _dropDown.onValueChanged.AddListener(DropDownAction);
        }
        protected abstract void DropDownAction(int value);
    }
}