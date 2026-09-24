# TECH_SPEC.md — 技術設計書

> Claude Code へ：この設計は「初心者が理解できること」を優先しています。より高度な設計を思いついても、まずこの形で作り、改善はロードマップの後半で **学習テーマとして** 提案してください。

## 1. 環境

| 項目 | 内容 |
|---|---|
| Unity | Unity 6 LTS（Unity Hub から入れる最新の 6000.x LTS） |
| テンプレート | 2D (URP) |
| UI | uGUI（Canvas）＋ TextMeshPro |
| 入力 | uGUI のボタン・EventSystem（マウス操作のみ） |
| 日本語フォント | Noto Sans JP を TextMeshPro の Font Asset に変換して使う |
| テスト | Unity Test Framework（EditMode テスト） |
| バージョン管理 | Git ＋ GitHub（非公開リポジトリ） |
| ビルド対象 | Windows（.exe） |

### プロジェクト設定（最初に必ず確認）

- Edit → Project Settings → Editor → **Asset Serialization Mode = Force Text**
- 同 → **Version Control Mode = Visible Meta Files**
- Game ビューの解像度：1920×1080（16:9）
- Canvas Scaler：Scale With Screen Size、基準解像度 1920×1080

## 2. フォルダ構成

```
Assets/
├─ Scripts/
│  ├─ Data/        … ScriptableObject の定義（CardData, EnemyData など）と enum
│  ├─ Battle/      … 戦闘ロジック（MonoBehaviour に依存しない C# クラス中心）
│  ├─ Run/         … ラン（1 回のプレイ全体）の管理、マップ
│  ├─ UI/          … 表示用の MonoBehaviour（CardView など）
│  └─ Editor/      … エディタ拡張（CSV インポーターなど）
├─ Data/
│  ├─ Cards/       … CardData アセット
│  ├─ Enemies/     … EnemyData アセット
│  ├─ Tags/        … TagBonusData アセット
│  └─ members.csv  … 部員データ（後半で使用）
├─ Prefabs/        … CardView, EnemyView など
├─ Scenes/         … Title, Map, Battle, Reward, Rest, Result
├─ Art/            … 画像
├─ Fonts/          … Noto Sans JP
└─ Tests/EditMode/ … テストコード
```

## 3. 全体の構造

```
【データ層】ScriptableObject（Inspector で編集できる設定ファイル）
   CardData / EnemyData / TagBonusData
        │ 読むだけ
        ▼
【ロジック層】普通の C# クラス（画面を知らない）
   BattleState, Combatant, Deck, EffectResolver, EnemyAI
        │ イベント（C# の event）で変化を通知
        ▼
【表示層】MonoBehaviour（画面に出す・クリックを受け取る）
   BattleController, CardView, HandView, EnemyView, HudView
```

- **なぜ分けるか**：ロジックを画面から切り離すと、①テストが書ける ②バグの場所を特定しやすい ③後で見た目を差し替えやすい
- 表示層はロジックの状態を「見て」描画し、プレイヤーの操作をロジックに「お願い」するだけにする

## 4. データ定義（ScriptableObject）

### 4.1 enum 一覧（`Scripts/Data/Enums.cs`）

```csharp
public enum CardType { Attack, Skill, Power, Curse }        // 作業, 対策, 体質, 手戻り
public enum TargetType { SingleEnemy, AllEnemies, Self, None }
public enum EffectType {
    Damage,          // ダメージ（value）
    Block,           // 余裕を得る（value）
    Draw,            // カードを引く（value）
    GainEnergy,      // 集中力を得る（value）
    LoseHp,          // 気力を失う（value）
    ApplyMotivation, // やる気を付与（value）
    ApplySleepy,     // 寝不足を付与（value ターン）
    AddCurseToDeck,  // 手戻りを山札に加える（value 枚）
    RemoveCurse      // 手札の手戻りを消す（value 枚）
}
public enum ConditionType {
    None,            // 常に
    DeadlineNear,    // 締切まで残り value ターン以下
    HpBelowHalf,     // 気力が半分以下
    TagCombo,        // このターン同タグを value 枚以上使った
    FirstCardOfTurn  // このターン最初のカード
}
public enum CardTag { CSharp, Infra, CG, Rhythm, LateNight /* 部員データに合わせて追加 */ }
```

### 4.2 効果の表現（最初は「enum ＋ 数値」方式）

```csharp
[System.Serializable]
public class EffectEntry {
    public EffectType type;
    public int value;
    public TargetType target;
    public int repeat = 1;          // 「3 ダメージを 2 回」用
}

[System.Serializable]
public class PassiveEntry {        // 性格パッシブ
    public string personalityName;  // 表示用（例：締切駆動）
    public ConditionType condition;
    public int conditionValue;
    public EffectEntry[] bonusEffects;
    public float damageMultiplier = 1f; // 「ダメージ 2 倍」用
    public int costOverride = -1;       // 「コスト 1 として扱う」用（-1 = なし）
}
```

- 効果の実行は `EffectResolver` の `switch (effect.type)` で行う
- **学習テーマ（ロードマップ Phase 8）**：効果が増えて switch が長くなったら、「Strategy パターン（効果ごとにクラスを分ける）」へのリファクタリングを教材として行う

### 4.3 CardData

```csharp
[CreateAssetMenu(menuName = "DeadlineDeck/Card")]
public class CardData : ScriptableObject {
    public string id;
    public string displayName;
    public string memberName;       // 部員名（基本カードは空）
    public int cost;
    public CardType type;
    public CardTag[] tags;
    public EffectEntry[] effects;
    public PassiveEntry passive;    // なければ condition = None かつ効果なし
    [TextArea] public string flavorText;
    public Sprite artwork;
}
```

### 4.4 EnemyData

```csharp
[System.Serializable]
public class EnemyAction {
    public string label;            // 表示用
    public IntentType intent;       // Attack, Defend, Debuff, Buff, Special（Enums.cs に追加）
    public EffectEntry[] effects;
}

[CreateAssetMenu(menuName = "DeadlineDeck/Enemy")]
public class EnemyData : ScriptableObject {
    public string id;
    public string displayName;
    public int maxHp;
    public bool isBoss;
    public int deadlineTurns = 8;   // この敵がいる戦闘の締切（複数体なら最大値を使う）
    public EnemyAction[] actionPattern; // 上から順に繰り返す
    public Sprite artwork;
}
```

### 4.5 TagBonusData

```csharp
[CreateAssetMenu(menuName = "DeadlineDeck/TagBonus")]
public class TagBonusData : ScriptableObject {
    public CardTag tag;
    public string displayName;      // 【C#】など
    public int requiredCount = 3;
    public EffectEntry[] bonusEffects;
}
```

## 5. 主要クラス

### ロジック層（`Scripts/Battle/`）

| クラス | 役割 |
|---|---|
| `Combatant` | HP・最大 HP・余裕・状態（やる気・寝不足）を持つ。`TakeDamage()`、`GainBlock()`。プレイヤーと敵の共通部分 |
| `PlayerState : Combatant` | 集中力 |
| `EnemyState : Combatant` | 元データ（EnemyData）、行動パターンの何番目か、次の行動 |
| `Deck` | 山札・手札・捨て札・廃棄の 4 つのリスト。`Shuffle()`、`Draw(n)`、`Discard()`、捨て札の再利用 |
| `CardInstance` | 戦闘中の 1 枚のカード（CardData への参照 ＋ 一時的な変化）。同じカードが 2 枚あっても区別できるようにするため |
| `BattleState` | 戦闘全体の状態：プレイヤー、敵リスト、デッキ、現在のターン、締切、このターンに使ったタグの数 |
| `EffectResolver` | 効果 1 つを実際に適用する（ダメージ計算：基本値 ＋ やる気 → 寝不足で ×0.75 → 炎上で ×2 → 余裕で吸収） |
| `ConditionChecker` | 性格パッシブの条件を判定する |
| `TagComboTracker` | タグの使用回数を数えて、ボーナス発動を判定する |
| `EnemyAI` | 行動パターンから次の行動を決める |
| `BattleFlow` | ターン進行の状態マシン：`PlayerTurnStart → PlayerTurn → EnemyTurn → (Win / Lose)` |

**イベント（表示層への通知）の例**：`OnHpChanged`、`OnCardDrawn`、`OnCardPlayed`、`OnTurnChanged`、`OnBattleEnded(bool win)`

### 表示層（`Scripts/UI/`）

| クラス | 役割 |
|---|---|
| `BattleController` | Battle シーンの司令塔。BattleState を作り、UI とつなぐ。唯一「ロジックと表示の両方を知っている」クラス |
| `CardView` | カード 1 枚の見た目。クリックされたら BattleController に通知 |
| `HandView` | 手札の CardView を並べる |
| `EnemyView` | 敵 1 体の見た目、HP バー、インテント表示。クリックで攻撃対象に選ぶ |
| `HudView` | 気力・集中力・余裕・締切・山札/捨て札の枚数 |

### ラン層（`Scripts/Run/`）

| クラス | 役割 |
|---|---|
| `RunState` | ラン全体の状態：現在の気力、デッキ（CardData のリスト）、マップの現在地。**シーンをまたいで保持** する |
| `RunManager` | `RunState` を持つシングルトン（`DontDestroyOnLoad`）。シーン移動を担当 |
| `MapData` / `MapNode` | マスの種類（Battle / Rest / Boss）と、そこで出る敵 |

## 6. 部員データの取り込み（MVP 後）

### 6.1 流れ

```
Google フォーム（部員が自己申告） → スプレッドシート → CSV 出力
  → Assets/Data/members.csv（members_template.csv を参考に作成） → Unity メニュー「DeadlineDeck/部員CSVを取り込む」
  → CardData アセットを自動生成・更新
```

### 6.2 CSV の列（`members.csv`）

| 列名 | 例 | 説明 |
|---|---|---|
| id | m_yoru | 英数字、一意 |
| memberName | よる | 表示する部員名（ニックネーム可） |
| cardName | 夜型プログラマ | カード名 |
| cost | 1 | |
| type | Attack | Attack / Skill / Power |
| tags | CSharp;LateNight | `;` 区切り |
| effects | Damage:5:SingleEnemy | `種類:値:対象` を `;` 区切り。`Damage:3:SingleEnemy:2` のように 4 つ目で回数 |
| passiveName | 締切駆動 | |
| condition | DeadlineNear:3 | `条件:値` |
| passiveEffects | | パッシブの追加効果（effects と同じ書式） |
| damageMultiplier | 2 | |
| flavor | まだ慌てる時間じゃない | |
| consent | yes | **本人が掲載を了承しているか。yes 以外は取り込まない** |

- 取り込み処理は `Scripts/Editor/MemberCsvImporter.cs`（`[MenuItem]` を使うエディタ拡張）
- フォームで集めた「得意技術・性格・趣味・口癖」を上の列に変換する作業は人間（開発者）が行う。バランスを取るのも人間の仕事

## 7. テスト方針

- ロジック層は EditMode テストを書く（例：`Deck` の山札切れ時に捨て札が戻る、ダメージ計算の順番、締切を過ぎたら 2 倍）
- テストは「Window → General → Test Runner」から実行するよう開発者に案内する
- テストの書き方自体も学習テーマ（ロードマップ Phase 3 で初登場）

## 8. やらないこと（MVP では）

- セーブ/ロード（ラン途中の保存）
- ドラッグ＆ドロップでのカード使用（クリックで選択 → 対象クリックで使用、でよい）
- アニメーションライブラリ（DOTween など）。演出は Phase 9 でコルーチンを使って少しだけ
- オンライン対戦、スマホ対応
