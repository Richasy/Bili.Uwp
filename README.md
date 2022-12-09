<p align="center">
<img src="https://i.loli.net/2020/08/30/sn8ov9cYDCGeWPk.png"/>
</p>

<div align="center">

# 哔哩

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Richasy/Bili.Uwp)](https://github.com/Richasy/Bili.Uwp/releases) ![GitHub Release Date](https://img.shields.io/github/release-date/Richasy/Bili.Uwp) ![GitHub All Releases](https://img.shields.io/github/downloads/Richasy/Bili.Uwp/total) ![GitHub stars](https://img.shields.io/github/stars/Richasy/Bili.Uwp?style=flat) ![GitHub forks](https://img.shields.io/github/forks/Richasy/Bili.Uwp)

`哔哩` 现在为 Windows 11 设计！
  
[![Release Builder](https://github.com/Richasy/Bili.Uwp/actions/workflows/release-builder.yml/badge.svg)](https://github.com/Richasy/Bili.Uwp/actions/workflows/release-builder.yml)

***目前由于技术原因和个人精力原因，该项目不便再继续维护下去，请考虑使用哔哩哔哩官方客户端或网页***

现阶段 `直播` 和 `动态` 模块因API限制，不再可用

</div>

---

`哔哩` 是一款 [哔哩哔哩](https://www.bilibili.com) 的第三方应用，使用 UWP 框架开发，是原生的 Windows 应用，支持 Windows 10/11 桌面系统以及版本号在 22000 以上的 XBOX。主打设计和易用性，~~广受用户好评~~。

## 🙌 简单的开始

> **商店版本** 和 **侧加载版本** 可以共存

### 从商店安装 (推荐)

将链接 `ms-windows-store://pdp/?productid=9mvn4nslt150` 复制到浏览器地址栏打开，从 Microsoft Store 下载。

**对于 XBOX 用户，请先在桌面设备中登录你的微软账户，并通过上面的方式进入哔哩的商店页面并获取应用，然后你就可以在 XBOX 中登录相同的账户，并在 "我的应用" 中找到哔哩了**。

商店版本仅支持 Windows 11 及以上的系统，更新频率为每月一次（如果当月有更新的话），时间是月底。

### 侧加载 (Sideload)

如果你想本地安装哔哩，或者尝试当月的最新功能。请打开右侧的 [Release](https://github.com/Richasy/Bili.Uwp/releases) 页面，找到最新版本，并选择适用于当前系统的安装包下载。

然后打开 [系统设置](ms-settings:developers)，打开 `开发者模式` ，并等待系统安装一些必要的扩展项。

在应用压缩包下载完成后，解压压缩包，并在管理员模式下，使用 **Windows PowerShell** *(不是PowerShell Core)* 运行解压后的 `install.ps1` 脚本，根据提示进行安装。

**Watch** 项目，以获取应用的更新动态。

关于如何一步步地使用侧加载 (Sideload) 方式安装 UWP 应用及订阅应用更新，请参见 [下载并安装哔哩的详细说明](https://github.com/Richasy/Bili.Uwp/wiki/%E4%B8%8B%E8%BD%BD%E5%B9%B6%E5%AE%89%E8%A3%85%E5%93%94%E5%93%A9%E7%9A%84%E8%AF%A6%E7%BB%86%E8%AF%B4%E6%98%8E) 。

## ❓ 常见问题

在应用的安装使用过程中，你可能会碰到一些问题，这篇文档也许可以帮助你解决遇到的困难：[常见问题](https://github.com/Richasy/Bili.Uwp/wiki/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98)

## 📃 文档

所有关于 `哔哩` 的文档，包括架构、使用说明等，都放在仓库的 [Wiki](https://github.com/Richasy/Bili.Uwp/wiki) 中，如果你发现有文档缺失或错误，请提交 [Issue](https://github.com/Richasy/Bili.Uwp/issues/new/choose) 说明错漏的内容。

## 🚀 协作

非常感谢有兴趣的开发者或爱好者参与 `哔哩` 项目，分享你的见解与思路。对于任何有兴趣想为 `哔哩` 做出贡献的小伙伴，请参见我们的 [哔哩 Wiki](https://github.com/Richasy/Bili.Uwp/wiki) 了解更多有关协作的内容和引导知识。

## 💬 讨论

借助 Github 平台提供的 Discussions 功能，对于一般讨论、提议或分享，我们都可以在 [哔哩论坛](https://github.com/Richasy/Bili.Uwp/discussions) 中进行，欢迎来这里进行讨论。

## ~~🌏 路线图~~

~~哔哩会逐步完善，请查看 [哔哩里程碑](https://github.com/Richasy/Bili.Uwp/milestones) 来了解哔哩下一步打算做的事情。与此同时，欢迎各位开发者加入，让我们一起打造哔哩的未来。~~

## 🧩 截图

*桌面*
![桌面截图](./assets/screenshot_desktop.png)

*XBOX*
![XBOX截图](./assets/screenshot_xbox.png)
