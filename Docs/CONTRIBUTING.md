# Contributing Guide

感谢参与本 Unity 多项目仓库的开发。

本仓库用于集中管理多个成员各自维护的 Unity 项目。为了避免项目之间相互干扰、错误提交 Unity 缓存、大文件进入普通 Git 历史，以及对 `main` 分支造成破坏，所有成员提交内容前请遵守本文档中的规则。

---

# 1. 仓库目录规则

所有成员的 Unity 项目必须放置于：

```text
Projects/<GitHub用户名>/<Unity项目名>/
```

例如：

```text
Projects/
├── alice/
│   ├── ShooterGame/
│   └── VRDemo/
│
├── bob/
│   └── Platformer/
│
└── chris/
    └── ARPrototype/
```

正确示例：

```text
Projects/alice/ShooterGame/
Projects/alice/VRDemo/
Projects/bob/Platformer/
```

错误示例：

```text
Projects/ShooterGame/
Projects/Test/
Projects/MyGame/
```

请不要直接在 `Projects/` 根目录创建 Unity 项目。

---

# 2. Unity 项目最低结构

每个 Unity 项目至少应包含：

```text
<ProjectName>/
├── Assets/
├── Packages/
└── ProjectSettings/
```

建议确保以下文件正常提交：

```text
Packages/manifest.json
Packages/packages-lock.json
ProjectSettings/ProjectVersion.txt
```

例如：

```text
Projects/
└── alice/
    └── ShooterGame/
        ├── Assets/
        ├── Packages/
        │   ├── manifest.json
        │   └── packages-lock.json
        │
        └── ProjectSettings/
            └── ProjectVersion.txt
```

---

# 3. Unity `.meta` 文件必须提交

Unity 使用 `.meta` 文件维护资源 GUID 和引用关系。

因此：

```text
Assets/Player.cs
Assets/Player.cs.meta
```

两者都必须提交。

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

禁止在 `.gitignore` 中添加：

```text
*.meta
```

也不要手动删除项目中的 `.meta` 文件。

Unity 项目建议设置：

```text
Edit
→ Project Settings
→ Version Control
→ Visible Meta Files
```

并建议：

```text
Edit
→ Project Settings
→ Editor
→ Asset Serialization
→ Force Text
```

---

# 4. 禁止提交 Unity 自动生成目录

以下内容不得提交：

```text
Library/
Temp/
Logs/
obj/
UserSettings/
Build/
Builds/
MemoryCaptures/
```

仓库根目录的 `.gitignore` 已经配置这些规则。

但每次提交前仍必须运行：

```bash
git status
```

检查实际提交内容。

如果发现：

```text
Library/
Temp/
Logs/
```

等目录出现在提交列表中，请停止提交并检查 `.gitignore`。

---

# 5. 禁止创建嵌套 Git 仓库

整个项目只能有一个 Git 仓库：

```text
Unity-Projects/.git/
```

成员自己的 Unity 项目中不得再次执行：

```bash
git init
```

禁止出现：

```text
Unity-Projects/
├── .git/
└── Projects/
    └── alice/
        └── ShooterGame/
            └── .git/
```

如果复制进来的 Unity 项目原本是一个独立 Git 仓库，请先删除其内部 `.git` 目录，再加入本仓库。

---

# 6. Git LFS

本仓库使用 Git LFS 管理部分大型二进制资源。

首次 Clone 仓库后，请执行：

```bash
git lfs install
```

当前仓库计划使用 Git LFS 管理：

```text
.psd
.psb
.blend
.fbx
.wav
.mp4
.mov
```

具体规则由根目录：

```text
.gitattributes
```

统一管理。

请勿擅自删除或覆盖 `.gitattributes`。

如果需要新增其他大型文件类型，请先与仓库管理员沟通。

---

# 7. 不要提交构建产物

原则上不要提交以下内容：

```text
.exe
.apk
.aab
Game_Data/
Windows Build/
Android Build/
大型 ZIP
```

GitHub 仓库用于保存：

```text
源代码
Unity Assets
ProjectSettings
Packages
开发文档
```

而不是存储正式 Build。

---

# 8. 不要提交敏感信息

禁止提交：

```text
密码
API Key
Access Token
Private Key
数据库密码
服务器密码
证书私钥
.env 中的秘密信息
Service Account 密钥
```

如果误提交敏感信息，请立即联系仓库管理员。

仅仅删除文件通常不足以消除 Git 历史中的敏感内容。

---

# 9. main 分支规则
！！！！！！创建自己的开发分支：（重要！！！！！建议你在置入自己项目的时候单独创建一个分支，这样可以防止上传库时覆盖别人的上传，并且可以形成一份自己分支的日志）
`main` 是仓库主分支。

原则上：

```text
main = 已审核、可正常使用的版本
```

普通开发工作不要直接在 `main` 上进行。

标准流程：

```text
main
 ↓
创建开发分支
 ↓
开发
 ↓
commit
 ↓
push
 ↓
Pull Request
 ↓
Review / 自动检查
 ↓
Merge
 ↓
main
```

即使管理员拥有 Ruleset Bypass 权限，也建议对重要项目修改继续通过 Pull Request 完成。

---

# 10. 开始工作前

每次开始新任务时：

```bash
git switch main
git pull
```

确认 `main` 已更新到最新版本。

然后创建新的开发分支：

```bash
git switch -c <用户名>/<任务名>
```

例如：

```bash
git switch -c alice/add-shooter-project
```

或者：

```bash
git switch -c alice/update-player-controller
```

推荐的分支命名：

```text
<GitHub用户名>/<任务>
```

例如：

```text
alice/add-vr-demo
alice/update-shooter-ui
bob/fix-camera
chris/update-ar-tracking
```

---

# 11. 不要长期在同一个开发分支工作

开发分支代表“一次具体任务”，而不是长期个人分支。

推荐：

```text
alice/add-project
alice/update-player
alice/fix-camera
```

任务完成并 Merge 后，可以删除该分支。

下一次任务重新从最新 `main` 创建分支。

---

# 12. 提交自己的项目

假设用户名：

```text
alice
```

项目：

```text
ShooterGame
```

修改完成后首先执行：

```bash
git status
```

确认文件范围。

然后：

```bash
git add Projects/alice/ShooterGame
```

不建议无条件使用：

```bash
git add .
```

因为这可能将其他成员目录或其他无关修改一起加入提交。

再次检查：

```bash
git status
```

确认没有误修改：

```text
Projects/bob/
Projects/chris/
```

之后再 Commit。

---

# 13. Commit Message 规范

Commit Message 应准确描述：

> 这次提交实际完成了什么？

推荐格式：

```text
Add ...
Update ...
Fix ...
Remove ...
Configure ...
Initialize ...
Refactor ...
```

例如：

```text
Add ShooterGame project
Add player movement system
Update inventory UI
Fix camera rotation
Remove unused assets
Configure Git LFS
Update project documentation
Refactor enemy controller
```

不推荐：

```text
update
test
123
final
final2
修改
临时
aaaa
```

建议一次 Commit 只处理一类明确任务。

例如：

```text
Add enemy AI
```

和：

```text
Update README
```

如果属于完全不同的修改，最好拆成两个 Commit。

---

# 14. 创建 Commit

添加文件后：

```bash
git status
```

确认：

```text
Changes to be committed
```

然后：

```bash
git commit -m "Update ShooterGame player movement"
```

注意：

```text
git commit
```

只会保存到本地 Git 仓库。

它不会自动上传 GitHub。

---

# 15. 上传开发分支

第一次上传：

```bash
git push -u origin <分支名>
```

例如：

```bash
git push -u origin alice/update-player
```

之后该分支再次 Push 时可以直接：

```bash
git push
```

---

# 16. Pull Request

Push 完成后，在 GitHub 创建 Pull Request。

目标必须确认：

```text
base: main
```

当前开发分支作为：

```text
compare: alice/update-player
```

即：

```text
alice/update-player
        ↓
       main
```

Pull Request 描述中应说明：

```text
修改了什么
为什么修改
涉及哪个 Unity 项目
是否有需要特别检查的内容
```

---

# 17. Pull Request 提交前检查

请确认：

* [ ] 项目位于 `Projects/<GitHub用户名>/<项目名>/`
* [ ] 项目包含 `Assets/`
* [ ] 项目包含 `Packages/`
* [ ] 项目包含 `ProjectSettings/`
* [ ] `Packages/manifest.json` 已提交
* [ ] `ProjectSettings/ProjectVersion.txt` 已提交
* [ ] `.meta` 文件已提交
* [ ] 未提交 `Library/`
* [ ] 未提交 `Temp/`
* [ ] 未提交 `Logs/`
* [ ] 未提交 `UserSettings/`
* [ ] 未提交 Build 文件
* [ ] 未提交密码或密钥
* [ ] 未创建项目内部 `.git`
* [ ] 未误修改其他成员项目
* [ ] Commit Message 能准确说明修改内容

---

# 18. 修改其他成员项目

原则上每位成员负责：

```text
Projects/<自己的GitHub用户名>/
```

不要未经沟通直接修改：

```text
Projects/<其他成员用户名>/
```

如果确实需要协作，请先沟通，再通过：

```text
Branch
→ Commit
→ Push
→ Pull Request
→ Review
```

完成修改。

---

# 19. Pull Request 合并后

PR Merge 后，本地旧开发分支不会自动更新。

开始下一项任务前：

```bash
git switch main
git pull
```

然后重新创建分支：

```bash
git switch -c <用户名>/<新任务>
```

如果旧分支已经完成，可以删除本地分支：

```bash
git branch -d <旧分支名>
```

例如：

```bash
git branch -d alice/update-player
```

---

# 20. 遇到 Git 问题时

如果不确定当前发生了什么，第一条命令始终是：

```bash
git status
```

不要在不了解含义的情况下执行：

```text
git reset --hard
git clean -fd
git push --force
```

也不要：

```text
删除 .git
重新 git init
```

这些操作可能导致本地修改或提交历史丢失。

如果问题无法判断，请保留当前目录状态，并将：

```bash
git status
```

以及完整错误信息提供给仓库管理员。

---

# 21. 基本原则

本仓库协作遵循以下原则：

```text
先同步
→ 再创建分支
→ 只修改需要修改的内容
→ 提交前检查
→ Commit
→ Push
→ Pull Request
→ Review
→ Merge
```

保持 `main` 稳定，比快速直接 Push 更重要。

Git 新手请阅读：

```text
Docs/Git-Beginner-Guide.md
```
