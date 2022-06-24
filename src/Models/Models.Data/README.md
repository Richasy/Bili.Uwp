这是应用的核心模型层，它是对来自BiliBili的数据模型的一层抽象，从而降低对BiliBili数据模型的直接依赖。

> 这里只是数据模型定义，对于如何将 BiliBili 的数据模型转换为这里定义的数据模型，则需要借助项目 `Adapter.Data` 来进行。

对于数据类型，如果没有派生必要，则需要封闭该类 (添加 `sealed` 关键字)，明确类的封闭性可以对应用性能有一定的提升 (来自 [Performance benefits of sealed class in .NET](https://www.meziantou.net/performance-benefits-of-sealed-class.htm))。