# AGENTS.md

## プロジェクト概要

**Blazing Story** は、[Storybook](https://storybook.js.org/) の Blazor 向けクローンです。UI コンポーネントやページを分離して構築・カタログ化するためのフロントエンドワークショップを提供します。ほぼ 100% Blazor ネイティブで構築されており、npm や webpack などの JavaScript ツールチェーンは不要です。また、MCP (Model Context Protocol) サーバー機能により、AI エージェントへのコンポーネント情報の公開も可能です。

ライセンス: MPL-2.0

## NuGet パッケージ

このリポジトリは以下の NuGet パッケージを生成します。

| パッケージ名 | 役割 |
|---|---|
| **BlazingStory** | メインライブラリ。Storybook 風の UI カタログ機能を提供する |
| **BlazingStory.Abstractions** | 抽象インターフェースや基本型を定義する基盤レイヤー |
| **BlazingStory.Addons** | アドオン拡張のためのフレームワーク |
| **BlazingStory.Addons.BuiltIns** | 組み込みアドオンの実装 |
| **BlazingStory.ToolKit** | 共通ユーティリティを提供するツールキット |
| **BlazingStory.McpServer** | MCP サーバー統合機能 (AI/LLM 連携) |
| **BlazingStory.ProjectTemplates** | `dotnet new` 用のプロジェクトテンプレート |

## フォルダ構成

```
BlazingStory/                  … メインライブラリ (BlazingStory パッケージ)
BlazingStory.Abstractions/     … 抽象型・インターフェース (BlazingStory.Abstractions パッケージ)
BlazingStory.Addons/           … アドオンフレームワーク (BlazingStory.Addons パッケージ)
BlazingStory.Addons.BuiltIns/  … 組み込みアドオン (BlazingStory.Addons.BuiltIns パッケージ)
BlazingStory.ToolKit/          … ユーティリティ (BlazingStory.ToolKit パッケージ)
BlazingStory.McpServer/        … MCP サーバー (BlazingStory.McpServer パッケージ)
BlazingStory.Stories/          … デモ・参照用の Blazor WebAssembly アプリ (パッケージ生成なし)
ProjectTemplate/               … dotnet テンプレート (BlazingStory.ProjectTemplates パッケージ)
Samples/                       … サンプルアプリケーション
Tests/
  BlazingStory.Test/           … メインのテストプロジェクト
  BlazingStory.Build.Test/     … ビルド関連のテストプロジェクト
  Fixtures/                    … テスト用フィクスチャプロジェクト群
build/                         … MSBuild のカスタム .targets ファイル
```

## ビルドとテスト

```shell
# ビルド
dotnet build

# テスト実行
dotnet test
```
