# 候鸟留学API接口文档
## 1.API接口说明

- 接口基准地址： http://43.142.41.192:6001/

- 服务端已开启 CORS 跨域支持 
- ~~API V1 认证统一使用 Token 认证 （）~~
- ~~需要授权的 API ，必须在请求头中使用 Authorization 字段提供 token 令牌 （）~~
- 使用 HTTP Status Code 标识状态
- 数据返回格式统一使用 JSON 

### 1.1.支持的请求方法
- GET(SELECT):请求指定的页面信息，并返回实体主体。
- ~~HEAD:类似于 GET 请求，只不过返回的响应中没有具体的内容，用于获取报头~~
- POST(CREATE):向指定资源提交数据进行处理请求（例如提交表单或者上传文件）。数据被包含在请求体中。POST 请求可能会导致新的资源的建立和/或已有资源的修改。
- PUT(UPDATE):从客户端向服务器传送的数据取代指定的文档的内容。
- PATCH(UPDATE):是对 PUT 方法的补充，用来对已知资源进行局部更新。
- DELETE(DELETE):请求服务器删除指定的页面。
- OPTIONS:允许客户端查看服务器的性能。
- TRACE:回显服务器收到的请求，主要用于测试或诊断。

### 1.2.通用返回状态说明

| 状态码 | 意义 | 说明 |
| :-: | :-: | :-: |
| 200 | OK | 请求成功 |
|300|Failed|找不到指定资源|
|400|Bad Request|服务器未能处理|


## 2.API接口描述

### 2.1.登录注册相关

#### 2.1.1.用户登录验证（完成）

> 用户登录

- 请求路径：api/login
- 请求方法：post

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: |:-:|
|user_id|int|非空|用户ID|
|user_password|string|非空，符合密码规范|密码|

- 响应数据
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-:|
|status|bool|状态码|标明登录是否成功|
|user_id|int|非空|用户id|
|user_password|string|非空|用户密码|
|user_name|string|无|用户名|
|user_profile|string|无|用户头像相对地址|
|user_email|string|无|用户邮箱|
|user_phone|string|无|用户电话|
|user_createtime|date|年月日的int形式|用户注册日期|
|user_birthday|date|年月日的int形式|用户生日|
|user_gender|char|无|用户性别|
|user_state|bool|无|用户封禁状态|
|user_signature|string|无|用户个性签名|
|user_follower|int|无|用户粉丝数|
|user_follows|int|无|用户关注数|
|user_level|int|无|用户等级|
|user_coin|int|非空|用户鸟币数|

#### 2.1.2.用户注册（短信验证码未完成）

> 游客注册，手机号不能重复
> 验证码来源于手机端的短信
> 注册成功则添加到用户账户表中

- 请求路径：api/register
- 请求方法：post

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: |:-:|
|user_password|string|非空|密码|
|user_phone|string|非空|手机号|
|verification_code|string|非空|短信验证码|

- 响应数据
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: |:-:|
|status|int|状态码|标明注册是否成功|
|user_id|int|自动生成|用户id|

### 2.2.个人信息相关

#### 2.2.1.获取个人公共信息(完成)

> 浏览自己或他人主页，根据用户名获取相关公共信息用于展示
> 请求参数或许改为user_name更好，因为并不知道他人id

- 请求路径：api/userInfo
- 请求方法：get

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: |:-:|
|user_name|string|非空|用户名|

- 响应数据
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: |:-:|
|user_id|int|非空|用户id|
|user_name|string|无|用户名|
|user_profile|string|无|用户头像相对地址|
|user_email|string|无|用户邮箱|
|user_phone|string|无|用户电话|
|user_createtime|date|年月日的int形式|用户注册日期|
|user_birthday|date|年月日的int形式|用户生日|
|user_gender|char|无|用户性别|
|user_state|bool|无|用户封禁状态|
|user_signature|string|无|用户个性签名|
|user_follower|int|无|用户粉丝数|
|user_follows|int|无|用户关注数|
|user_level|int|无|用户等级|

#### 2.2.2.获取个人私有信息

> 根据自己的用户id（登录状态时存）获取私有信息

- 请求路径：api/userInfo/private
- 请求方法：get

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_password|string|无|用户密码|
|user_coin|int|无|用户硬币数|

#### 2.2.3.编辑非重要个人信息

> 在个人主页编辑相关信息，此处待修改

- 请求路径：api/userEdit/normal
- 请求方法：post

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空，必须在登录态|用户id|
|edit_attribution|string|非空|要修改的属性，不能为机密信息|
|new_value|???|非空|新的数据|

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|statusCode|int|状态码|标明编辑是否成功|

#### 2.2.4.编辑机密信息

> 编辑个人机密信息，可以只选择修改其中一种，验证码必须有
> 若修改手机号，则验证码发送给新的手机

- 请求路径：api/userEdit/secret
- 请求方法：post

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|
|user_password|string|可空|用户密码|
|user_email|string|可空|用户邮箱|
|user_phone|string|可空|用户手机号|
|verification_code|string|非空|验证码|

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|statusCode|int|状态码|标明编辑是否成功|

#### 2.2.5.领取鸟币

> 签到，领取鸟币

- 请求路径：api/user/coin
- 请求方法：post

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|statusCode|int|状态码|标明领取是否成功|
|last_time|date|年月日的int形式|上次签到时间|

#### 2.2.6.获取收藏的问题

> 需包含问题id，以便在点击具体问题时直接查询
> 假设共收藏了n个问题

- 请求路径：api/userStar/question
- 请求方法：get

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_list|json|所有收藏的问题|问题列表|
|question[0]|Question|非空|具体第1个问题|
|...|若所有回答个数不足n，则可空|||
|question[n-1]|Question|可空|具体第n个问题|
|Question.id|int|非空|问题id|
|Question.title|string|非空|问题标题|

#### 2.2.7.获取收藏的回答

> 需包含回答id和对应的问题id，以便在点击具体回答时直接查询
> 假设共收藏了n个回答

- 请求路径：api/userStar/answer
- 请求方法：get

- 请求参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|answer_list|json|所有收藏的回答|回答列表|
|answer[0]|Answer|非空|具体第1个回答|
|...||||
|answer[n-1]|Answer|可空|具体第n个问题|
|Answer.id|int|非空|回答id|
|Answer.question_id|int|非空|回答对应的问题id|
|Answer.content|string|非空|回答内容|

#### 2.2.8.获取收藏的动态

> 假设共收藏了n个动态

- 请求路径：api/userStar/blog
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|user_id|int|非空|用户id|
- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|blog_list|json|所有收藏的动态|动态列表|
|blog[0]|Blog|非空|具体第1个动态|
|...||||
|blog[n-1]|Blog|非空|具体第n个动态|
|Blog.id||||

#### 2.2.9上传头像/校徽（完成）

> 通过用户ID上传头像

- 请求路径：api/login/image
- 请求方法：post

- 请求参数：

|        参数名         | 参数类型 | 备注 |    说明     |
| :-------------------: | :------: | :--: | :---------: |
| user_id/university_id |   int    | 非空 | 用户/高校id |

- 响应参数

|  参数名  | 参数类型 | 备注 |     说明     |
| :------: | :------: | :--: | :----------: |
|  status  |   bool   | 非空 | 操作是否成功 |
| imageurl |  string  | 非空 |  头像的url   |

#### 2.2.10.修改密码（未测试）

> 通过用户ID和用户密码将用户密码更换为新密码

- 请求路径：api/userinfo/password
- 请求方法：put

- 请求参数：

|    参数名     | 参数类型 | 备注 |    说明    |
| :-----------: | :------: | :--: | :--------: |
|    user_id    |   int    | 非空 |   用户id   |
| user_password |  string  | 非空 | 用户旧密码 |
| new_password  |  string  | 非空 | 用户新密码 |

- 响应参数

| 参数名 | 参数类型 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

#### 2.2.11.找回用户ID（未测试）

> 通过用户电话和用户密码找回用户ID

- 请求路径：api/userinfo/id
- 请求方法：put

- 请求参数：

|    参数名     | 参数类型 | 备注 |     说明     |
| :-----------: | :------: | :--: | :----------: |
|  user_phone   |  string  | 非空 | 用户电话号码 |
| user_password |  string  | 非空 |   用户密码   |

- 响应参数

| 参数名  | 参数类型 | 备注 |     说明     |
| :-----: | :------: | :--: | :----------: |
| status  |   bool   | 非空 | 操作是否成功 |
| user_id |   int    | 非空 |    用户ID    |

-----

-----

-----

-----

-----

-----

-----


### 2.2.首页相关

> 获取首页展示的问答、动态、快讯等板块的数据，API分开设置

#### 2.2.1.获取问题

> 获取固定数量的问题，假设为n
> 关于Question.visitble，返回的问题一定是可见的

- 请求路径：api/home/question
- 请求方法：get

- 请求参数：无

- 响应参数
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_list|json|问题个数<=n|问题列表|
|question[0]|Question|非空|具体第1个问题|
|...|若所有问题个数不足n，则可空|||
|question[n-1]|Question|可空|具体第n个问题|
|Question.id|int|非空|问题id|
|Question.date|年月日形式的int|非空|问题发布时间|
|Question.title|string|非空|问题标题|
|Question.description|string|非空|问题描述|
|Question.image|string|非空|问题图片路径|

#### 2.2.2.获取回答

> 根据已经的问题id，获得问题的一些回答，假设为n，若未被回答则状态码为300
> 这个地方需要些什么数据呢？

- 请求路径：api/home/answer
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_id|int|非空|问题id|

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|answer_list|json|回答个数<=n|回答列表|
|answer[0]|Answer|非空|具体第1个回答|
|...|若所有回答个数不足n，则可空|||
|answer[n-1]|Answer|可空|具体第n个问题|
|Answer.id|int|非空|回答id|
|Answer.content|string|非空|回答内容|

#### 2.2.3.获取留学快讯

> 获取固定数量的问题，假设为n
> 关于NewsFlash.visitble，返回的快讯一定是可见的

- 请求路径：api/home/newsflash
- 请求方法：get

- 请求参数：无

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|newsflash_list|json|快讯个数<=n|快讯列表|
|newsflash[0]|NewsFlash|非空|具体第1个快讯|
|...|若所有回答个数不足n，则可空|||
|newsflash[n-1]|NewsFlash|可空|具体第n个快讯|
|NewsFlash.id|int|非空|快讯id|
|NewsFlash.tag|string|非空|快讯标签|
|NewsFlash.date|date|年月日形式的int|快讯发布时间|
|NewsFlash.region|string|非空|快讯地区|
|NewsFlash.content|string|非空|快讯内容|

#### 2.2.4.获取网站信息

> 在主页最底下网站的相关信息
> 这块儿没有也行......

- 请求路径：api/home/about
- 请求方法：get

- 请求参数：无

- 响应参数
|参数名|参数类型|说明|
| :-: | :-: | :-: |
|...|||

### 2.3.问答板块

> 根据一定法则获取问答板块最火的若干个问题的相关信息
> 传入参数分为页码page和每页问题数量m，从而实现翻页功能
> 包含：
> 1. 问题陈列页面：每页具体数量的问题，问题的简要信息
> 2. 具体问题界面：具体的某个问题，该问题的回答的简要信息
> 3. 具体回答界面：具体的某个回答，以及该回答的固定数量的评论，支持翻页

#### 2.3.1.获取若干问题

> 根据排序（热度+时间）获取现有的问题，分页进行传输
> 事先确定每页的问题数目，假设为m，则API参数为第page页
> 若page超过最大页数maxPage，则errorCode=300并返回最后一页内容
> 这地方获取的信息或许有点多？打\*的可能不需要。与2.3.2有所重复

- 请求路径：api/questions
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|page|int|非空|页数|
|m|int|非空，定值|每页问题数目|

- 响应参数
|参数名|参数说明|备注|说明|
| :-: | :-: | :-: | :-: |
|question_list|json|问题个数<=m|问题列表|
|question[0]|Question|非空|具体的第1个问题|
|...|若所有问题个数不足m，则可空|只在最后一页可能<m||
|question[m-1]|Question|可空|具体的第m个问题|
|Question.id|int|非空|问题id|
|Question.date|年月日形式的int|非空|问题提出时间|
|Question.title|string|非空|问题标题|
|\*Question.description|string|非空|问题描述|
|\*Question.user_info|json|非空|提问者信息|
|\*Question.ans_num|int|非空|问题的回答个数|

#### 2.3.2.获取具体某一问题

> 获取具体一个问题的相关信息，2.3.1有过的这里是否需要重复？

- 请求路径：api/question/:question_id
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_id|int|非空|问题id|

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_user_id|int|提问者|
|question_description|string|非空|问题描述|
|question_user_info|json|非空|提问者信息|
|question_reward|int|默认为零|问题悬赏|
|question_image|string|无|问题图片路径|


#### 2.3.3.获取某个问题的若干回答

> 属于“具体问题界面”，page表示页码，m表示每页数量

- 请求路径：api/answers/:question_id
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_id|int|非空|问题id|
|page|int|非空|页码|
|m|int|非空，定值|每页回答数目|

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|answer_list|json|回答个数<=m|回答列表|
|answer[0]|Answer|非空|具体的被采纳的回答|
|answer[1]|Answer|可空|具体的第2个回答|
|...|若所有问题个数不足m，则可空|只在最后一页可能<m||
|answer[m-1]|Answer|可空|具体的第m个问题|
|Answer.id|int|非空|问题id|
|Answer.user_id|int|非空|回答用户id|
|Answer.content|string|非空|回答内容|
|Answer.like|int|非空|回答被点赞数|
|Answer.coin|int|非空|回答被投币数|

#### 2.3.4.获取某个问题的具体回答

> 2.3.3获得回答的各个id，用id来获取具体的回答
> 2.3.3已获取的这里是否还需要？似乎2.3.3已经大概获取了所有信息

- 请求路径：api/answer/:answer_id
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|answer_id|int|非空|回答id|

- 响应参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|answer_image|string|无|回答图片路径|

#### 2.3.5.获取某个回答的评论

> 在具体回答页面底下，展示固定数量的评论，page是页码，m是每页数量

- 请求路径：api/answers/:question_id
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|question_id|int|非空|问题id|
|page|int|非空|页码|
|m|int|非空，定值|每页回答数目|

- 响应参数

  这个地方如何返回评论的评论效果，在json中得以体现？

#### 2.3.6.提问（未测试）

> 用户输入问题的标题、内容、tag、悬赏金额并提出问题

- 请求路径：api/question
- 请求方法：post
- 请求参数：

|        参数名        | 参数类型 | 备注 |    说明    |
| :------------------: | :------: | :--: | :--------: |
|   question_user_id   |   int    | 非空 |  提问者id  |
|     question_tag     |  string  |  无  |  问题标签  |
|    question_title    |  string  | 非空 | 问题的标题 |
| question_description |  string  | 非空 |  问题描述  |
|   question_reward    | decimal  |  无  |  问题悬赏  |

- 响应参数：

|   参数名    | 参数类型 | 备注 |  说明  |
| :---------: | :------: | :--: | :----: |
| question_id |   int    | 非空 | 问题id |

### 2.4.留学快讯

#### 2.4.1.获取若干留学快讯

> 根据排序（热度+时间）获取现有的快讯，分页进行传输
> 事先确定每页的快讯数目，假设为m，则API参数为第page页
> 若page超过最大页数maxPage，则errorCode=300并返回最后一页内容

- 请求路径：api/newsflashs
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|page|int|非空|页数|
|m|int|非空，定值|每页快讯数目|

- 响应参数：
|参数名|参数说明|备注|说明|
| :-: | :-: | :-: | :-: |
|newsflash_list|json|快讯个数<=m|快讯列表|
|newsflash[0]|NewsFlash|非空|具体的第1个快讯|
|...|若所有快讯个数不足m，则可空|只在最后一页可能<m||
|newsflash[m-1]|NewsFlash|可空|具体的第m个快讯|
|NewsFlash.id|int|非空|快讯id|
|NewsFlash.date|年月日形式的int|非空|快讯提出时间|
|NewsFlash.title|string|非空|快讯标题|

#### 2.4.2.获取某个快讯具体信息

> 根据快讯id获取其具体信息用于展示

- 请求路径：api/newsflash/:newsflash_id
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|news_id|int|非空|快讯id|

- 响应参数：
|参数名|参数说明|备注|说明|
| :-: | :-: | :-: | :-: |
|news_content|string|非空|快讯内容|
|news_date|年月日形式的int|非空|快讯发布时间|
|news_title|string|非空|快讯标题|

### 2.5.动态分享

#### 2.5.1.获取动态

> 根据排序（热度+时间）获取现有的动态，分页进行传输
> 事先确定每页的动态数目，假设为m，则API参数为第page页
> 若page超过最大页数maxPage，则errorCode=300并返回最后一页内容

- 请求路径：api/blog
- 请求方法：get

- 请求参数：
|参数名|参数类型|备注|说明|
| :-: | :-: | :-: | :-: |
|page|int|非空|页数|
|m|int|非空，定值|每页快讯数目|
|tag|int|非空|排序方式（0热度，1时间）|

- 响应参数：

|      参数名      | 参数说明 | 备注 |     说明     |
| :--------------: | :------: | :--: | :----------: |
|     blog_id      |   int    | 非空 |    动态ID    |
|   blog_user_id   |   int    | 非空 | 动态发布者ID |
|   blog_content   |  string  | 非空 |   动态内容   |
|  blog_like_num   |   int    | 非空 |  动态点赞数  |
| blog_comment_num |   int    | 非空 |  动态评论数  |

#### 2.5.2.展示具体动态

> 展示具体的一条动态内容，以及相关评论

- 请求路径：api/blog/content
- 请求方法：get

- 请求参数：

| 参数名  | 参数类型 | 备注 |  说明  |
| :-----: | :------: | :--: | :----: |
| blog_id |   int    | 非空 | 动态ID |

- 响应参数：

|        参数名        | 参数说明 | 备注 |      说明      |
| :------------------: | :------: | :--: | :------------: |
|       blog_id        |   int    | 非空 |     动态ID     |
|     blog_user_id     |   int    | 非空 |  动态发布者ID  |
|     blog_content     |  string  | 非空 |    动态内容    |
|    blog_like_num     |   int    | 非空 |   动态点赞数   |
|   blog_comment_num   |   int    | 非空 |   动态评论数   |
|   blog_comment_id    |   int    | 非空 |   动态评论ID   |
| blog_comment_user_id |   int    | 非空 |  动态评论者ID  |
| blog_comment_content |  string  | 非空 |  动态评论内容  |
|  blog_comment_time   |   date   | 非空 |  动态评论时间  |
|  blog_comment_reply  |   int    | 非空 | 动态评论回复ID |

### 2.6.大学信息

#### 2.6.1.展示学校列表

> 根据国家or排行榜or年份返回高校信息
> 事先确定每页的动态数目，假设为m，则API参数为第page页
> 若page超过最大页数maxPage，则errorCode=300并返回最后一页内容

- 请求路径：api/university
- 请求方法：get

- 请求参数：

|  参数名  | 参数类型 |    备注    |               说明                |
| :------: | :------: | :--------: | :-------------------------------: |
|   page   |   int    |    非空    |               页数                |
|    m     |   int    | 非空，定值 |           每页学校数目            |
|   tag    |   int    |    非空    | 查找方式（0国家，1排行榜，2年份） |
| tag_para |  string  |    非空    |             查找内容              |

- 响应参数：

|         参数名          | 参数说明 | 备注 |      说明      |
| :---------------------: | :------: | :--: | :------------: |
|      university_id      |   int    | 非空 |     高校ID     |
|     university_name     |  stirng  | 非空 |    高校名称    |
|    university_badge     |  string  | 非空 |    高校校徽    |
| university_abbreviation |  string  | 非空 |    高校简称    |
| university_introduction |  string  | 非空 |    高校简介    |
|         ranking         |   int    | 可空 | 当前排行榜排名 |

#### 2.6.2.高校详细信息（未测试）

> 根据高校ID返回高校详细信息

- 请求路径：api/university
- 请求方法：get

- 请求参数：

|    参数名     | 参数类型 | 备注 |  说明  |
| :-----------: | :------: | :--: | :----: |
| university_id |   int    | 非空 | 高校ID |

- 响应参数：

|         参数名          | 参数说明 | 备注 |       说明       |
| :---------------------: | :------: | :--: | :--------------: |
|      university_id      |   int    | 非空 |      高校ID      |
|     university_name     |  stirng  | 非空 |     高校名称     |
| university_abbreviation |  string  | 非空 |     高校简称     |
| university_introduction |  string  | 非空 |     高校简介     |
|    university_region    |  string  | 非空 |   高校所属地区   |
|   university_country    |  string  | 非空 |    高校所在国    |
|   university_location   |  string  | 非空 |     高校地址     |
|    university_email     |  string  | 非空 |     高校邮箱     |
| university_student_num  |   int    | 非空 |  高校在校学生数  |
|   university_website    |  string  | 非空 |     高校官网     |
|   university_college    |  string  | 非空 |     高校院系     |
|   university_QS_rank    |   int    | 非空 |    高校QS排名    |
|   university_THE_rank   |   int    | 非空 |  高校泰晤士排名  |
| university_USNews_rank  |   int    | 非空 | 高校U.S.News排名 |

#### 2.6.3关注高校（完成）

> 根据关注者、高校ID完成关注操作

- 请求路径：api/follow/university
- 请求方法：post

- 请求参数：

|    参数名     | 参数类型 | 备注 |   说明   |
| :-----------: | :------: | :--: | :------: |
|    user_id    |   int    | 非空 | 关注者ID |
| university_id |   int    | 非空 |  高校ID  |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

#### 2.6.4取关高校（完成）

> 根据取关者、高校ID完成取关操作

- 请求路径：api/follow/university
- 请求方法：put

- 请求参数：

|    参数名     | 参数类型 | 备注 |   说明   |
| :-----------: | :------: | :--: | :------: |
|    user_id    |   int    | 非空 | 取关者ID |
| university_id |   int    | 非空 |  高校ID  |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

#### 2.6.5添加高校信息（未测试）

> 添加高校信息

- 请求路径：api/university
- 请求方法：post

- 请求参数：

|         参数名          | 参数说明 | 备注 |       说明       |
| :---------------------: | :------: | :--: | :--------------: |
|     university_name     |  stirng  | 非空 |     高校名称     |
| university_abbreviation |  string  | 非空 |     高校简称     |
| university_introduction |  string  | 非空 |     高校简介     |
|    university_region    |  string  | 非空 |   高校所属地区   |
|   university_country    |  string  | 非空 |    高校所在国    |
|   university_location   |  string  | 非空 |     高校地址     |
|    university_email     |  string  | 非空 |     高校邮箱     |
| university_student_num  |   int    | 非空 |  高校在校学生数  |
|   university_website    |  string  | 非空 |     高校官网     |
|   university_college    |  string  | 非空 |     高校院系     |
|   university_QS_rank    |   int    | 非空 |    高校QS排名    |
|   university_THE_rank   |   int    | 非空 |  高校泰晤士排名  |
| university_USNews_rank  |   int    | 非空 | 高校U.S.News排名 |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

### 2.7.机构

#### 2.7.1.展示机构信息

> 根据机构ID返回机构详细信息

- 请求路径：api/institution/{institution_id}
- 请求方法：get

- 请求参数：

|     参数名     | 参数类型 | 备注 |  说明  |
| :------------: | :------: | :--: | :----: |
| institution_id |   int    | 非空 | 机构ID |

- 响应参数：

|               参数名               | 参数说明 | 备注 |     说明     |
| :--------------------------------: | :------: | :--: | :----------: |
|           institution_id           |   int    | 非空 |    机构ID    |
|          institution_name          |  stirng  | 非空 |   机构名称   |
|         institution_phone          |  string  | 非空 |   机构电话   |
|        institution_qualify         |  string  | 非空 | 机构营业执照 |
|      institution_introduction      |  string  | 非空 |   机构简介   |
|        institution_profile         |  string  | 非空 |   机构头像   |
|          institution_city          |  string  | 非空 | 机构所在城市 |
|        institution_location        |  string  | 非空 |   机构地址   |
|         institution_email          |  string  | 非空 |   机构邮箱   |
| institution_lessons_characteristic |  string  | 非空 | 机构课程特色 |
|        institution_lessons         |  string  | 非空 |   机构课程   |
|       institution_createtime       |   date   | 非空 | 机构创立时间 |

### 2.8.管理员相关（TODO）

#### 2.8.1.展示管理员信息

> 根据管理员ID返回管理员详细信息

- 请求路径：api/administrator/{administrator_id}
- 请求方法：get

- 请求参数：

|      参数名      | 参数类型 | 备注 |   说明   |
| :--------------: | :------: | :--: | :------: |
| administrator_id |   int    | 非空 | 管理员ID |

- 响应参数：

|          参数名          | 参数说明 | 备注 |        说明        |
| :----------------------: | :------: | :--: | :----------------: |
|          status          |   bool   | 非空 |    操作是否成功    |
|     administrator_id     |   int    | 非空 |      管理员ID      |
|    administrator_name    |  stirng  | 非空 |     管理员名称     |
|   administrator_phone    |  string  | 非空 |     管理员电话     |
|  administrator_password  |  string  | 非空 |     管理员密码     |
|   administrator_gender   |  string  | 非空 |     管理员性别     |
|  administrator_profile   |  string  | 非空 |     管理员头像     |
|   administrator_email    |  string  | 非空 |     管理员邮箱     |
| administrator_createtime |   date   | 非空 | 管理员账号注册时间 |

#### 2.8.2.修改管理员信息

> 修改管理员详细信息

- 请求路径：api/administrator/{administrator_id}
- 请求方法：put

- 请求参数：

|      参数名      | 参数类型 | 备注 |   说明   |
| :--------------: | :------: | :--: | :------: |
| administrator_id |   int    | 非空 | 管理员ID |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

#### 2.8.3.展示未审核认证申请

> 根据管理员ID返回管理员详细信息

- 请求路径：api/administrator/{administrator_id}/qualification/{identity_id}
- 请求方法：get

- 请求参数：

|      参数名      | 参数类型 | 备注 |   说明   |
| :--------------: | :------: | :--: | :------: |
| administrator_id |   int    | 非空 | 管理员ID |
|   identity_id    |   int    | 非空 |  认证ID  |

- 响应参数：

|            参数名            | 参数说明 | 备注 |     说明     |
| :--------------------------: | :------: | :--: | :----------: |
|         identity_id          |   int    | 非空 |    认证ID    |
|           user_id            |   int    | 非空 |    用户ID    |
|        university_id         |   int    | 非空 |    高校ID    |
|           identity           |  string  | 非空 |     学历     |
| identity_qualification_image |  string  | 非空 | 学历认证证明 |

#### 2.8.4.展示已审核认证申请

> 根据管理员ID返回管理员详细信息

- 请求路径：api/administrator/{administrator_id}/qualification/{identity_id}/qualificationchecking
- 请求方法：get

- 请求参数：

|      参数名      | 参数类型 | 备注 |   说明   |
| :--------------: | :------: | :--: | :------: |
| administrator_id |   int    | 非空 | 管理员ID |
|   identity_id    |   int    | 非空 |  认证ID  |

- 响应参数：

|            参数名            | 参数说明 | 备注 |     说明     |
| :--------------------------: | :------: | :--: | :----------: |
|         identity_id          |   int    | 非空 |    认证ID    |
|           user_id            |   int    | 非空 |    用户ID    |
|        university_id         |   int    | 非空 |    高校ID    |
|           identity           |  string  | 非空 |     学历     |
| identity_qualification_image |  string  | 非空 | 学历认证证明 |
|       administrator_id       |   int    | 非空 |   管理员ID   |
|         summit_date          |   date   | 非空 |   提交时间   |
|         review_date          |   date   | 非空 |   审核时间   |
|        review_result         |  string  | 非空 |   审核结果   |
|        review_reason         |  string  | 非空 |   审核理由   |

#### 2.8.5.处理认证申请

> 根据管理员ID返回管理员详细信息

- 请求路径：api/administrator/{administrator_id}/qualificationchecking
- 请求方法：put

- 请求参数：

|      参数名      | 参数类型 | 备注 |   说明   |
| :--------------: | :------: | :--: | :------: |
| administrator_id |   int    | 非空 | 管理员ID |
|   identity_id    |   int    | 非空 |  认证ID  |
|  review_result   |  string  | 非空 | 审核结果 |
|  review_reason   |  string  | 非空 | 审核理由 |

- 响应参数：

|   参数名    | 参数说明 | 备注 |     说明     |
| :---------: | :------: | :--: | :----------: |
| identity_id |   int    | 非空 |    认证ID    |
|   user_id   |   int    | 非空 |    用户ID    |
|   status    |   bool   | 非空 | 操作是否成功 |

### 2.9.用户间交互

#### 2.9.1关注（完成）

> 根据关注者、被关注者ID完成关注操作

- 请求路径：api/follow
- 请求方法：post

- 请求参数：

|     参数名     | 参数类型 | 备注 |    说明    |
| :------------: | :------: | :--: | :--------: |
|    user_id     |   int    | 非空 |  关注者ID  |
| follow_user_id |   int    | 非空 | 被关注者ID |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |

#### 2.9.2取关（完成）

> 根据取关者、被取关者ID完成取关操作

- 请求路径：api/follow
- 请求方法：put

- 请求参数：

|     参数名     | 参数类型 | 备注 |    说明    |
| :------------: | :------: | :--: | :--------: |
|    user_id     |   int    | 非空 |  取关者ID  |
| follow_user_id |   int    | 非空 | 被取关者ID |

- 响应参数：

| 参数名 | 参数说明 | 备注 |     说明     |
| :----: | :------: | :--: | :----------: |
| status |   bool   | 非空 | 操作是否成功 |