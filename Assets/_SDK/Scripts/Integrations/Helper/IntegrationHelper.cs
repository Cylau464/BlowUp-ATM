namespace apps
{
    public class IntegrationHelper : UnityEngine.MonoBehaviour
    {
        private void OnApplicationPause(bool pause)
        {
            AppsIntegration.OnApplicationPause(pause);
        }
    }
}
