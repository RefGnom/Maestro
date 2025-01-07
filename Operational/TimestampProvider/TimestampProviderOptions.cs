using System.Text;

namespace Maestro.Operational.TimestampProvider;

public class TimestampProviderOptions
{
    public string Filename { get; set; } = "timestamps";
    public string Path { get; set; } = Directory.GetCurrentDirectory();
    public string FullPath => System.IO.Path.Combine(Path, Filename);
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    public TimestampProviderOptions WithFilename(string filename)
    {
        Filename = filename;
        return this;
    }

    public TimestampProviderOptions WithPath(string path)
    {
        Path = path;
        return this;
    }

    public TimestampProviderOptions WithEncoding(Encoding encoding)
    {
        Encoding = encoding;
        return this;
    }
}