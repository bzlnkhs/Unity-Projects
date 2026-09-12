# Unity-Projects
Shared repository for multiple Unity projects
# Unity Projects Repository

本仓库用于集中管理团队成员各自开发的 Unity 项目。

仓库采用多项目 Monorepo 结构。每位成员在 `Projects/` 目录下使用自己的 GitHub 用户名建立个人目录，并在个人目录中存放一个或多个独立 Unity 项目。

`main` 为主分支，用于保存经过确认后的稳定版本。日常开发请勿直接在 `main` 上进行修改，应通过独立 Branch 开发，并使用 Pull Request 合并到 `main`。

---
下述内容为docu文档内[Git 新手使用指南](Docs/Git-Beginner-Guide.md)与[项目贡献与提交规范](CONTRIBUTING.md)的部分简述可直接转到该文档内查看完整版
若你有一定git使用基础可以直接看下述的库编辑规则
## Repository Structure

仓库基础结构如下：

```text
Unity-Projects/
│
├── Projects/
│   ├── <用户名>/
│   │   ├── <Unity项目A>/
│   │   │   ├── Assets/
│   │   │   ├── Packages/
│   │   │   └── ProjectSettings/
│   │   │
│   │   └── <Unity项目B>/
│   │       ├── Assets/
│   │       ├── Packages/
│   │       └── ProjectSettings/
│   │
│   └── ...
│
├── SharedPackages/
│
├── Docs/
│
├── .github/
│
├── .gitignore
├── .gitattributes
├── CONTRIBUTING.md
└── README.md
```

---

## Project Directory Rules

所有成员的 Unity 项目必须按照以下格式存放：

```text
Projects/<用户名>/<项目名称>/
```

例如：

```text
Projects/alice/ShooterGame/
Projects/alice/VRDemo/
Projects/bob/Platformer/
Projects/chris/ARPrototype/
```

请勿直接在 `Projects/` 根目录中创建 Unity 项目。

错误示例：

```text
Projects/ShooterGame/
Projects/Test/
Projects/MyProject/
```

正确示例：

```text
Projects/alice/ShooterGame/
Projects/bob/TestProject/
```

---

## Unity Project Requirements

每一个提交到本仓库的 Unity 项目至少应包含：

```text
<ProjectName>/
├── Assets/
├── Packages/
└── ProjectSettings/
```

其中建议确保以下 Unity 项目文件正常提交：

```text
Packages/manifest.json
Packages/packages-lock.json
ProjectSettings/ProjectVersion.txt
```

Unity 的 `.meta` 文件属于项目资源引用体系的一部分，必须提交到 Git。

例如：

```text
Assets/
├── Player.cs
├── Player.cs.meta
├── Player.prefab
├── Player.prefab.meta
├── Player.png
└── Player.png.meta
```

请勿删除或忽略 `.meta` 文件。

---

## Files That Must Not Be Uploaded

Unity 会自动生成大量缓存和本地文件，这些内容不应该提交到 GitHub，仓库存储有限需清理缓存文件后再上传。

包括但不限于：

```text
Library/
Temp/
Logs/
obj/
UserSettings/
Build/
Builds/
```

仓库根目录的 `.gitignore` 已配置用于自动忽略这些目录。

提交前仍应使用：

```bash
git status
```

检查即将提交的文件。

如果发现 `Library/`、`Temp/`、`Logs/` 等目录出现在提交列表中，请停止提交并检查 `.gitignore` 配置。

---

## Git LFS

本仓库使用 Git LFS 管理部分大型二进制资源。

所有成员首次使用仓库前应安装 Git LFS，并执行：

```bash
git lfs install
```

目前建议通过 Git LFS 管理的文件类型包括：

```text
.psd
.psb
.blend
.fbx
.wav
.mp4
.mov
```
.png .jpg等图片类型文件大小超过5mb的建议以压缩包形式标注存放位置后使用网盘或qq直接传输，暂不考虑存放于github
Git LFS 的具体配置统一保存在仓库根目录：

```text
.gitattributes
```

请勿自行删除或覆盖该文件。

---

## Branch Workflow

`main` 是本仓库的主分支。

原则上所有开发工作都应通过独立 Branch 完成。

开始开发前：

```bash
git switch main
git pull
```

！！！！！！创建自己的开发分支：（重要！！！！！建议你在置入自己项目的时候单独创建一个分支，这样可以防止上传库时覆盖别人的上传，并且可以形成一份自己分支的日志）

```bash
git switch -c <用户名>/<修改内容>
```

例如：

```bash
git switch -c alice/add-shooter-project
```

或者：

```bash
git switch -c bob/update-platformer-ui
```

完成修改后：

```bash
git status
```

确认修改范围。

然后只添加自己需要提交的项目：

```bash
git add Projects/<用户名>/<项目名>
```

例如：

```bash
git add Projects/alice/ShooterGame
```

创建 Commit：

```bash
git commit -m "Add Alice ShooterGame project"
```

第一次上传该分支：

```bash
git push -u origin alice/add-shooter-project
```

之后在 GitHub 创建 Pull Request：

```text
开发分支
    ↓
Pull Request
    ↓
main
```

确认无误后再 Merge。

---

## Commit Message Guidelines

Commit Message 应简短说明本次修改实际完成了什么。

推荐使用：

```text
Add ...
Update ...
Fix ...
Remove ...
Configure ...
Initialize ...
```

例如：

```text
Add ShooterGame project
Update player movement
Fix camera rotation
Remove unused assets
Configure Git LFS
Update repository documentation
```

请避免：

```text
update
test
123
final
final2
修改
临时
```

一个 Commit 尽量只完成一类明确修改。

---

## Pull Request Rules

提交 Pull Request 前，请确认：

* 项目位于 `Projects/<GitHub用户名>/<项目名>/`
* Unity 项目包含 `Assets/`
* Unity 项目包含 `Packages/`
* Unity 项目包含 `ProjectSettings/`
* `.meta` 文件已经提交
* 没有提交 `Library/`
* 没有提交 `Temp/`
* 没有提交 `Logs/`
* 没有提交 `UserSettings/`
* 没有提交本地 Build 文件
* 没有误修改其他成员的项目
* Commit Message 能够说明本次修改内容

如仓库已启用自动检查，则 Pull Request 必须通过相应检查后才能合并。

---

## Working With Other Members' Projects

原则上，每位成员主要负责：

```text
Projects/<自己的GitHub用户名>/
```

不要在没有沟通的情况下直接修改其他成员的项目。

如果确实需要协作，请通过：

```text
Branch
+
Pull Request
+
Review
```

完成修改。

目录代表项目归属，Branch 代表一次具体的开发任务。

例如：

```text
Projects/alice/
```

代表 Alice 负责的项目目录。

而：

```text
alice/update-player-controller
```

只是一次临时开发分支。

---

## Opening a Unity Project

Clone 仓库后，不要直接把整个 Git 仓库作为 Unity Project 打开。

例如仓库：

```text
Unity-Projects/
└── Projects/
    └── alice/
        └── ShooterGame/
            ├── Assets/
            ├── Packages/
            └── ProjectSettings/
```

应在 Unity Hub 中打开：

```text
Projects/alice/ShooterGame/
```

而不是：

```text
Unity-Projects/
```

每个子项目都是一个独立 Unity Project，可以拥有不同的 Unity 版本、Packages 和 ProjectSettings。

---

## First-Time Setup

第一次参与项目的成员建议执行：

```bash
git clone <repository-url>
```

进入仓库：

```bash
cd Unity-Projects
```

初始化 Git LFS：

```bash
git lfs install
```

同步主分支：

```bash
git switch main
git pull
```

之后创建个人开发分支：

```bash
git switch -c <用户名>/<任务名>
```

---

## Daily Workflow

推荐的日常流程：

```text
切换到 main
      ↓
git pull
      ↓
创建新的开发分支
      ↓
修改自己的 Unity 项目
      ↓
git status
      ↓
git add 指定项目目录
      ↓
git status
      ↓
git commit
      ↓
git push
      ↓
创建 Pull Request
      ↓
Review / 自动检查
      ↓
Merge 到 main
```

常用命令：

```bash
git switch main
git pull
git switch -c <branch-name>

git status
git add <path>
git commit -m "Commit message"

git push -u origin <branch-name>
```

---

## Important Notes

1. 不要在子项目目录中再次执行 `git init`。

本仓库只能存在一个 Git Repository：

```text
Unity-Projects/.git/
```

禁止出现：

```text
Unity-Projects/
└── Projects/
    └── alice/
        └── ShooterGame/
            └── .git/
```

2. 不要提交账号密码、API Key、Token、服务器密钥、证书或其他敏感信息。

3. 不要直接提交大型 Build 文件，例如：

```text
.exe
.apk
Game_Data/
大型压缩包
```

4. 提交前务必执行：

```bash
git status
```

确认实际提交内容。

5. `main` 应始终保持为经过确认的稳定版本。

---

## Additional Documentation

更详细的成员贡献规范请查看：

```text
CONTRIBUTING.md
```

其他项目说明与开发文档统一存放于：

```text
Docs/
```

公共 Unity Package 或未来需要跨项目共享的模块统一规划于：

```text
SharedPackages/
```

请勿直接将公共资源随意复制到多个项目中；共享模块的具体管理方式将在需要时统一定义。

---

## Repository Goal

本仓库当前目标是：

* 集中管理多个成员的独立 Unity 项目
* 保持不同 Unity 项目之间相互隔离
* 统一 Git 和 GitHub 协作流程
* 避免 Unity 自动生成文件进入版本库
* 通过 Branch + Pull Request 控制 `main`
* 为后续自动检查、CODEOWNERS、CI 和共享 Unity Packages 留出扩展空间
