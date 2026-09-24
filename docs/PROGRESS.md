# PROGRESS.md — 進捗と引き継ぎメモ

> Claude Code へ：セッション開始時に必ず読み、終了時に更新してください。
> 開発者へ：Claude Code に「今日はここまで。PROGRESS を更新して」と言えば更新されます。

## 基本情報

- **チャットタイトル**：開発_部員カードローグライク_2026-09-25
- **セッションキー**：2026-09-25_開発_01
- **作品名（仮）**：デッドライン・デッキ
- **リポジトリ**：https://github.com/bututetu1-stack/DeadlineDeck
- **Unity バージョン**：6000.3.22f1

## 現在地

- Phase：1（Unity と C# に慣れる）Step 1 完了 → 次は Step 2
- 状態：SampleScene に空の GameObject「Practice」を置き、`HelloDeadline` を付けて Console 出力を確認済み。開発者が自分でコミット＆プッシュできた
- 作業ブランチ：`claude/epic-ride-sqx1jy`（開発者の PC でもこのブランチで作業中。取り込みは `git pull`）

## 次にやること

Phase 1 のステップ計画（完了したら [x] にする）

- [x] Step 1：Unity の画面の歩き方 ＋ 初めてのスクリプト（`Debug.Log` で Console に表示）— 小課題「締切まであと8ターン」も完了
- [ ] Step 2：変数・メソッド・`[SerializeField]`（HP を持たせ、ダメージを Console で確認）
- [ ] Step 3：画面に文字を出す（Canvas・TextMeshPro・日本語フォント Noto Sans JP）
- [ ] Step 4：ボタンを押すと HP が減る（Button の OnClick とテキスト更新）
- [ ] Step 5：if 文で「撃破！」＋ リセット（Phase 1 完了条件の達成・コミット）

練習用スクリプトは `Assets/Scripts/Practice/` に置く（Phase 1 専用の練習場所）。

## 未決定事項・メモ

- 作品名は仮。部員に投票してもらうのもアリ
- 部員データの収集（Google フォーム）は Phase 7 までに始めておくと Phase 8 がスムーズ
- 本物の部員をカードにするときは、本人の了承を取る（CSV の consent 列）

## 作業ログ

| 日付 | Phase | やったこと | 次回 |
|---|---|---|---|
| 2026-09-25 | 0 | 仕様書・ロードマップ作成 | 環境構築 |
| 2026-09-24 | 1 | 仕様書を読み込み、Phase 1 を 5 ステップに分けて計画 | Step 1 から開始 |
| 2026-09-24 | 1 | Step 1：`Assets/Scripts/Practice/HelloDeadline.cs` を作成（Start と Debug.Log） | 動作確認・小課題 → Step 2 |
| 2026-09-24 | 1 | Step 1 完了。小課題で Debug.Log を自力で 1 行追加、初めて自分でコミット＆プッシュ | Step 2（変数・`[SerializeField]`） |
