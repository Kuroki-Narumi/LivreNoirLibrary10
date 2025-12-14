using LivreNoirLibrary.Media.Bms.Play;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IBmsTimer obj)
        {
            public static TimerId JudgeType2TimerId(JudgeType type) => (TimerId)(TimerIdOffsets.GeneralJudge + (int)type);
            public static TimerId Player2TimerId(int player) => (TimerId)(TimerIdOffsets.PlayerJudge + player * 10);
            public static TimerId Lane2TimerId(int lane) => (TimerId)(TimerIdOffsets.Button + lane * 10);

            public void SetJudgeTimer(double time, JudgeType type, int player, double offset)
            {
                obj.Set(JudgeType2TimerId(type), time);
                if (player is > 0)
                {
                    var id = Player2TimerId(player);
                    obj.Set(id + TimerIdOffsets.Judge, time);

                    if (offset is > 0)
                    {
                        obj.Set(id + TimerIdOffsets.Late, time);
                    }
                    else
                    {
                        obj.Remove(id + TimerIdOffsets.Late);
                    }
                    if (offset is < 0)
                    {
                        obj.Set(id + TimerIdOffsets.Early, time);
                    }
                    else
                    {
                        obj.Remove(id + TimerIdOffsets.Early);
                    }
                }
            }

            public void PrepareToPlay()
            {
                obj.Clear();
                obj.Set(TimerId.Scene_Start, 0);
            }

            public void SetBeatTimer(double time, ITimeCounter timeCounter, IBarPositionProvider<double> provider)
            {
                if (obj.TryGet(TimerId.Play_MusicStart, time, out var musicTime))
                {
                    var beat = timeCounter.Time2Beat(musicTime);
                    var pos = provider.GetBarPosition(beat);
                    var inBeat = (((double)pos.Offset * provider.GetBarLength(pos.Bar)) * 4) % 1;
                    obj.Set(TimerId.Play_Beat, time - inBeat);
                }
            }
        }

        extension(IBgaVisibilityProvider obj)
        {
            public bool GetShowFlag(BgaVisibility flag) => (obj.BgaVisibility & flag) is not 0;
            public void SetShowFlag(BgaVisibility flags, bool value)
            {
                if (value)
                {
                    obj.BgaVisibility |= flags;
                }
                else
                {
                    obj.BgaVisibility &= ~flags;
                }
            }
        }

        public static readonly HsCorrectionMode[] HsCorrectionModes =
        [
            HsCorrectionMode.None,
            HsCorrectionMode.MaxBpm,
            HsCorrectionMode.MinBpm,
            HsCorrectionMode.AverageBpm,
            HsCorrectionMode.MainBpm,
            HsCorrectionMode.MainTimeBpm,
        ];

        extension(IHighSpeedProvider obj)
        {
            public double ActualHighSpeed => obj.HighSpeed * obj.HighSpeedCorrection;


            public void UpdateHsCorrection(ITimeCounter counter)
            {
                var factor = obj.HsCorrectionMode switch
                {
                    HsCorrectionMode.MaxBpm => counter.MaxTempo,
                    HsCorrectionMode.MinBpm => counter.MinTempo,
                    HsCorrectionMode.AverageBpm => counter.AverageTempo,
                    HsCorrectionMode.MainBpm => counter.MainTempo,
                    HsCorrectionMode.MainTimeBpm => counter.MainTimeTempo,
                    _ => 0,
                };
                obj.HighSpeedCorrection = factor is > 0 ? 120 / factor : 1;
            }
        }

        public static JudgeInfo CreateJudgeInfo(in JudgeDefinition judge, double error, in ScoreDefinition score, in GaugeDefinition gauge, double gaugeGainBase)
        {
            var type = judge.Type;
            var comboChange = judge.ComboChange;
            var isMiss = judge.IsMiss;
            var scoreGain = score.GetScoreGain(type);
            var gaugeGain = gauge.GetGaugeGain(type, gaugeGainBase);
            return new(type, comboChange, isMiss, error, scoreGain, gaugeGain);
        }

        extension(IJudgeProvider obj)
        {
            public void UpdateGaugeGainBase(IBmsViewModel vm, bool includesNoteEnd)
            {
                obj.GaugeGainBase = vm.Total / vm.CurrentTimeline.GetNoteCount(includesNoteEnd);
            }

            public JudgeInfo GetThroughJudge() => CreateJudgeInfo(obj.Judges.ThroughJudge, 0, obj.ScoreDefinition, obj.GaugeDefinition, obj.GaugeGainBase);

            public bool TryGetJudge(double error, out JudgeInfo judge)
            {
                if (obj.Judges.TryGetJudge(error, out var j))
                {
                    judge = CreateJudgeInfo(j, j.Type is JudgeType.Perfect ? 0 : error, obj.ScoreDefinition, obj.GaugeDefinition, obj.GaugeGainBase);
                    return true;
                }
                judge = default;
                return false;
            }
        }
    }
}
