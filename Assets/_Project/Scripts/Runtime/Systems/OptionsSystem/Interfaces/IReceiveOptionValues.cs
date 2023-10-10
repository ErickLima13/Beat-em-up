using PainfulSmile.Runtime.Systems.OptionsSystem.Core;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Interfaces
{
    public interface IReceiveOptionValues<T>
    {
        public OptionType OptionType { get; set; }

        public abstract void ReceiveValue(OptionType optionType, T value);
        public void RegisterGlobalCallback(IReceiveOptionValues<T> callback);
        public void UnRegisterGlobalCallback(IReceiveOptionValues<T> callback);
    }
}