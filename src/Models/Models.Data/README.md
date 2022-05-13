这是应用的核心模型层，它是对来自BiliBili的数据模型的一层抽象，从而降低对BiliBili数据模型的直接依赖。

> 这里只是数据模型定义，对于如何将 BiliBili 的数据模型转换为这里定义的数据模型，则需要借助项目 `Adapter.Data` 来进行。

对于数据类型，如果没有派生必要，则需要封闭该类 (添加 `sealed` 关键字)，明确类的封闭性可以对应用性能有一定的提升 (来自 [Performance benefits of sealed class in .NET](https://www.meziantou.net/performance-benefits-of-sealed-class.htm))。

这里的数据分为以下几类：

- Community  
    社群相关的类，比如评论、标签等具备一定社交属性的数据模型
- Local  
    应用定义的一些简单数据模型，以支持本地应用的相关功能
- Appearance  
    界面相关的数据模型，用以创建通用的视觉元素
- Player  
    播放器相关的数据模型，用来播放视频，记录进度等
- Video  
    视频相关的模型，用来记录视频信息等
- User  
    用户相关的数据模型，以 `UserProfile` 为核心
