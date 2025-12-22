using System;

namespace LivreNoirLibrary.Media
{
    public interface ITimelineObject
    {
        /// <summary>
        /// このオブジェクトが存在する楽譜上の位置。楽譜の開始位置を 0 とし、全音符(四分音符4個)を単位とする。
        /// </summary>
        public double Position { get; }
        public ObjectType Type { get; }
        /// <summary>
        /// このオブジェクトが持つ固有の値。その意味は ITimelineObject.Type に依存して決まる。
        /// </summary>
        public double Value { get; }
    }

    public enum ObjectType
    {
        /// <summary>
        /// 通常のオブジェクト。実際に映像に描画されるのはこのタイプに限られる。
        /// </summary>
        Normal,
        /// <summary>
        /// テンポを設定/変更する。ITimelineObject.Value はその位置から適用されるテンポを beats per minute 単位で表す。
        /// </summary>
        Tempo,
        /// <summary>
        /// 楽譜のスクロールを一時停止する。ITimelineObject.Value はその位置で適用される停止の期間を全音符単位で表す。
        /// </summary>
        Stop,
    }
}
