namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers.EnableOrDisable
{
    public class StopAudioOnDisable : AudioTriggerBase
    {
        private void OnDisable()
        {
            StopSound();
        }
    }
}