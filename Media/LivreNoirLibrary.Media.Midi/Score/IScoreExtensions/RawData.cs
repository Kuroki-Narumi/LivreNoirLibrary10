using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    using NoteQueue = Queue<(Rational, Note)>;

    public static partial class IScoreExtensions
    {
        public static void ParseRawData(this IScore target, RawData source)
        {
            var tracks = source.Tracks;
            var c = tracks.Length;
            target.InitializeTracks(c);
            var conductor = target.ConductorTrack.Timeline;
            var whole = source.Resolution * 4;
            var currentNotes = new NoteQueue[128];
            for (var i = 0; i < 128; i++)
            {
                currentNotes[i] = [];
            }
            Dictionary<CCType, int> ccCache = [];
            for (var i = 0; i < c; i++)
            {
                var track = target.GetTrack(i);
                var timeline = track.Timeline;
                foreach (var cur in currentNotes)
                {
                    cur.Clear();
                }
                ccCache.Clear();

                void ProcessCC(Rational pos, Events.ControlChange cc)
                {
                    switch (cc.Number)
                    {
                        case CCType.RPN_MSB:
                            Set_RPN_MSB(pos, CCType.RPN, cc.Value);
                            return;
                        case CCType.RPN_LSB:
                            Set_RPN_LSB(pos, CCType.RPN, cc.Value);
                            return;
                        case CCType.NRPN_MSB:
                            Set_RPN_MSB(pos, CCType.NRPN, cc.Value);
                            return;
                        case CCType.NRPN_LSB:
                            Set_RPN_LSB(pos, CCType.NRPN, cc.Value);
                            return;
                        case CCType.DataEntry:
                            Set_Data_MSB(pos, cc.Value);
                            return;
                        case CCType.DataEntry_LSB:
                            Set_Data_LSB(pos, cc.Value);
                            return;
                        case CCType.BankSelect:
                        case CCType.BankSelect_LSB:
                            ccCache[cc.Number] = cc.Value;
                            return;
                    }
                    if (Events.ControlChange.IsMSB(cc.Number))
                    {
                        Set_General_MSB(pos, cc.Number, cc.Value);
                    }
                    else if (Events.ControlChange.IsLSB(cc.Number))
                    {
                        ccCache[cc.Number] = cc.Value;
                    }
                    else
                    {
                        timeline.Add(pos, new ControlChange(cc.Number, cc.Value, cc.Ext));
                    }
                }

                void Set_RPN_MSB(Rational pos, CCType type, int value)
                {
                    var v = ccCache.GetValueOrDefault(type, 0);
                    v &= 0x00FF;
                    v |= value << 8;
                    ccCache[type] = v;
                    if (v is 0x7F7F)
                    {
                        ProcessAddRPN(pos);
                    }
                }

                void Set_RPN_LSB(Rational pos, CCType type, int value)
                {
                    var v = ccCache.GetValueOrDefault(type, 0);
                    v &= 0xFF00;
                    v |= value;
                    ccCache[type] = v;
                    if (v is 0x7F7F)
                    {
                        ProcessAddRPN(pos);
                    }
                }

                void Set_Data_MSB(Rational pos, int value)
                {
                    if (ccCache.TryGetValue(CCType.DataEntry_LSB, out var v))
                    {
                        ccCache.Remove(CCType.DataEntry_LSB);
                        ccCache[CCType.Data] = (value << 8) | v;
                        ProcessAddRPN(pos);
                    }
                    else if (ccCache.TryGetValue(CCType.RPN, out var rpn) && Events.ControlChange.NeedsDataMSB(rpn))
                    {
                        ccCache[CCType.DataEntry] = value;
                    }
                    else
                    {
                        ccCache[CCType.Data] = value;
                        ProcessAddRPN(pos);
                    }
                }

                void Set_Data_LSB(Rational pos, int value)
                {
                    if (ccCache.TryGetValue(CCType.DataEntry, out var v))
                    {
                        ccCache.Remove(CCType.DataEntry);
                        ccCache[CCType.Data] = (v << 8) | value;
                        ProcessAddRPN(pos);
                    }
                    else
                    {
                        ccCache[CCType.DataEntry_LSB] = value;
                    }
                }

                void ProcessAddRPN(Rational pos)
                {
                    CCType type;
                    if (ccCache.TryGetValue(CCType.RPN, out var rpn))
                    {
                        type = CCType.RPN;
                    }
                    else if (ccCache.TryGetValue(CCType.NRPN, out rpn))
                    {
                        type = CCType.NRPN;
                    }
                    else
                    {
                        return;
                    }
                    var value = ccCache.GetValueOrDefault(CCType.Data, 0);
                    timeline.Add(pos, new ControlChange(type, value, rpn));
                    ccCache.Remove(CCType.Data);
                    if (rpn == 0x7F7F)
                    {
                        ccCache.Remove(type);
                    }
                }

                void Set_General_MSB(Rational pos, CCType type, int value)
                {
                    var lsb = Events.ControlChange.GetLSB(type);
                    if (ccCache.TryGetValue(lsb, out var v))
                    {
                        value = (value << 8) | v;
                        ccCache.Remove(lsb);
                    }
                    timeline.Add(pos, new ControlChange(type, value));
                }

                void ProcessPC(Rational pos, Events.ProgramChange ev)
                {
                    int? bank;
                    if (ccCache.TryGetValue(CCType.BankSelect, out var msb) &&
                        ccCache.TryGetValue(CCType.BankSelect_LSB, out var lsb))
                    {
                        bank = (msb << 8) | lsb;
                    }
                    else
                    {
                        bank = null;
                    }
                    timeline.Add(pos, new ControlChange(CCType.ProgramChange, ev.Number, bank));
                }

                void ProcessAltPC(Rational pos, Events.Event ev)
                {
                    ControlChange? cc = ev switch
                    {
                        Events.PolyphonicKeyPressure e => new(CCType.PolyphonicKeyPressure, e.Velocity, e.Number),
                        Events.ControlChange e => new(e.Number, e.Value, e.Ext),
                        Events.ChannelPressure e => new(CCType.ChannelPressure, e.Velocity),
                        Events.PitchBend pb => new(CCType.PitchBend, pb.Value),
                        Events.SequenceNumberEvent sn => new(CCType.SequenceNumber, sn.Number),
                        Events.ChannelPrefix sn => new(CCType.ChannelPrefix, sn.Channel),
                        Events.Port port => new(CCType.Port, port.Value),
                        _ => null,
                    };
                    if (cc is not null)
                    {
                        timeline.Add(pos, cc);
                    }
                }

                foreach (var (tick, ev) in tracks[i])
                {
                    var pos = IObject.GetPosition(tick, whole);

                    // 最初のポート指定はイベントリストに入れない
                    if (track.Port is < 0 && ev is Events.Port port)
                    {
                        track.Port = port.Value;
                        continue;
                    }
                    // 最初に指定されたチャンネルをそのトラックのチャンネルとする
                    if (track.Channel is < 0 && ev is Events.ChannelEvent chev)
                    {
                        track.Channel = chev.Channel;
                    }

                    void CheckNoteOff(int number)
                    {
                        if (currentNotes[number].TryDequeue(out var q))
                        {
                            var (pp, pn) = q;
                            pn.Length = pos - pp;
                            timeline.Add(pp, pn);
                        }
                    }

                    switch (ev)
                    {
                        case Events.TrackEnd:
                            // do nothing
                            break;
                        case Events.Tempo tempo:
                            conductor.Add(pos, new TempoEvent(tempo.Value));
                            break;
                        case Events.TimeSignature sig:
                            target.SetTimeSignature(pos, sig.ToStruct());
                            break;
                        case Events.Tonality ton:
                            conductor.Add(pos, new TonalityEvent(ton.ToStruct()));
                            break;
                        case Events.MetaText mt:
                            if (mt.Type is MetaType.Copyright)
                            {
                                target.Copyright = mt.Text;
                            }
                            else if (mt.Type is MetaType.Title)
                            {
                                track.Title = mt.Text;
                            }
                            else
                            {
                                timeline.Add(pos, new MetaText(mt));
                            }
                            break;
                        case Events.SequenceNumberEvent or Events.ChannelPrefix or Events.Port:
                            ProcessAltPC(pos, ev);
                            break;
                        case Events.SmpteOffset ofs:
                            timeline.Add(pos, new SmpteOffsetEvent(ofs));
                            break;
                        case Events.SequencerEvent seq:
                            timeline.Add(pos, new SequencerEvent(seq));
                            break;
                        case Events.SysEx sysex:
                            timeline.Add(pos, new SysEx(sysex));
                            break;
                        case Events.NoteOn no:
                            var number = no.Number;
                            var vel = no.Velocity;
                            // vel = 0 の NoteOn を NoteOff の代わりに置く場合もある
                            if (vel is 0)
                            {
                                CheckNoteOff(number);
                            }
                            else
                            {
                                Note note = new() { Number = number, Velocity = vel };
                                currentNotes[number].Enqueue((pos, note));
                            }
                            break;
                        case Events.NoteOff no:
                            CheckNoteOff(no.Number);
                            break;
                        case Events.ControlChange cc:
                            ProcessCC(pos, cc);
                            break;
                        case Events.ProgramChange pc:
                            ProcessPC(pos, pc);
                            break;
                        case Events.ChannelEvent ce:
                            ProcessAltPC(pos, ce);
                            break;
                    }
                }
            }
        }

        public static RawData ComposeRawData(this IScore source, int format, int resolution)
        {
            var c = source.TrackCount;
            RawData target = new(resolution, format);
            target.EnsureTrackCount(c);
            var targetTracks = target.Tracks;
            var whole = resolution * 4;

            // Copyright
            var text = source.Copyright;
            if (!string.IsNullOrEmpty(text))
            {
                targetTracks[0].Add(0, new Events.MetaText(MetaType.Copyright, text));
            }
            // tracks
            for (var i = 0; i < c; i++)
            {
                var sourceTrack = source.GetTrack(i);
                var targetTrack = targetTracks[i];
                text = sourceTrack.Title;
                if (!string.IsNullOrEmpty(text))
                {
                    targetTrack.Add(0, new Events.MetaText(MetaType.Title, text));
                }
                var port = sourceTrack.Port;
                if (port is >= 0)
                {
                    targetTrack.Add(0, new Events.Port(port));
                }
                sourceTrack.Timeline.ExtendToRawTimeline(targetTrack, sourceTrack.Channel, whole);
            }
            return target;
        }
    }
}
