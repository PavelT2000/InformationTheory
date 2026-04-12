using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RsaLab.Rsa;

namespace RsaLab.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const int MaxDecimalPreviewChars = 120_000;

    public TopLevel? TopLevelHost { get; set; }

    [ObservableProperty] private string _pText = "7";
    [ObservableProperty] private string _qText = "61";
    [ObservableProperty] private string _dText = "7";

    [ObservableProperty] private string _rText = "—";
    [ObservableProperty] private string _phiText = "—";
    [ObservableProperty] private string _eText = "—";

    [ObservableProperty] private string _keysStatus = "Введите p, q и d, затем нажмите «Вычислить e и проверить ключи».";

    [ObservableProperty] private string _encryptInputPath = "";
    [ObservableProperty] private string _encryptOutputPath = "";
    [ObservableProperty] private string _encryptDecimalOutput = "";
    [ObservableProperty] private string _encryptStatus = "";

    [ObservableProperty] private string _decryptInputPath = "";
    [ObservableProperty] private string _decryptOutputPath = "";
    [ObservableProperty] private string _decryptStatus = "";

    [ObservableProperty] private string _primitiveRootsP = "31";
    [ObservableProperty] private string _primitiveRootsResult = "";

    private int? _r;
    private int? _phi;
    private int? _e;
    private int? _d;

    partial void OnPrimitiveRootsPChanged(string value) => RefreshPrimitiveRoots();

    public MainWindowViewModel()
    {
        RefreshPrimitiveRoots();
    }

    private void RefreshPrimitiveRoots()
    {
        if (!int.TryParse(PrimitiveRootsP, out var p) || p < 2)
        {
            PrimitiveRootsResult = "Введите целое p ≥ 2.";
            return;
        }

        if (!RsaMath.IsPrime(p))
        {
            PrimitiveRootsResult = $"Число {p} не является простым.";
            return;
        }

        var roots = PrimitiveRootFinder.FindPrimitiveRoots(p);
        var phi = p - 1;
        var factors = string.Join(", ", PrimitiveRootFinder.DistinctPrimeFactors(phi));
        PrimitiveRootsResult =
            $"φ({p}) = {phi} = разложение на простые множители: {{{factors}}}.\n" +
            $"Проверка: g — первообразный корень, если для каждого простого q|φ({p}) выполняется g^(φ/q) ≢ 1 (mod {p}).\n\n" +
            $"Первообразные корни по модулю {p}: {string.Join(", ", roots)} " +
            $"(всего {roots.Count}).";
    }

    [RelayCommand]
    private void ComputeKeys()
    {
        KeysStatus = "";

        if (!int.TryParse(PText, out var p) || !int.TryParse(QText, out var q))
        {
            KeysStatus = "p и q должны быть целыми числами.";
            ClearKeyOutputs();
            return;
        }

        if (!RsaMath.IsPrime(p))
        {
            KeysStatus = $"Число p = {p} не простое. Введите другое p.";
            ClearKeyOutputs();
            return;
        }

        if (!RsaMath.IsPrime(q))
        {
            KeysStatus = $"Число q = {q} не простое. Введите другое q.";
            ClearKeyOutputs();
            return;
        }

        if (p == q)
        {
            KeysStatus = "p и q должны быть различными простыми.";
            ClearKeyOutputs();
            return;
        }

        var r = checked(p * q);
        if (!RsaMath.IsValidModule(r))
        {
            KeysStatus = $"r = p·q = {r}. {RsaMath.FormatModuleConstraint()}";
            ClearKeyOutputs();
            return;
        }

        var phi = (p - 1) * (q - 1);

        if (!int.TryParse(DText, out var d))
        {
            KeysStatus = "d должно быть целым числом.";
            ClearKeyOutputs();
            return;
        }

        if (d <= 1 || d >= phi)
        {
            KeysStatus = $"d должно удовлетворять 1 < d < φ(r) = {phi}.";
            ClearKeyOutputs();
            return;
        }

        if (RsaMath.Gcd(d, phi) != 1)
        {
            KeysStatus = $"d и φ(r) = {phi} должны быть взаимно просты (НОД ≠ 1).";
            ClearKeyOutputs();
            return;
        }

        int e;
        try
        {
            e = RsaMath.ModInverse(d, phi);
        }
        catch (InvalidOperationException ex)
        {
            KeysStatus = ex.Message;
            ClearKeyOutputs();
            return;
        }

        _r = r;
        _phi = phi;
        _d = d;
        _e = e;

        RText = r.ToString();
        PhiText = phi.ToString();
        EText = e.ToString();
        KeysStatus =
            $"Ключи готовы: r = {r}, φ(r) = {phi}, закрытый d = {d}, открытый e = {e} (e·d ≡ 1 mod φ, через расширенный алгоритм Евклида). " +
            $"Можно шифровать и расшифровывать файлы.";
    }

    private void ClearKeyOutputs()
    {
        _r = _phi = _e = _d = null;
        RText = PhiText = EText = "—";
    }

    private bool EnsureKeysReady()
    {
        if (_r is null || _e is null || _d is null)
        {
            KeysStatus = "Сначала вычислите ключи на вкладке «Ключи».";
            return false;
        }
        return true;
    }

    [RelayCommand]
    private async Task BrowseEncryptInputAsync()
    {
        var path = await OpenFileAsync("Файл для шифрования");
        if (path is not null) EncryptInputPath = path;
    }

    [RelayCommand]
    private async Task BrowseEncryptOutputAsync()
    {
        var path = await SaveFileAsync("Зашифрованный файл (бинарный)", "enc");
        if (path is not null) EncryptOutputPath = path;
    }

    [RelayCommand]
    private async Task BrowseDecryptInputAsync()
    {
        var path = await OpenFileAsync("Зашифрованный файл");
        if (path is not null) DecryptInputPath = path;
    }

    [RelayCommand]
    private async Task BrowseDecryptOutputAsync()
    {
        var path = await SaveFileAsync("Расшифрованный файл", "bin");
        if (path is not null) DecryptOutputPath = path;
    }

    [RelayCommand]
    private void RunEncrypt()
    {
        EncryptStatus = "";
        EncryptDecimalOutput = "";

        if (!EnsureKeysReady()) return;

        if (string.IsNullOrWhiteSpace(EncryptInputPath) || !File.Exists(EncryptInputPath))
        {
            EncryptStatus = "Укажите существующий входной файл.";
            return;
        }

        if (string.IsNullOrWhiteSpace(EncryptOutputPath))
        {
            EncryptStatus = "Укажите путь для сохранения зашифрованного файла.";
            return;
        }

        try
        {
            var crypto = new RsaFileCrypto(_r!.Value, _e!.Value, _d!.Value);
            var (_, preview) = crypto.EncryptFile(EncryptInputPath, EncryptOutputPath);

            if (preview.Length > MaxDecimalPreviewChars)
            {
                EncryptDecimalOutput = preview.AsSpan(0, MaxDecimalPreviewChars).ToString() +
                                       $"\n\n… (показаны первые {MaxDecimalPreviewChars} символов; полная последовательность в файле как байты.)";
            }
            else
            {
                EncryptDecimalOutput = preview;
            }

            EncryptStatus = $"Готово: зашифровано в бинарный файл (2 байта на исходный байт, big-endian). Десятичные C — на экране.";
        }
        catch (Exception ex)
        {
            EncryptStatus = ex.Message;
        }
    }

    [RelayCommand]
    private void RunDecrypt()
    {
        DecryptStatus = "";

        if (!EnsureKeysReady()) return;

        if (string.IsNullOrWhiteSpace(DecryptInputPath) || !File.Exists(DecryptInputPath))
        {
            DecryptStatus = "Укажите существующий зашифрованный файл.";
            return;
        }

        if (string.IsNullOrWhiteSpace(DecryptOutputPath))
        {
            DecryptStatus = "Укажите путь для сохранения расшифрованного файла.";
            return;
        }

        try
        {
            var crypto = new RsaFileCrypto(_r!.Value, _e!.Value, _d!.Value);
            crypto.DecryptFile(DecryptInputPath, DecryptOutputPath);
            DecryptStatus = "Расшифрование выполнено.";
        }
        catch (Exception ex)
        {
            DecryptStatus = ex.Message;
        }
    }

    private async Task<string?> OpenFileAsync(string title)
    {
        if (TopLevelHost is null) return null;

        var options = new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
        };

        var result = await TopLevelHost.StorageProvider.OpenFilePickerAsync(options);
        if (result.Count == 0) return null;

        return result[0].Path.LocalPath;
    }

    private async Task<string?> SaveFileAsync(string title, string suggestedExtension)
    {
        if (TopLevelHost is null) return null;

        var file = await TopLevelHost.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            DefaultExtension = suggestedExtension,
        });

        return file?.Path.LocalPath;
    }
}
