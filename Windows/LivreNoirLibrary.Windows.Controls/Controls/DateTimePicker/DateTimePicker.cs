using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class DateTimePicker : Control
    {
        const string PART_Date = nameof(PART_Date);
        const string PART_Hour = nameof(PART_Hour);
        const string PART_Minute = nameof(PART_Minute);
        const string PART_Now = nameof(PART_Now);

        public static readonly DateTime DefaultDateTime = DateTime.MinValue;
        public static readonly DateTime DefaultStart = DefaultDateTime;
        public static readonly DateTime DefaultEnd = new(2099, 12, 31, 23, 59, 59);
        public static readonly int DefaultHour = DefaultDateTime.Hour;
        public static readonly int DefaultMinute = DefaultDateTime.Minute;
        static DateTimePicker()
        {
            PropertyUtils.OverrideDefaultStyleKey<DateTimePicker>();
        }

        public static readonly DependencyProperty DisplayDateStartProperty = DatePicker.DisplayDateStartProperty.AddOwner(typeof(DateTimePicker), DefaultStart, OnDateStartChanged);
        public static readonly DependencyProperty DisplayDateEndProperty = DatePicker.DisplayDateEndProperty.AddOwner(typeof(DateTimePicker), DefaultEnd, OnDateEndChanged);

        private static void OnDateStartChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DateTimePicker p)
            {
                p._start = ((DateTime)e.NewValue).Date;
            }
        }

        private static void OnDateEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DateTimePicker p)
            {
                p._end = (DateTime)e.NewValue;
            }
        }

        private DateTime _start = DefaultStart;
        private DateTime _end = DefaultEnd;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private DateTime _selectedDateTime = DefaultDateTime;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private DateTime _selectedDate = DefaultDateTime.Date;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _selectedHour = DefaultHour;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _selectedMinute = DefaultMinute;

        public DateTime DisplayDateStart { get => _start; set => SetValue(DisplayDateStartProperty, value.Date); }
        public DateTime DisplayDateEnd { get => _end; set => SetValue(DisplayDateEndProperty, value.Date); }

        private bool _value_changing;

        private void OnSelectedDateTimeChanged(DateTime value)
        {
            if (value < _start)
            {
                SetValue(SelectedDateTimeProperty, _start);
            }
            else if (value > _end)
            {
                SetValue(SelectedDateTimeProperty, _end);
            }
            else
            {
                ApplyFromDateTime();
            }
        }

        private void OnSelectedDateChanged(DateTime value)
        {
            _selectedDate = value.Date;
            ApplyToDateTime();
        }

        private void OnSelectedHourChanged(int value)
        {
            ApplyToDateTime();
        }

        private void OnSelectedMinuteChanged(int value)
        {
            ApplyToDateTime();
        }

        private void ApplyFromDateTime()
        {
            if (!_value_changing)
            {
                _value_changing = true;
                SelectedDate = _selectedDateTime.Date;
                SelectedHour = _selectedDateTime.Hour;
                SelectedMinute = _selectedDateTime.Minute;
                _value_changing = false;
            }
        }

        private void ApplyToDateTime()
        {
            if (!_value_changing)
            {
                _value_changing = true;
                SelectedDateTime = new(_selectedDate.Year, _selectedDate.Month, _selectedDate.Day, _selectedHour, _selectedMinute, 0);
                _value_changing = false;
            }
        }

        private DatePicker? _selector_date;
        private ComboBox? _selector_hour;
        private ComboBox? _selector_minute;
        private ButtonBase? _button_now;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_selector_date is not null) _selector_date.PreviewMouseWheel -= OnPreviewMouseWheel_Date;
            if (_selector_hour is not null) _selector_hour.PreviewMouseWheel -= OnPreviewMouseWheel_Hour;
            if (_selector_minute is not null) _selector_minute.PreviewMouseWheel -= OnPreviewMouseWheel_Minute;
            if (_button_now is not null) _button_now.Click -= OnClick_Now;

            _selector_date = GetTemplateChild(PART_Date) as DatePicker;
            _selector_hour = GetTemplateChild(PART_Hour) as ComboBox;
            _selector_minute = GetTemplateChild(PART_Minute) as ComboBox;
            _button_now = GetTemplateChild(PART_Now) as ButtonBase;

            if (_selector_date is not null)
            {
                _selector_date.PreviewMouseWheel += OnPreviewMouseWheel_Date;
            }
            if (_selector_hour is not null)
            {
                _selector_hour.PreviewMouseWheel += OnPreviewMouseWheel_Hour;
                _selector_hour.ItemsSource = TimeItem.Hours;
            }
            if (_selector_minute is not null)
            {
                _selector_minute.PreviewMouseWheel += OnPreviewMouseWheel_Minute;
                _selector_minute.ItemsSource = TimeItem.Minutes;
            }
            if (_button_now is not null)
            {
                _button_now.Click += OnClick_Now;
            }
        }

        private void OnPreviewMouseWheel_Date(object sender, MouseWheelEventArgs e)
        {
            if (_selector_date?.IsDropDownOpen is false)
            {
                SelectedDateTime = _selectedDateTime.AddDays(e.Delta > 0 ? 1 : -1);
                e.Handled = true;
            }
        }

        private void PreviewMouseWheel_General(ComboBox? selector, Func<double, DateTime> func, MouseWheelEventArgs e)
        {
            if (selector?.IsDropDownOpen is false)
            {
                SelectedDateTime = func(e.Delta > 0 ? 1 : -1);
                e.Handled = true;
            }
        }

        private void OnPreviewMouseWheel_Hour(object sender, MouseWheelEventArgs e) => PreviewMouseWheel_General(_selector_hour, _selectedDateTime.AddHours, e);
        private void OnPreviewMouseWheel_Minute(object sender, MouseWheelEventArgs e) => PreviewMouseWheel_General(_selector_minute, _selectedDateTime.AddMinutes, e);

        private void OnClick_Now(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = DateTime.Now;
        }
    }
}
