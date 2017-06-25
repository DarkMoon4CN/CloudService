# CloudService
蓝色星际云服务项目

Bsr.Cloud.WebEntry
是Web的入口，包括网页，asp.net程序以及REST服务接口。

Bsr.Cloud.Core
提供给其它项目用的公共类库的实现。它引用Bsr.Cloud.Model。

Bsr.Cloud.Model
是云服务的数据库定义及相关模块间交互用的公共类型定义，不引用项目内的任何类库。

Bsr.Cloud.BLogic
是核心逻辑业务层，通常由asp.net程序和REST服务接口来调用。