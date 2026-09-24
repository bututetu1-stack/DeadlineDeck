# 最初に読む — 環境構築と Claude Code の使い方

このフォルダは、部員カードのローグライク「デッドライン・デッキ（仮）」を **Claude Code に教わりながら作る** ための仕様書一式です。

## 入っているもの

| ファイル | 誰が読む | 中身 |
|---|---|---|
| `README_最初に読む.md` | あなた | このファイル。環境構築と使い方 |
| `CLAUDE.md` | Claude Code（自動で読む） | 「教えながら作る」ルール、コーディング規約 |
| `docs/GAME_SPEC.md` | 両方 | ゲームのルール・カード・敵の仕様 |
| `docs/TECH_SPEC.md` | 主に Claude Code | クラス設計・フォルダ構成 |
| `docs/ROADMAP.md` | 両方 | Phase ごとの作るもの・学ぶこと |
| `docs/PROGRESS.md` | 両方 | 進捗と引き継ぎ（毎回更新） |
| `docs/LEARNING_LOG.md` | あなた（Claude Code も追記） | 学んだ用語ノート |
| `Assets/Data/members_template.csv` | あなた | 部員データの入力例（Phase 8 で使用） |
| `.gitignore` | Git | Unity 用の除外設定 |

---

## Phase 0：環境構築（Windows）

### 1. Unity

1. [Unity Hub](https://unity.com/ja/download) をインストールし、Unity ID でログイン
2. Unity Hub →「インストール」→「エディターをインストール」→ **Unity 6 の LTS（6000.x.x LTS）** を選ぶ
3. 追加モジュールで **「Windows Build Support (IL2CPP)」** にチェック（Visual Studio が不要なら外してよい）
4. Unity Hub →「新しいプロジェクト」→ テンプレート **「2D (URP)」** → プロジェクト名 `DeadlineDeck` → 作成

### 2. コードエディタ

- おすすめ：**VS Code** ＋ 拡張機能「Unity」（Microsoft 製）と「C# Dev Kit」
- Unity 側：Edit → Preferences → External Tools → External Script Editor で VS Code を選ぶ
- 確認：Unity の Project ウィンドウで C# スクリプトをダブルクリック → VS Code が開き、`Debug.` と打つと候補が出れば OK

### 3. Git と GitHub

1. [Git for Windows](https://git-scm.com/) をインストール（Claude Code にも必要）
2. GitHub で **非公開（Private）** のリポジトリ `DeadlineDeck` を作る

### 4. Claude Code

- [公式ドキュメント](https://docs.claude.com/ja/docs/claude-code/overview) の手順でインストールし、ログインする
- 確認：PowerShell で `claude --version` がバージョンを表示すれば OK

### 5. 仕様書をプロジェクトに入れる

Unity プロジェクトのフォルダ（`Assets` フォルダがある階層）に、このフォルダの中身をすべてコピーします。

```
DeadlineDeck/            ← Unity プロジェクトのフォルダ
├─ Assets/
│  └─ Data/members_template.csv   ← コピー
├─ Packages/
├─ ProjectSettings/
├─ docs/                 ← コピー
├─ CLAUDE.md             ← コピー
├─ README_最初に読む.md    ← コピー
└─ .gitignore            ← コピー
```

### 6. Unity の設定（Git で壊さないため）

Unity で Edit → Project Settings → Editor を開き、次を確認します。

- Asset Serialization → Mode：**Force Text**
- Version Control → Mode：**Visible Meta Files**

### 7. 最初のコミット

プロジェクトのフォルダで PowerShell を開いて実行します（`<URL>` は GitHub のリポジトリ URL）。

```powershell
git init
git add .
git commit -m "プロジェクト作成と仕様書追加"
git branch -M main
git remote add origin <URL>
git push -u origin main
```

分からなければ、この段階から Claude Code に聞いてかまいません。

---

## Claude Code との進め方

### 毎回の始め方

プロジェクトのフォルダで PowerShell を開き、`claude` と入力して起動します。最初の一言は次のとおりです。

**初回：**
```
CLAUDE.md と docs/ の中身を全部読んで、プロジェクトの内容を理解したら、
Phase 1 を小さなステップに分けて計画を見せてください。
私は Unity 初心者なので、CLAUDE.md の「教え方ルール」に従って進めてください。
```

**2 回目以降：**
```
docs/PROGRESS.md を読んで、前回の続きから始めてください。
```

**終わるとき：**
```
今日はここまで。PROGRESS.md と LEARNING_LOG.md を更新してください。
```

### 覚えておくと便利な言い方

| こうしたいとき | こう言う |
|---|---|
| 説明が難しかった | 「もっと簡単に、たとえ話で説明して」 |
| 小課題で詰まった | 「ヒント 1 をください」（2、3 と段階的に） |
| 答えを見たい | 「答えを見せて」 |
| 自分のコードを見てほしい | 「○○.cs を書いたのでレビューして」 |
| エラーが出た | Console の赤いエラーをそのまま貼り付けて「このエラーの読み方から教えて」 |
| 理解を確かめたい | 「今日の内容からクイズを出して」 |
| 急ぎたい日 | 「今日は小課題なしで進めたい」 |

### 成長のためのコツ

1. **コードを貼られても、すぐ次に行かない**：解説を読んで、分からない行があれば必ず質問する
2. **小課題は飛ばさない**：自分で書いた 1 行は、読んだだけの 100 行より身につきます
3. **LEARNING_LOG の「自分のメモ」欄を自分の言葉で書く**
4. **毎回のセッション終わりにコミットする**：壊しても戻せる安心感があると、いろいろ試せます
5. **Phase 7 が終わったら部員に遊んでもらう**：感想が一番のモチベーションになります
