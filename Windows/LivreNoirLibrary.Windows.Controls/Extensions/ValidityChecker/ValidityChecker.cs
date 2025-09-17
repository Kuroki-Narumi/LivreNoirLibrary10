using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class ValidityChecker
    {
        private readonly ValidityChangedEventHandler? _handler;
        private readonly Dictionary<object, bool> _list = [];
        private int _valid_count;
        private bool _last_valid;

        public bool IsValid => _valid_count == _list.Count;

        public ValidityChecker(ValidityChangedEventHandler? handler, params ReadOnlySpan<object> controls)
        {
            _handler = handler;
            foreach (var control in controls)
            {
                bool flag;
                if (control is DefaultTextBox t)
                {
                    flag = t.IsTextValid;
                    t.TextChanged += CheckT;
                }
                else if (control is IValidityCheck s)
                {
                    flag = s.IsInputValid;
                    s.ValidityChanged += CheckS;
                }
                else
                {
                    continue;
                }
                _list.Add(control, flag);
                if (flag)
                {
                    _valid_count++;
                }
            }
            _last_valid = IsValid;
        }

        private void CheckObject(object sender, bool current)
        {
            if (_list.TryGetValue(sender, out var last))
            {
                _list[sender] = current;
                if (current != last)
                {
                    if (current)
                    {
                        _valid_count++;
                    }
                    else
                    {
                        _valid_count--;
                    }
                }
            }
        }

        private void Check()
        {
            var flag = IsValid;
            if (flag != _last_valid)
            {
                _last_valid = flag;
                _handler?.Invoke(this, new() { IsValid = flag });
            }
        }

        private void CheckT(object sender, TextChangedEventArgs e)
        {
            if (sender is DefaultTextBox t)
            {
                CheckObject(sender, t.IsTextValid);
            }
            Check();
        }

        private void CheckS(object sender, ValidityChangedEventArgs e)
        {
            CheckObject(sender, e.IsValid);
            Check();
        }
    }
}
