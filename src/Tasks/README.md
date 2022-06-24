# 后台任务项目介绍

该项目是一个 WinRT Component，关于 UWP 后台任务的具体介绍，可以参见 [Support your app with background tasks](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/support-your-app-with-background-tasks)

就目前来说，哔哩支持的后台项目只有新动态通知，它会以15分钟每次的频率定时请求新的动态（如果用户登录了的话），通过和本地缓存的最新动态 Id 比较，如果有新的动态就进行通知