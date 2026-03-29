using System;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace threadCipher.Views;

public partial class MainWindow : Window
{
    private const int KeyBitLength = 33;
    private const int BitsPreviewByteCount = 10;
    private const int KeyStreamPreviewBits = 64;

    private byte[]? _plainBytes;
    private byte[]? _resultBytes;
    private string _currentExtension = ".bin";
    private bool _keyTextInternalChange;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SetSaveButtonState(bool enabled)
    {
        SaveResultButton.IsEnabled = enabled;
        SaveResultButton.Background = enabled ? Brushes.LightGreen : null;
    }

    private void KeyTextBox_TextInput(object? sender, Avalonia.Input.TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text))
            return;

        if (!e.Text.All(c => c is '0' or '1'))
            e.Handled = true;
    }

    private void KeyTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (KeyTextBox is null || _keyTextInternalChange)
            return;

        var raw = KeyTextBox.Text ?? "";
        var filtered = new string(raw.Where(c => c is '0' or '1').Take(KeyBitLength).ToArray());

        if (raw != filtered)
        {
            _keyTextInternalChange = true;
            try
            {
                var caret = KeyTextBox.CaretIndex;
                KeyTextBox.Text = filtered;
                KeyTextBox.CaretIndex = Math.Min(Math.Max(0, caret - (raw.Length - filtered.Length)), filtered.Length);
            }
            finally
            {
                _keyTextInternalChange = false;
            }
        }

        UpdateKeyUiState();
    }

    private void UpdateKeyUiState()
    {
        if (KeyTextBox is null || KeyCountLabel is null || EncryptDecryptButton is null)
            return;

        var text = KeyTextBox.Text ?? "";
        var length = text.Length;

        KeyCountLabel.Text = $"Введено: {length} / {KeyBitLength}";

        if (length == KeyBitLength)
        {
            KeyCountLabel.Foreground = Brushes.Green;
            EncryptDecryptButton.IsEnabled = _plainBytes is { Length: > 0 };
        }
        else
        {
            KeyCountLabel.Foreground = Brushes.Red;
            EncryptDecryptButton.IsEnabled = false;
        }
    }

    private void InputTextBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (sender is not TextBox textBox || string.IsNullOrEmpty(textBox.Text))
            return;

        _plainBytes = Encoding.UTF8.GetBytes(textBox.Text);
        UpdateBitsPreview();
        SetSaveButtonState(false);
        UpdateKeyUiState();
    }

    private async void OpenFileButton_Click(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите файл",
            AllowMultiple = false
        });

        if (files.Count == 0)
            return;

        var file = files[0];
        _currentExtension = Path.GetExtension(file.Name);

        await using var stream = await file.OpenReadAsync();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        _plainBytes = ms.ToArray();
        InputTextBox.Text = $"[Файл: {file.Name}]";
        SetSaveButtonState(false);
        UpdateBitsPreview();
        UpdateKeyUiState();
    }

    private void EncryptDecryptButton_Click(object? sender, RoutedEventArgs e)
    {
        var seed = KeyTextBox.Text ?? "";

        if (seed.Length != KeyBitLength)
        {
            LfsrBitsBox.Text = $"ОШИБКА: Ключ должен быть ровно {KeyBitLength} бит!";
            return;
        }

        if (_plainBytes is null || _plainBytes.Length == 0)
            return;

        var cipher = new LfsrCipher();
        var (result, lfsrText) = cipher.Execute(_plainBytes, seed);

        _resultBytes = result;
        LfsrBitsBox.Text = lfsrText.Length > KeyStreamPreviewBits
            ? lfsrText[..KeyStreamPreviewBits] + "..."
            : lfsrText + "...";

        UpdateResultPreview();
        SetSaveButtonState(true);
    }

    private async void SaveResultButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_resultBytes is null || _resultBytes.Length == 0)
            return;

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Сохранить результат",
            DefaultExtension = _currentExtension,
            SuggestedFileName = "result_encrypted" + _currentExtension
        });

        if (file is null)
            return;

        await using var stream = await file.OpenWriteAsync();
        await stream.WriteAsync(_resultBytes.AsMemory(0, _resultBytes.Length));
    }

    private void UpdateBitsPreview()
    {
        if (_plainBytes is null || _plainBytes.Length == 0)
            return;

        var previewBytes = _plainBytes.Take(BitsPreviewByteCount).ToArray();
        var bits = string.Join(" ", previewBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

        if (_plainBytes.Length > BitsPreviewByteCount)
            bits += "...";

        BitsPreviewBox.Text = bits;
    }

    private void UpdateResultPreview()
    {
        if (_resultBytes is null || _resultBytes.Length == 0)
            return;

        var previewBytes = _resultBytes.Take(BitsPreviewByteCount).ToArray();
        var bits = string.Join(" ", previewBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

        if (_resultBytes.Length > BitsPreviewByteCount)
            bits += "...";

        ResultBitsBox.Text = bits;
    }
}
