## スキルデータ仕様書

#### スキルを使うのに必要なデータ
|型|変数名|詳細|
| ------- | --------- | ----------- |
|int|skillId|スキルのID|
|int|pearentId|親スキルのID|
|int|nowLv|現在のレベル|
|int|maxLv|最大のレベル|
|int|animationID|アニメーションのID|
|int|hate|使用時の上昇ヘイト値|
|int|necessaryPoints|取得に必要なポイント|
|float|range|効果範囲|



#### そのほか演出等で必要なもの

|型|変数名|詳細|
| --- | ---- | ----- |
|float|recast|再使用までの待機時間|
|float|currentCast|現在のリキャスト経過時間|
|int|effect|使用するエフェクトID|
|---------|
|ステータス変化|
|---------|
|int|hp|HP回復|
|int|mp|MP回復|
|int|str|ストレングス変化|
|int|vit|バイタリティ変化|
|int|inte|インテリジェンス変化|
|int|mnd|マインド変化|
|int|dex|デクスタリティ変化|
|int|agi|アジリティ変化|



## マスターデータ

|int|スキルID|
|int|効果ターゲットID|
|int|攻撃範囲のタイプ|
|int|効果範囲|
|int|消費HP|
|int|消費MP|
|int|リキャスト|
|int|威力|
|int|アイコンID|
|int|アニメーションID|
|int|エフェクトID|
|string|効果説明文|
|int|親スキルID|
|int|最大レベル|


