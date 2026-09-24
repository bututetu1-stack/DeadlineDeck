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

- Phase：1（Unity と C# に慣れる）開始前
- 状態：Phase 0 完了（Unity プロジェクト作成・Force Text / Visible Meta Files 設定済み・最初のコミット済み）。Phase 1 の計画を作成した

## 次にやること

Phase 1 のステップ計画（完了したら [x] にする）

- [ ] Step 1：Unity の画面の歩き方 ＋ 初めてのスクリプト（`Debug.Log` で Console に表示）
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
