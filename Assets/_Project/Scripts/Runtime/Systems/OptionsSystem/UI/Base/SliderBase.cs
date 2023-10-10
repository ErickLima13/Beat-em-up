using UnityEngine;
using UnityEngine.UI;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.UI.Base
{
    public abstract class SliderBase : MonoBehaviour
    {
        protected Slider slider;
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        protected void AddListener()
        {
            slider.onValueChanged.AddListener(SliderUpdateAction);
        }

        protected void RemoveListener()
        {
            slider.onValueChanged.RemoveListener(SliderUpdateAction);
        }

        protected virtual void SetMinMax(float min, float max)
        {
            slider.minValue = min;
            slider.maxValue = max;
        }
        protected abstract void SliderUpdateAction(float value);
    }
}