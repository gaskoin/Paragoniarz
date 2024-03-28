namespace Paragoniarz.Domain.Settings;

public interface IConfigurationService
{
    Configuration LoadConfiguration();
    void SaveConfiguration(Configuration configuration);
}
