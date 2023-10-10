namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers.EnableOrDisable
{
    public class PlayAudioOnDisable : AudioTriggerBase
    {
        private void OnDisable()
        {
            PlayFirstSound();
        }
    }
}