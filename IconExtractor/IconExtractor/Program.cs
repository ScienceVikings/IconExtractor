using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using IWshRuntimeLibrary;

namespace IconExtractor {

  class IconRequest {

    public string Context { get; set; }
    public string Path { get; set; }
    public string Base64ImageData { get; set; }

  }

  class Program {

    private static bool useShortcutPath;

    static void Main(string[] args) {

      // https://msdn.microsoft.com/en-us/library/ms404308%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
      
      Console.InputEncoding = UTF8Encoding.UTF8;
      Console.OutputEncoding = UTF8Encoding.UTF8;

      useShortcutPath = args.Length > 0 && args[0] == "-x";
       
      while (true) {

        string input = Console.In.ReadLine().Trim();
        IconRequest data = JsonConvert.DeserializeObject<IconRequest>(input);

        try {

          data.Base64ImageData = getIconAsBase64(data.Path);
          Console.WriteLine(JsonConvert.SerializeObject(data));

        } catch (Exception ex) {
          Console.Error.WriteLine(ex);
          Console.Error.WriteLine(input);
        }
        
      }

    }

    static string getIconAsBase64(string path) {
      
      if (useShortcutPath && path.EndsWith(".lnk")) {
        path = getShortcutTarget(path);
      }
      
      Icon iconForPath = SystemIcons.Application;

      if (System.IO.File.Exists(path)) {
        iconForPath = Icon.ExtractAssociatedIcon(path);
      }

      ImageConverter vert = new ImageConverter();
      byte[] data = (byte[])vert.ConvertTo(iconForPath.ToBitmap(), typeof(byte[]));

      return Convert.ToBase64String(data);

    }

    static string getShortcutTarget(string path) {

      if (!System.IO.File.Exists(path)) {
        return "";
      }

      WshShell shell = new WshShell();
      IWshShortcut lnk = (IWshShortcut)shell.CreateShortcut(path);
      return lnk.TargetPath;

    }
  }
}
