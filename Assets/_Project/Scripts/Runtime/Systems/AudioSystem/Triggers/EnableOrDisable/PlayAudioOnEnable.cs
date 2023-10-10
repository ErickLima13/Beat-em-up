namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers.EnableOrDisable
{
    public class PlayAudioOnEnable : AudioTriggerBase
    {
        private void OnEnable()
        {
            PlayFirstSound();
        }
    }
}