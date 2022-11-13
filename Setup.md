<img align="left" width="116" height="116" src="./Assets/net-logo.svg" />

# &nbsp;**.NET Incubator Setup**

<br/><br/><br/>


- [ ] Setup your environment - [How to video](https://www.youtube.com/watch?v=G1-Zfr9-3zs&list=PLLWMQd6PeGY2GVsQZ-u3DPXqwwKW8MkiP)
  - [ ] [Visual Studio 2022 Community](https://visualstudio.microsoft.com/downloads/) Installed
  - [ ] [.NET 7](https://dotnet.microsoft.com/download) Installed
  - [ ] [SQL Server Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) Installed
  - [ ] [SQL Server Management Studio SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) Installed
- [ ] SQL Incubator - [SQL basics for Beginners](https://www.youtube.com/watch?v=9Pzj7Aj25lw)

Setup Visual Studio
- [ ] Update the default C# Class in Visual Studio, everytime you create a new class it will use this template.
- [ ]  Open the following file in a text editor

```
C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Class\Class.cs
```

Change the template to the following, move using inside of the namespace and public before the class.

```cs
namespace $rootnamespace$;

using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$

public class $safeitemrootname$
{
}
```

Do the same with the Interface Template

```
C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Interface\Interface.cs
```

```cs
namespace $rootnamespace$;

using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$

public interface $safeitemrootname$
{
}
```


- [ ] [Import new Settings](./Assets/visualstudio.vssettings)

![](./Assets/2021-08-15-14-25-41.png)

![](./Assets/2021-08-15-14-26-35.png)

![](./Assets/2021-08-15-14-27-20.png)

Browse to **visualstudio.vssettings**

![](./Assets/2021-08-15-14-27-59.png)

At the bottom of Visual Studio click on the broom and choose **Configure Code Cleanup**. Choose everything except the bottom 2.

![](./Assets/2021-08-15-14-31-39.png)

Note on any file you can press Ctr+K and then Ctrl+E to cleanup your code. This helps for consitency and clean code.

### **Pezza Digital Solutions Intro**

- [ ] [Pezza Intro](https://github.com/entelect-incubator/.NET#intro)
