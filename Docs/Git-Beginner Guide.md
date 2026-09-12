# Git Beginner Guide

本文档面向第一次使用 Git 和 GitHub 的项目成员。

不要求你提前掌握 Git。

按照本文档中的流程操作，即可完成：

```text
下载仓库
→ 创建自己的分支
→ 修改 Unity 项目
→ 保存 Git 版本
→ 上传 GitHub
→ 创建 Pull Request
→ 合并到 main
```

---

# 1. Git 和 GitHub 是什么？

首先区分两个概念。

## Git

Git 是安装在你电脑上的版本管理工具。

它负责记录：

```text
谁修改了什么
什么时候修改
每一个版本有什么变化
```

例如：

```text
版本 1
↓
增加 Player
↓
版本 2
↓
修复 Camera
↓
版本 3
```

这些版本首先保存在你的电脑上。

---

## GitHub

GitHub 是远程代码托管平台。

可以简单理解：

```text
Git
= 管理你电脑上的项目版本

GitHub
= 保存团队共享的远程 Git 仓库
```

因此：

```text
git commit
```

和：

```text
git push
```

不是一回事。

---

# 2. Git 最核心的工作流程

请先记住：

```text
工作区
  ↓
git add
  ↓
暂存区
  ↓
git commit
  ↓
本地 Git 仓库
  ↓
git push
  ↓
GitHub
```

分别代表：

### 修改文件

例如修改：

```text
Player.cs
```

现在变化还只是存在你的电脑文件中。

---

### git add

```bash
git add Player.cs
```

意思：

> 将 Player.cs 当前修改加入“下一次准备提交的内容”。

---

### git commit

```bash
git commit -m "Update player movement"
```

意思：

> 在本地 Git 仓库中保存一个正式版本记录。

注意：

```text
commit ≠ 上传 GitHub
```

Commit 完以后，即使电脑断网，这个版本仍然存在你的本地仓库。

---

### git push

```bash
git push
```

意思：

> 将本地已经 Commit 的内容上传到 GitHub。

---

# 3. 五个最重要的 Git 命令

对于新手，首先掌握：

```bash
git pull
git status
git add
git commit
git push
```

可以理解成：

```text
git pull
= 获取 GitHub 最新内容

git status
= 查看现在发生了什么

git add
= 选择这次准备提交什么

git commit
= 在本地保存一个版本

git push
= 上传已经保存的版本
```

---

# 4. 第一次使用前需要安装什么？

请准备：

```text
Git
Git LFS
Unity Hub
项目要求的 Unity 版本
```

检查 Git：

```bash
git --version
```

检查 Git LFS：

```bash
git lfs version
```

如果都能够返回版本号，说明已经安装。

---

# 5. 第一次配置 Git 身份

首次使用 Git 时：

```bash
git config --global user.name "你的GitHub用户名"
```

例如：

```bash
git config --global user.name "alice"
```

然后：

```bash
git config --global user.email "你的GitHub邮箱"
```

例如：

```bash
git config --global user.email "alice@example.com"
```

检查：

```bash
git config --global --list
```

---

# 6. 第一次下载仓库

GitHub 中打开仓库。

点击：

```text
Code
→ HTTPS
```

复制仓库地址。

例如：

```text
https://github.com/xxx/Unity-Projects.git
```

然后在准备保存项目的位置打开 PowerShell：

```bash
git clone https://github.com/xxx/Unity-Projects.git
```

这叫：

```text
Clone
```

意思是：

> 将完整 GitHub 仓库复制到你的电脑，并建立 Git 连接。

---

# 7. 进入仓库

例如：

```bash
cd Unity-Projects
```

查看：

```bash
git status
```

如果看到：

```text
On branch main
```

说明当前处于：

```text
main
```

主分支。

---

# 8. 初始化 Git LFS

第一次 Clone 后执行：

```bash
git lfs install
```

本仓库使用 Git LFS 保存部分大型 Unity 二进制资源。

通常只需要在电脑首次配置时执行一次。

---

# 9. 什么是 branch？

Branch 就是：

> 独立开发线。

主分支：

```text
main
```

负责保存稳定内容。

成员不应该日常直接修改 `main`。

正确流程：

```text
main
 │
 ├── alice/update-player
 │
 ├── bob/fix-camera
 │
 └── chris/add-ar-project
```

不同成员可以同时开发，而不直接破坏 `main`。

---

# 10. 每次开始工作前

先切回主分支：

```bash
git switch main
```

然后：

```bash
git pull
```

意思是：

> 从 GitHub 获取最新 main。

这是多人协作中非常重要的一步。

---

# 11. 创建自己的开发分支

从最新 main 创建：

```bash
git switch -c <用户名>/<任务名>
```

例如：

```bash
git switch -c alice/update-player
```

其中：

```text
git switch
= 切换分支

-c
= 创建一个新分支

alice/update-player
= 新分支名称
```

所以整个命令意思是：

> 创建 `alice/update-player` 分支并立即切换过去。

---

# 12. 查看当前分支

执行：

```bash
git branch
```

例如：

```text
* alice/update-player
  main
```

`*` 表示当前所在分支。

---

# 13. 本仓库 Unity 项目应该放在哪里？

统一放：

```text
Projects/<GitHub用户名>/<项目名>/
```

例如你的 GitHub 用户名：

```text
alice
```

项目：

```text
ShooterGame
```

那么：

```text
Projects/
└── alice/
    └── ShooterGame/
        ├── Assets/
        ├── Packages/
        └── ProjectSettings/
```

---

# 14. Unity 项目哪些内容需要提交？

需要：

```text
Assets/
Packages/
ProjectSettings/
```

以及 Unity `.meta` 文件。

例如：

```text
Player.cs
Player.cs.meta
```

两个都需要。

---

# 15. 哪些内容不要提交？

不要提交：

```text
Library/
Temp/
Logs/
obj/
UserSettings/
Build/
Builds/
```

仓库 `.gitignore` 已经配置自动忽略。

但是提交前仍然应该检查：

```bash
git status
```

---

# 16. 修改完成以后第一步做什么？

执行：

```bash
git status
```

例如可能看到：

```text
Changes not staged for commit:

modified:
    Projects/alice/ShooterGame/Assets/Player.cs
```

意思：

> Player.cs 被修改了，但现在还没有准备提交。

---

# 17. git status 是最重要的检查工具

建议：

```text
不知道 Git 当前状态？
→ git status

不知道文件有没有 add？
→ git status

不知道现在在哪个分支？
→ git status

不知道还有没有没提交文件？
→ git status
```

遇到问题时优先运行：

```bash
git status
```

---

# 18. git add 是什么意思？

假设修改的是：

```text
Projects/alice/ShooterGame/
```

执行：

```bash
git add Projects/alice/ShooterGame
```

意思：

> 将这个项目中的变化加入下一次 Commit。

---

# 19. 为什么不建议新手直接 git add .？

你可能经常看到：

```bash
git add .
```

`.` 表示：

> 当前目录及下面所有变化。

虽然方便，但多人仓库可能误提交：

```text
其他成员项目
临时文件
不相关文档
```

因此推荐：

```bash
git add Projects/alice/ShooterGame
```

明确指定自己的项目。

---

# 20. add 完之后再次检查

执行：

```bash
git status
```

如果看到：

```text
Changes to be committed:
```

说明这些内容已经进入暂存区。

例如：

```text
Changes to be committed:

modified:
    Projects/alice/ShooterGame/Assets/Player.cs
```

---

# 21. git commit 是什么意思？

执行：

```bash
git commit -m "Update player movement"
```

意思：

> 将已经 git add 的内容保存为一个本地 Git 版本。

其中：

```text
-m
```

表示后面直接填写 Commit Message。

---

# 22. Commit Message 写什么？

写：

> 这次实际完成了什么。

例如：

```text
Add ShooterGame project
Add player movement
Update inventory UI
Fix camera rotation
Remove unused assets
```

不要写：

```text
123
test
update
final
final2
aaaa
```

---

# 23. Commit 保存在哪里？

Commit 首先保存在：

```text
你的电脑
```

具体属于当前仓库：

```text
.git/
```

不会自动上传到 GitHub。

查看 Commit：

```bash
git log --oneline
```

例如：

```text
8ef231a Update player movement
5f31abc Add ShooterGame project
```

---

# 24. git push 是什么意思？

Commit 完成以后：

```bash
git push
```

表示：

> 把本地 Commit 上传到 GitHub。

但是第一次上传一个新分支，需要：

```bash
git push -u origin <分支名>
```

例如：

```bash
git push -u origin alice/update-player
```

---

# 25. origin 是什么？

执行：

```bash
git remote -v
```

你可能看到：

```text
origin https://github.com/xxx/Unity-Projects.git
```

`origin` 就是：

> 当前 GitHub 远程仓库的默认名称。

因此：

```bash
git push -u origin alice/update-player
```

意思是：

> 把我的 `alice/update-player` 分支上传到 GitHub 的 origin 仓库。

---

# 26. -u 是什么意思？

第一次：

```bash
git push -u origin alice/update-player
```

`-u` 会建立：

```text
本地 alice/update-player
        ↕
GitHub alice/update-player
```

之间的关联。

以后这个分支继续提交，只需要：

```bash
git push
```

---

# 27. 上传完成后为什么 main 没变？

这是正常的。

因为你上传的是：

```text
alice/update-player
```

而不是：

```text
main
```

GitHub 现在可能是：

```text
main
│
└── alice/update-player
```

下一步需要 Pull Request。

---

# 28. 什么是 Pull Request？

Pull Request，简称：

```text
PR
```

意思是：

> 请求把一个开发分支的修改合并到另一个分支。

例如：

```text
alice/update-player
        ↓
       main
```

---

# 29. 创建 Pull Request

Push 后进入 GitHub 仓库。

通常会看到：

```text
Compare & pull request
```

点击。

确认：

```text
base: main
```

以及：

```text
compare: alice/update-player
```

然后填写：

```text
标题
修改说明
```

最后：

```text
Create pull request
```

---

# 30. PR 通过之后

管理员或成员检查内容。

如果：

```text
代码正常
目录正常
自动检查通过
```

就可以：

```text
Merge
```

之后内容正式进入：

```text
main
```

---

# 31. PR 合并后下一次怎么开发？

首先：

```bash
git switch main
```

然后：

```bash
git pull
```

此时你本地 main 会取得刚才已经 Merge 的版本。

然后创建新的任务分支：

```bash
git switch -c alice/next-task
```

不要继续长期使用已经完成的旧分支。

---

# 32. 日常完整流程

每一次新任务：

```bash
git switch main
git pull
git switch -c <用户名>/<任务>
```

开发完成：

```bash
git status
```

添加指定项目：

```bash
git add Projects/<用户名>/<项目>
```

检查：

```bash
git status
```

Commit：

```bash
git commit -m "本次修改说明"
```

第一次上传：

```bash
git push -u origin <分支名>
```

然后：

```text
GitHub
→ Pull Request
→ Review
→ Merge
```

---

# 33. git pull 和 git push 的区别

```text
GitHub
  ↓
git pull
  ↓
你的电脑
```

而：

```text
你的电脑
  ↓
git push
  ↓
GitHub
```

简单记：

```text
pull = 拉下来

push = 推上去
```

---

# 34. 常见状态：Your branch is up to date

如果看到：

```text
Your branch is up to date with 'origin/main'.
```

意思：

> 你的本地分支和 GitHub 当前版本一致。

这是正常状态。

---

# 35. Changes not staged for commit

如果看到：

```text
Changes not staged for commit:

modified: README.md
```

意思：

> 文件已经修改，但还没有执行 git add。

下一步可能是：

```bash
git add README.md
```

---

# 36. Changes to be committed

如果看到：

```text
Changes to be committed:
```

意思：

> 文件已经 git add，可以准备 Commit。

下一步：

```bash
git commit -m "..."
```

---

# 37. nothing to commit

如果看到：

```text
nothing to commit, working tree clean
```

意思：

> 当前没有新的文件修改需要提交。

这通常是正常状态。

---

# 38. Push 被 main Ruleset 拒绝

如果看到类似：

```text
push declined due to repository rule violations
```

通常说明：

> main 不允许普通成员直接 Push。

这是仓库保护规则正常生效。

不要尝试强制 Push。

应该：

```bash
git switch -c <自己的分支>
```

然后：

```bash
git push -u origin <自己的分支>
```

最后创建 Pull Request。

---

# 39. 网络错误不代表 Commit 消失

例如：

```text
Failed to connect to github.com
Connection was reset
```

这是网络连接失败。

只要之前已经：

```bash
git commit
```

Commit 仍然保存在本地。

网络恢复以后再：

```bash
git push
```

即可。

---

# 40. 不要随便执行危险命令

新手不建议在不了解后果时执行：

```bash
git reset --hard
```

```bash
git clean -fd
```

```bash
git push --force
```

这些命令可能导致：

```text
本地修改丢失
未跟踪文件删除
远程历史被覆盖
```

如果遇到问题，首先：

```bash
git status
```

然后保留完整错误信息。

---

# 41. 不要删除 `.git`

仓库根目录存在隐藏目录：

```text
.git/
```

它保存 Git 仓库的：

```text
Commit 历史
Branch
配置
版本数据
```

不要手动删除。

也不要因为 Git 报错就重新：

```bash
git init
```

---

# 42. 不要在 Unity 子项目中 git init

正确：

```text
Unity-Projects/
├── .git/
└── Projects/
    └── alice/
        └── ShooterGame/
```

错误：

```text
Unity-Projects/
├── .git/
└── Projects/
    └── alice/
        └── ShooterGame/
            └── .git/
```

整个仓库只使用最外层 `.git`。

---

# 43. 如何打开 Unity 项目？

假设：

```text
Unity-Projects/
└── Projects/
    └── alice/
        └── ShooterGame/
            ├── Assets/
            ├── Packages/
            └── ProjectSettings/
```

Unity Hub 应打开：

```text
Projects/alice/ShooterGame/
```

不要打开：

```text
Unity-Projects/
```

因为最外层是 Git 仓库，不是 Unity Project。

---

# 44. 不同项目可以使用不同 Unity 版本

每个 Unity 项目拥有自己的：

```text
ProjectSettings/ProjectVersion.txt
```

因此：

```text
Projects/alice/GameA
```

可以使用一个 Unity 版本，

而：

```text
Projects/bob/GameB
```

可以使用另一个 Unity 版本。

打开项目时请使用项目对应 Unity 版本。

---

# 45. 大文件和 Git LFS

如果仓库使用：

```text
.psd
.psb
.blend
.fbx
.wav
.mp4
.mov
```

等大型文件，

仓库会通过：

```text
.gitattributes
```

配置 Git LFS。

首次使用：

```bash
git lfs install
```

正常 Clone 后 Git LFS 文件会自动处理。

---

# 46. 常用 Git 命令速查

| 命令                       | 含义             |
| ------------------------ | -------------- |
| `git status`             | 查看当前 Git 状态    |
| `git clone URL`          | 第一次下载仓库        |
| `git pull`               | 获取远程最新提交       |
| `git switch main`        | 切换到 main       |
| `git switch -c xxx`      | 创建并切换新分支       |
| `git branch`             | 查看本地分支         |
| `git add <path>`         | 将指定修改加入暂存区     |
| `git commit -m "..."`    | 创建本地 Commit    |
| `git push`               | 上传当前分支         |
| `git push -u origin xxx` | 第一次上传新分支       |
| `git log --oneline`      | 查看 Commit 历史   |
| `git remote -v`          | 查看远程 GitHub 地址 |
| `git lfs install`        | 初始化 Git LFS    |

---

# 47. 新手最推荐记住的流程

如果暂时记不住所有 Git 命令，只记这一套：

```bash
git switch main
git pull
git switch -c <自己的分支>
```

完成开发后：

```bash
git status
git add <自己的项目目录>
git status
git commit -m "说明修改内容"
git push -u origin <自己的分支>
```

然后：

```text
GitHub
↓
Create Pull Request
↓
等待检查 / Review
↓
Merge
```

可以简化理解成：

```text
先更新 main
↓
创建自己的分支
↓
修改
↓
status
↓
add
↓
commit
↓
push
↓
PR
↓
main
```

---

# 48. 遇到问题时应提供什么信息？

如果需要管理员帮助，请提供：

第一项：

```bash
git status
```

完整输出。

第二项：

```bash
git branch
```

完整输出。

第三项：

错误命令和完整错误信息。

例如：

```text
执行：
git push

错误：
fatal: ...
```

不要只说：

```text
Git 不能用了
```

完整信息能显著加快排查。

---

# 49. 最终原则

对于新手，Git 使用过程中最重要的不是记住大量命令，而是保持以下习惯：

```text
开始工作前先 git pull

不要直接修改 main

一个任务创建一个 branch

提交前先 git status

git add 时只添加需要提交的内容

Commit Message 写清楚修改内容

Push 后通过 Pull Request 合并

不理解危险命令时不要执行
```

只要遵守这些规则，即使暂时不了解 Git 的高级功能，也可以安全参与本仓库的多人 Unity 开发。
