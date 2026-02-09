[TOC]

# 简体中文

## Unity音频池系统

### 概述

本系统实现了Unity中非绑定物体音频播放的池化，给音频设计师提供了：
1. 可选择的音频实例回收功能
2. 可自定义的平台特化音频实例数规则
3. 统一且解耦的AudioManager单例类，用于提供音频播放入口
4. AudioEvent类，提供可扩展的音频单例基类

### 使用说明

#### AudioConfig

`AudioConfig` 定义了音频相关的设置，设计师可以通过`Asset (folder) → Scriptable Object → AudioConfig` 创建一个Scriptable Object。
<br>
该可编程物体包含了`2`个***公共字段***：
1. `AudioRecycleStrategy recycleStrategy` 用于定义音频实例的回收策略。本系统提供了`3`种回收策略，`ByTime`, `ByVolume`, 和`ByDistance`。
    <br>
   `ByTime` 允许系统按音频播放时间顺序回收实例（按最早开始播放 → 最晚开始播放的顺序）
   `ByVolume` 允许系统按音频音量排序回收实例（按最小音量 → 最大音量），如果音量相同，随机按默认顺序回收。
   `ByDistance` 允许系统按音频实例距离玩家位置远近回收（按最远 → 最近），如果距离相同，随机按默认顺序回收。
   <br><br>
2. `List<PlatformObjectNumber> objectPerPlatform`
    <br>
    该列表允许设计师在Unity检视器内针对各平台设置最大音频实例数。
    `struct PlayformObjectNumber` 标识了特定平台可以拥有的最大音频实例数。设计师可以在此处选择多种平台以匹配同一个实例数。

该可编程物体也包含了`1`个***公共方法***：
`public int GetMaxNumber()`

# ENGLISH

