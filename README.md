# TinyKLX
A sample spyware written in VB.NET
#### It is just a proof of existance of Visual Basic .Net users.
## Customize
To Build it for your own testing, follow these steps
* Clone Solution
* Edit Config.vb file according to your requirements
* Set .Net Framework for specific target
* Build Solution
* Use [ConfuserEx](https://github.com/yck1509/ConfuserEx) CLI with TinyKLX.crproj to obfuscate your app
* Finally your app will be ready to use
## Mechanism
This malware uses classic malware techniques to persist.
* It is built against .Net making machine code much complex.
* Since it is confused with ConfuserEx, hiding flow and data, making it almost undetectable.
But still it follows a specific pattern which can be detected as follow:
* When it is launched, it shows fake messege `Config.UserErrorMessage` in foreground and re-starts itself with `Config.SafeCommand`
* As soon as it is launched with `Config.SafeCommand`, it checks its execution location. If it is not a temp path then it copies itself to temp path and starts from temp path with safe command
* If successfully previous two steps are performed then it sets itself to be started again when computer restarted from `"RunOnce"` key with name `Config.AppName`
* Since RunOnce Keys are removed everytime app is started. It sets again and again.
* When persistance is verified, it starts logging
* Every key is captured, and screenshot is captured when user presses enter key or mouse click (called capture event).
* All this captured events are stored till their number exceed `Config.MaxEvents`.
* When certain amount of events are captured they are then compiled as HTML file which is GZipped and transmitted to network via Email.
* This loop continues for ever.
