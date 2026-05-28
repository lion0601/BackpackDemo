# 末日背包 - 生存背包管理系统

> 一款基于 Unity 的背包管理原型，参考《阿瑞斯病毒》的核心设计理念，模拟生存游戏中的资源取舍与策略决策。

---

## ✨ 功能亮点

- ✅ **左键拾取 / 右键丢弃**：简单直观的物品管理  
- ✅ **拖拽交换**：任意拖拽物品到其他格子，支持交换与移动  
- ✅ **一键整理**：合并同类物品，按名称自动排序  
- ✅ **一键清空**：快速清空所有格子（负重归零）  
- ✅ **负重系统**：50点负重上限，满负重无法拾取  
- ✅ **本地存档**：关闭游戏后背包状态完全保留（PlayerPrefs）

---

## 🛠 技术栈

| 类别 | 技术 |
|------|------|
| 引擎 | Unity 2022.3 (2D Core) |
| 语言 | C# |
| UI | UGUI、GridLayoutGroup |
| 数据驱动 | ScriptableObject |
| 存档 | PlayerPrefs |
| 交互接口 | IPointerClickHandler, IBeginDragHandler, IDropHandler |

---

## 🎮 操作说明

| 操作 | 效果 |
|------|------|
| 左键点击格子 | 拾取/增加物品（空格子生成默认物品） |
| 右键点击格子 | 丢弃物品（每次减1，归零后清空） |
| 拖拽格子 | 交换两个格子的物品 |
| 点击"整理"按钮 | 合并同类物品，按名称排序 |
| 点击"清空"按钮 | 清空所有格子，负重归零 |

---

## 📦 如何运行

1. 克隆本仓库：git clone https://github.com/lion0601/BackpackDemo.git
2. 用 Unity 2022.3 或更高版本打开项目
3. 打开 Assets/Scenes/GameScene.unity
4. 点击 **Play** 按钮开始体验

> 注：仓库已清理 Library 等临时文件夹，首次打开需等待导入。

---

## 📺 演示视频

[点击观看演示视频](https://www.bilibili.com/video/BV1otVp6bE9P/)

---

## 📄 游戏设计文档（GDD）

[查看GDD](GDD.md)

---

## 📊 游戏分析报告

[《星露谷物语》经济系统分析报告](StardewValley_Economic_Analysis.pdf)

---

## 👤 作者

- 2027届计算机专业，目标岗位：游戏策划 / 技术策划
- GitHub：[lion0601](https://github.com/lion0601)

---

## 📜 参考与致敬

- 《阿瑞斯病毒》——背包网格与资源管理的灵感来源
- Unity 官方文档 & UGUI 教程
