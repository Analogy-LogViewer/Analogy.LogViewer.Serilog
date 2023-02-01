# Analogy LogViewer Serilog

Serilog Parser for Analogy Log Viewer.
| Nuget   |      Version      |  Description |
|----------|:-------------:|------|
| [Analogy.LogViewer.Serilog](https://www.nuget.org/packages/Analogy.LogViewer.Serilog/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogViewer.Serilog)](https://www.nuget.org/packages/Analogy.LogViewer.Serilog) | Serilog Extension for Analogy Log viewer (this package) |
| [Analogy.LogViewer.Serilog.Sinks](https://www.nuget.org/packages/Analogy.LogViewer.Serilog.Sinks/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogViewer.Serilog.Sinks)](https://www.nuget.org/packages/Analogy.LogViewer.Serilog.Sinks) | Serilog Sink for sending logs to Analogy Log server |

Supported formatters, for now, are:

1. [Compact formatting](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog#Compact-formatting)
2. [Regular Expression Parser](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog#regular-expression-parser)
3. [Real Time Sink](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog#Real-Time-Sink)

#### Compact formatting
[Compact formatting](https://github.com/serilog/serilog-formatting-compact). The initial version is based on [this project](https://github.com/serilog/serilog-formatting-compact-reader)

   example log:
   ```json
   {"@t":"2016-10-12T04:46:58.0554314Z","@mt":"Hello, {@User}","User":{"Name":"nblumhardt","Id":101}}
   {"@t":"2016-10-12T04:46:58.0684369Z","@mt":"Number {N:x8}","@r":["0000002a"],"N":42}
   {"@t":"2016-10-12T04:46:58.0724384Z","@mt":"Tags are {Tags}","@l":"Warning","Tags":["test","orange"]}
   {"@t":"2016-10-12T04:46:58.0904378Z","@mt":"Something failed","@l":"Error", "@x":"System.DivideByZer...<snip>"}
   ```

#### Regular Expression Parser
Regular Expression Parser: in this mode you need to define your custom regex to match you log format in the application settings.

![Serilog Settings](Assets/SerilogRegularExpression.jpg)

for example, with the above regex: in the screenshot this example log can be parsed:
```
$2020-04-24 13:18:23,207|1|INFO|logsource|My Manager App Starting...
$2020-04-24 13:28:24,380|1|WARN|files|file not found
$2020-04-24 13:48:27,193|2|INFO|AppBase|Loading done
   ```
 
![Serilog Settings](Assets/serilogParserExample.jpg)

the available tags to use for parsing are:

   ```csharp
   public enum AnalogyLogMessagePropertyName
  {
    Date,
    ID,
    Text,
    Category,
    Source,
    Module,
    MethodName,
    FileName,
    User,
    LineNumber,
    ProcessId,
    ThreadId,
    Level,
    Class,
    MachineName,
  }
 ```


## History


### V3.6.1(01.02.2023):
- https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog/issues/499: Can't parse dictionary that contains an empty key #499

