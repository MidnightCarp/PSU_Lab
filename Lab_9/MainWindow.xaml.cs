using System;
using System.Windows;
using Lab9.Classes;

namespace Lab9
{
    public partial class MainWindow : Window
    {
        private Time _currentTime;

        public MainWindow()
        {
            InitializeComponent();
            // Инициализация начального времени при запуске
            _currentTime = new Time(12, 30);
            UpdateTimeDisplay();
        }

        // Приватный метод обновления отображения текущего времени
        private void UpdateTimeDisplay()
        {
            TxtCurrentTime.Text = $"Текущее время: {_currentTime}";
        }

        // Приватный метод очистки предыдущих результатов и ошибок
        private void ClearResults()
        {
            TxtError.Text = string.Empty;
            TxtAddResult.Text = "Результат: ";
            TxtUnaryResult.Text = "Результат: ";
            TxtCastResult.Text = "Результат: ";
            TxtBinaryResult.Text = "Результат: ";
        }

        // ================= ОБРАБОТЧИКИ СОБЫТИЙ =================

        private void BtnCreateTime_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                byte h = GetValidByte(TxtHours.Text, 0, 23, "Часы");
                byte m = GetValidByte(TxtMinutes.Text, 0, 59, "Минуты");
                _currentTime = new Time(h, m);
                UpdateTimeDisplay();
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnAddMinutes_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                uint addMins = GetValidUInt(TxtAddMinutes.Text, "Минуты для добавления");
                Time result = _currentTime.AddMinutes(addMins);
                TxtAddResult.Text = $"Результат: {result}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnIncrement_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                _currentTime = ++_currentTime;
                UpdateTimeDisplay();
                TxtUnaryResult.Text = $"Результат: {_currentTime}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnDecrement_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                _currentTime = --_currentTime;
                UpdateTimeDisplay();
                TxtUnaryResult.Text = $"Результат: {_currentTime}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnCastByte_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                byte extractedHours = (byte)_currentTime;
                TxtCastResult.Text = $"Результат (часы): {extractedHours}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnCastBool_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                bool isNotZero = _currentTime;
                TxtCastResult.Text = $"Результат (не ноль?): {isNotZero}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnPlusTimeUint_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                uint operand = GetValidUInt(TxtBinaryOperand.Text, "Операнд");
                Time result = _currentTime + operand;
                TxtBinaryResult.Text = $"Результат ({_currentTime} + {operand}): {result}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnPlusUintTime_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                uint operand = GetValidUInt(TxtBinaryOperand.Text, "Операнд");
                Time result = operand + _currentTime;
                TxtBinaryResult.Text = $"Результат ({operand} + {_currentTime}): {result}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnMinusTimeUint_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                uint operand = GetValidUInt(TxtBinaryOperand.Text, "Операнд");
                Time result = _currentTime - operand;
                TxtBinaryResult.Text = $"Результат ({_currentTime} - {operand}): {result}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BtnMinusUintTime_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            try
            {
                uint operand = GetValidUInt(TxtBinaryOperand.Text, "Операнд");
                Time result = operand - _currentTime;
                TxtBinaryResult.Text = $"Результат ({operand} - {_currentTime}): {result}";
            }
            catch (ArgumentException ex)
            {
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
        }

        // ================= ПРИВАТНЫЕ МЕТОДЫ ПРОВЕРКИ ВВОДА =================

        private byte GetValidByte(string input, byte min, byte max, string fieldName)
        {
            if (!byte.TryParse(input, out byte result))
            {
                throw new ArgumentException($"Поле '{fieldName}' должно содержать целое число.", fieldName);
            }

            if (result < min || result > max)
            {
                throw new ArgumentException($"Поле '{fieldName}' должно быть в диапазоне от {min} до {max}.", fieldName);
            }

            return result;
        }

        private uint GetValidUInt(string input, string fieldName)
        {
            if (!uint.TryParse(input, out uint result))
            {
                throw new ArgumentException($"Поле '{fieldName}' должно содержать неотрицательное целое число.", fieldName);
            }

            return result;
        }
    }
}