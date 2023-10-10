namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers.EnableOrDisable
{
    public class StopAudioOnEnable : AudioTriggerBase
    {
        private void OnEnable()
        {
            StopSound();
        }
    }
}