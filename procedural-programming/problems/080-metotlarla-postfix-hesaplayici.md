---
id: "080"
order: 80
title: "Metotlarla postfix ifade değerlendirme"
level: "İleri"
prerequisites: ["068", "069", "070", "073", "075"]
concepts: ["Birleşik prosedürel uygulama", "Belirteç ayrıştırma", "Diziyle yığın", "Try ve out sözleşmeleri", "Güvenli tamsayı aritmetiği"]
---

# Metotlarla postfix ifade değerlendirme

## Problem tanımı

Boşluklarla ayrılmış postfix ifadeyi tamsayı aritmetiğiyle değerlendirin. Postfix gösterimde işlem iki işlenenden sonra gelir: `8 3 - 2 *`, `(8 - 3) * 2` anlamındadır ve 10 üretir. Desteklenen işlemler `+`, `-`, `*`, `/` olur. Tek karakterlik işlem belirteçleri ile işaretli sayıları ayırın: `-` işlemdir, `-3` sayıdır.

Sayı belirteci, isteğe bağlı `+` veya `-` işaretinin ardından en az bir ASCII rakam içermelidir; tam `int` aralığına sığmalıdır. Başta sıfırlar kabul edilir. Belirteç ayırma, elle sayı ayrıştırma ve aritmetiği ayrı metotlara bölün. Diziyle yığın kullanın; hazır `Stack`, `Split`, düzenli ifade, `Parse` veya `TryParse` kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | İfade | 0..200 yazdırılabilir ASCII karakteri; ayraç yalnız U+0020 boşluk. |
| Ara değer | Yığın | `int` işlenenler; geçerli adet, kapasiteden ayrıdır. |
| Başarı | Sonuç | Tek satır `Sonuç: N`. |
| Hata | Neden | Boş ifade, eksik/artan işlenen, geçersiz belirteç, sıfıra bölme veya taşma. |

`İfade: ` istemini yanıtlayın. EOF, fazla uzunluk veya ASCII 32..126 dışı karakter için `Hata: İfade 0..200 yazdırılabilir ASCII karakteri içermelidir.` yazılır. Tam satır karakter kümesi açısından önce doğrulanır. Sonra belirteçler soldan sağa değerlendirilir; ilk değerlendirme hatası raporlanır. Sonunda tam bir işlenen kalmalıdır. Hiçbir ara sonuç yazdırılmaz.

Aralık dışı sayı belirteci `Hata: Geçersiz belirteç.` üretir; geçerli işlenenlerin işlemi `int` aralığını aşarsa `Hata: Taşma.` olur. Bölme sıfıra doğru kesilir: `-7 2 /` sonucu -3'tür. Parantez, ondalık sayı veya örtük işlem desteklenmez; her belirteç normal boşlukla ayrılmalıdır.

## Algoritma

1. Giriş uzunluğunu ve bütün karakterlerini doğrulayın.
2. `Hesapla` içinde boşlukları atlayıp her belirtecin başlangıç ve uzunluğunu bulun.
3. Tek karakterlik işlemi tanıyın; yığında iki işlenen yoksa eksik işlenen hatası dönün.
4. Önce sağ, sonra sol işleneni çıkarın; `Islem` ile hesaplayıp sonucu yığına koyun.
5. Diğer belirteçleri `Sayi` metoduyla elle ayrıştırın; geçersizse hata, geçerliyse yığına ekleme yapın.
6. Bitişte sıfır işlenen için boş ifade, birden fazla için artan işlenen hatası dönün.
7. Bir sonuç kaldıysa `out` ile döndürün ve ana programda yazdırın.

Yığının tepesi, sıradaki işlemde kullanılacak sağ işlenendir; çıkarma ve bölmede bu sıra önemlidir. İşlem `long` içinde yapılır, ardından `int` sınırı denetlenir. İki `int` değerinin çarpımı `long` aralığına sığar. Sayı ayrıştırmada çarpma/toplamadan önce büyüklük sınırı denetlenir. Giriş uzunluğundaki yığın kapasitesi belirteç sayısı için yeterlidir.

`8 3 - 2 *` için yığın; soldan sağa alt-üst sırası:

| Belirteç | Yığın | İşlem |
| --- | --- | --- |
| `8` | `8` | Sayıyı ekle. |
| `3` | `8, 3` | Sayıyı ekle. |
| `-` | `5` | Sol 8, sağ 3; fark 5. |
| `2` | `5, 2` | Sayıyı ekle. |
| `*` | `10` | Çarpım 10. |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("İfade: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: İfade 0..200 yazdırılabilir ASCII " +
        "karakteri içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    if (metin[i] < ' ' || metin[i] > '~')
    {
        Console.WriteLine("Hata: İfade 0..200 yazdırılabilir ASCII " +
            "karakteri içermelidir.");
        return;
    }
}

if (Hesapla(metin, out int sonuc, out string neden))
{
    Console.WriteLine("Sonuç: " + sonuc.ToString(CultureInfo.InvariantCulture));
}
else
{
    Console.WriteLine("Hata: " + neden + ".");
}

static bool Hesapla(string metin, out int sonuc, out string neden)
{
    sonuc = 0;
    neden = "";
    int[] yigin = new int[metin.Length];
    int i = 0, tepe = 0;
    while (i < metin.Length)
    {
        while (i < metin.Length && metin[i] == ' ')
        {
            i++;
        }
        if (i == metin.Length)
        {
            break;
        }

        int baslangic = i;
        while (i < metin.Length && metin[i] != ' ')
        {
            i++;
        }
        string belirtec = metin.Substring(baslangic, i - baslangic);
        if (IslemMi(belirtec))
        {
            if (tepe < 2)
            {
                neden = "Eksik işlenen";
                return false;
            }
            int sag = yigin[--tepe];
            int sol = yigin[--tepe];
            if (!Islem(belirtec[0], sol, sag, out int deger, out neden))
            {
                return false;
            }
            yigin[tepe++] = deger;
        }
        else
        {
            if (!Sayi(belirtec, out int deger))
            {
                neden = "Geçersiz belirteç";
                return false;
            }
            yigin[tepe++] = deger;
        }
    }

    if (tepe != 1)
    {
        neden = tepe == 0 ? "Boş ifade" : "Artan işlenen";
        return false;
    }
    sonuc = yigin[0];
    return true;
}

static bool IslemMi(string belirtec)
{
    return belirtec.Length == 1 && (belirtec[0] == '+' ||
        belirtec[0] == '-' || belirtec[0] == '*' || belirtec[0] == '/');
}

static bool Sayi(string metin, out int deger)
{
    deger = 0;
    int i = 0;
    bool negatif = metin[0] == '-';
    if (metin[0] == '-' || metin[0] == '+')
    {
        i++;
    }
    if (i == metin.Length)
    {
        return false;
    }

    long sinir = negatif ? 2147483648L : 2147483647L;
    long buyukluk = 0;
    for (; i < metin.Length; i++)
    {
        if (metin[i] < '0' || metin[i] > '9')
        {
            return false;
        }
        int rakam = metin[i] - '0';
        if (buyukluk > (sinir - rakam) / 10)
        {
            return false;
        }
        buyukluk = buyukluk * 10 + rakam;
    }
    deger = (int)(negatif ? -buyukluk : buyukluk);
    return true;
}

static bool Islem(char islem, int sol, int sag,
    out int sonuc, out string neden)
{
    sonuc = 0;
    neden = "";
    if (islem == '/' && sag == 0)
    {
        neden = "Sıfıra bölme";
        return false;
    }

    long genis = islem == '+' ? (long)sol + sag
        : islem == '-' ? (long)sol - sag
        : islem == '*' ? (long)sol * sag : (long)sol / sag;
    if (genis < int.MinValue || genis > int.MaxValue)
    {
        neden = "Taşma";
        return false;
    }
    sonuc = (int)genis;
    return true;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `Substring` yalnız zaten belirlenmiş karakter aralığını kopyalar; belirteç sınırlarını döngü bulur. `Sayi` boş olmayan belirteç, `Islem` dört desteklenen işlem önkoşuluyla çağrılır. Bu koşullar `Hesapla` tarafından sağlanır. Hata halinde `sonuc` kullanılmaz; `neden` yazdırılır. Hesaplama metotları konsola erişmez ve dış değişken yakalamaz.

## Örnek çalıştırmalar

Bloklar `İfade: ` isteminden sonraki tam sonuçtur.

`8 3 - 2 *`:

```text
Sonuç: 10
```

`-7 2 /`:

```text
Sonuç: -3
```

`+0042`:

```text
Sonuç: 42
```

`3 -`:

```text
Hata: Eksik işlenen.
```

`1 0 /`:

```text
Hata: Sıfıra bölme.
```

`2147483647 1 +`:

```text
Hata: Taşma.
```

## Sınır durumları

- Boş satır veya yalnız normal boşluk için `Hata: Boş ifade.` yazılır.
- `2 3` için `Hata: Artan işlenen.` yazılır; iki bağımsız sayı sonuç oluşturmaz.
- `2147483648` veya `2.5` için `Hata: Geçersiz belirteç.` yazılır.
- `-2147483648 -1 /` bölmesi `int` aralığını aşar; taşma hatasıdır.
- `3 8 -` sonucu -5'tir; yığından çıkarma sırası ters çevrilemez.
- Birden fazla, baştaki veya sondaki boşluk boş belirteç üretmez.
- Satır sonundaki sekme veya Türkçe harf, değerlendirme başlamadan giriş hatası üretir.
- İzin verilen uzunluk içinde baştaki sıfırlar sayı büyüklüğünü değiştirmez.

## Kazanımlar

- Birleşik bir uygulamayı ayrıştırma, değerlendirme ve aritmetik sözleşmelerine bölme.
- Yığınla işlem sırasını ve işlenen yönünü koruma.
- Aynı başarı/veri/hata aktarım biçimini metotlar arasında kullanma.
- Sayı belirteci hatası ile hesaplama taşmasını ayırma.
- Giriş doğrulamasını ve sonuç yazdırmayı hesaplamanın dışında yönetme.

## Alıştırmalar

1. Hatalı belirtecin sıfır tabanlı başlangıç dizinini ek bir `out` parametresiyle bildirin.
2. Kalan işlem `%` desteği ekleyin; sıfır sağ işlenen ve negatif değer davranışını tanımlayın.
3. Her işlemden sonraki yığın durumunu ayrı izleme modunda yazdırın; normal modda yalnız sonucun yazılmasını koruyun.
