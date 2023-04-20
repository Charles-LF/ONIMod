# 《缺氧》 Mod 制作指南- HelloWorld


在本章节，将会制作您的第一个MOD，~~想想都觉得很激动呢~~，废话不多说，让我们准备开始吧！！！

---

### 一、启动VS
+ 启动vs，在主屏幕选择 `创建新项目` ，选择`类库（.NET Framework）用于创建C#类库（.dll）的项目` ，然后下一步。
+ 填写你的项目名称，选择项目存放的位置，选择框架为`.NET Framework 4.7.1`，其他随意，看个人，然后点击创建，你就创建好了一个基本的开发环境。

---
### 二、添加依赖

创建项目后，是时候导入编写Mod需要用到的依赖文件了，这样才能让IDE知道你在干些什么，~~你知道你在干什么的对吧~~。让我们开始：
+ 转到你保存代码的文件夹，里面应该有`{你的解决方案名称}.sln`。
+ 在目录下创建一个名为`lib`的文件夹用来存放马上用到的依赖文件。
+ 回到`OxygenNotIncluded\OxygenNotIncluded_Data\Managed`文件夹，找到以下文件并将它们复制到lib目录:
    + Assembly-CSharp.dll
    + Assembly-CSharp-firstpass.dll
    + 0Harmony.dll
    + UnityEngine.dll
    + UnityEngine.CoreModule.dll
+ 下面这些不常用，但说不定什么时候可能就用到了：
    + UnityEngine.UI.dll
    + Unity.TextMeshPro.dll
    + UnityEngine.ImageConversionModule.dll  

+ 复制完成以后，回到VS的主界面，在`解决方案资源管理器`找到`引用`,右键，添加引用，浏览，选择刚刚复制到lib文件夹里的所有dll文件。然后确认。添加依赖部分就完成咯。
+ 点开引用，点击第一个依赖项，按住`shift`,点击最后一个依赖项目，选择全部的依赖项目，在`解决方案资源管理器`下方的依赖属性中，点击`复制本地`右边的框框，选择`False`,不然依赖会在生产的时候复制一份到生成目录里。

---

### 三、游戏日志和模组目录
+ 游戏输出日志位于`%USERPROFILE%\AppData\LocalLow\Klei\Oxygen Not Included\player.log`。这个文件将会是你开发Mod路上最好的朋友，好好使用日志将会使你的开发更加轻松。有关如何使用日志的更多详细信息将在另帖子上发布。
+ 模组开发目录位于`%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods`，在这里，通常有一个存在的文件夹`steam`，里面存放着你在创意工坊下载的模组。
+ 现在，请在当前目录下创建一个和`steam`同级的目录`Dev`,你开发的所有Mod都应当放在这个目录里面进行测试。

---


### 四、HelloWorld  

终于是开始了代码部分，在此之前，我猜你已经阅读了不下两遍的`Harmony 文档`，并且已经理解了其中各类方法的使用。熟悉以后，让我们直接上代码：
```csharp
using HarmonyLib;
using KMod;
using UnityEngine;

namespace ONIMod
{
    public class Patches : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Prefix
        {
            public static void Prefix()
            {
                Debug.Log("HelloWorld,This is Prefix method");
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Postfix
        {
            public static void Postfix()
            {
                Debug.Log("HelloWorld Again,This is Postfix method");
            }
        }
    }
}
```
+ 如你所见，`Patches`包含两个修补方法，其中`Db_Prefix`和`Db_Postfix`将在原始代码`Db.Initialize`前后执行（给你最后一次机会去看`Harmony 文档`）。你可以在前面所提到的`游戏日志`里找到他们。所以的补丁都应当放在同一个地方，通常是`Patches`类，你把补丁放在哪里不重要，但应当保持代码整洁易读，别到时候回来修bug的时候读不懂自己的代码。

+ 你会发现代码中`Patches`继承自`UserMod2`，你不需要做任何事情来注册他：`默认情况下，程序会在开始时读取并应用此处的代码`，如果你需要在补丁前或者之后做些什么，可以阅读不知道什么时候会更新的同名帖子`UserMod2`。

+ 代码写完后，你可以点击菜单栏的`生成`，`生成解决方案`或者`生成{项目名称}`，如果没有报错，你可以在项目文件夹内找到`bin`，里面包含两个文件夹，取决于你生成项目的方式，一般在`Debug`文件夹内能找到你项目名称的`dll`文件,在上面所提到的`模组目录`里新建一个文件夹，名字随意，把生成的dll文件拖进去。代码部分，基本就完成了。

### 五、 mod. yaml 和 mod_info.yaml
mods使用两个数据文件-它们将在不知道什么时候会更新的章节全面描述。

您需要创建：
+ mod.yaml  -- 虽然mod没有它也可以正常运行，但不要省略它！
    ```yaml
    title: "模组的标题"
    description: "模组的介绍"
    staticID: "模组供别的模组调用（或许会实现）的唯一ID" 
    ```

+ mod_info.yaml --这个是必须有的，没有就会报错，某些不兼容DLC或者代码能正常跑的远古mod或许可以修改这个来让他正常跑？
    ```yaml
    supportedContent: ALL # 支持的版本，ALL：全部支持，VANILLA_ID ：仅原版游戏。EXPANSION1_ID 仅眼冒金星
    minimumSupportedBuild: 468097 # 模组能运行的最低游戏版本
    version: 1.0.0 # 模组的版本
    APIVersion: 2 # 所以包含.dll的Mod必须将此项设置为2，以声明你使用的 Harmony 版本为 2，如果没有设置，模组可能不会加载或者会出现奇奇怪怪的问题。
    ```

现在，在你刚刚在`Dev`文件夹下创建的文件夹里新建上述两个`.yaml`文件，现在，你在`Dev`文件夹下创建的文件夹里应当有以下三个文件：
+ mod.yaml
+ mod_info.yam
+ 在VS编写代码生成的.dll

### 测试

+ 确保上述步骤无误，打开游戏，你应该可以在游戏模组界面看到你编写的模组，点击启用然后按照提示游戏。
+ 是时候接受命运的审判了：打开前面提到的日志文件，如果你在日志里发现游戏加载了你编写的Mod，并且在日志里输出了我们在程序里写的`HelloWorld`，那么，恭喜你，您现在已经是一名合格的缺氧模组制作者了。
+ 如果有其他疑问或者有其他建议，欢迎在本帖留言或者联系管理员分配`Modder`身份组以获得在`缺氧Mod制作组`子频道发言的权限。
---
## 以上，下一章节将讲解如何上传至steam