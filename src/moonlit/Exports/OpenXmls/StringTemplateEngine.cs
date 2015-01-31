namespace Moonlit.Exports.OpenXmls
{
    //public class OpenXmlExport
    //{
    //    private readonly IEngine _engine;

    //    public OpenXmlExport(IEngine engine)
    //    {
    //        _engine = engine;
    //    }
    //    public void Add(string name, object value)
    //    {
    //        _engine.Add(name, value);
    //    }
    //    public byte[] ExportFromTemplateFile<T>(string zipFile)
    //    {
    //        var tmpPath = Path.GetTempFileName();
    //        File.Delete(tmpPath);
    //        Directory.CreateDirectory(tmpPath);
    //        FileInfo zipFileInfo = new FileInfo(zipFile);

    //        zipFileInfo.UnZip(tmpPath);

    //        Parse(tmpPath);
    //        DirectoryInfo tmpDir = new DirectoryInfo(tmpPath);
    //        var output = Path.ChangeExtension(tmpPath, "zip");
    //        tmpDir.Zip(output);
    //        return File.ReadAllBytes(output);
    //    }

    //    private void Parse<T>(string tmpPath)
    //    {
    //        var files = Directory.GetFiles(tmpPath, "*.xml", SearchOption.AllDirectories);
    //        foreach (var fileName in files)
    //        {
    //            var text = _engine.Parse(File.ReadAllText(fileName, Encoding.UTF8), new TemplateModel<T>(model));
    //            File.WriteAllText(fileName, text, Encoding.UTF8);
    //        }
    //    }
    //}

    //public interface IEngine
    //{
    //    string Parse<T>(string template);
    //    void Add(string name, object value);
    //}

    //public class TemplateEngineeParameter
    //{
    //    public string Name { get; set; }
    //    public object Value { get; set; }

    //    public TemplateEngineeParameter()
    //    {

    //    }

    //    public TemplateEngineeParameter(string name, object value)
    //    {
    //        Name = name;
    //        Value = value;
    //    }
    //}
    //public class TemplateEnginee
    //{
    //    private List<TemplateEngineeParameter> _parameters = new List<TemplateEngineeParameter>();


    //    public void Add(string key, object value)
    //    {
    //        _parameters.Add(new TemplateEngineeParameter(key, value));
    //    }

    //    public string Parse(string text)
    //    {
    //        var template = new Template(text, '$', '$');
    //        foreach (var parameter in _parameters)
    //        {
    //            template.Add(parameter.Name, parameter.Value);
    //        }
    //        return template.Render();
    //    }
    //}
}
