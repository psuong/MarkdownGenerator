MarkdownGenerator
===
Generate markdown from C# binary & xml document for Hugo based sites.

How to Use
---
Clone and open solution, build console application.

Command Line Argument
- `[0]` = dll src path
- `[1]` = output directory 

Put .xml on same directory, use document comment for generate.

for example

```
MarkdownGenerator.exe UniRx.dll md
```