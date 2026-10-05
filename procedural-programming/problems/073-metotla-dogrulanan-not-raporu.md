---
id: "073"
order: 73
title: "Tekrar kullanılabilir giriş metoduyla not raporu"
level: "İleri"
prerequisites: ["010", "021", "071", "072"]
concepts: ["out parametresi", "Try sözleşmesi", "Sınırlı yeniden deneme", "EOF", "Doğrulama ve hesaplama ayrımı"]
---

# Tekrar kullanılabilir giriş metoduyla not raporu

## Problem tanımı

1..10 öğrenci için 0..100 aralığındaki notları okuyup ortalamayı ve en az 60 alan öğrenci sayısını bulun. Her sayısal alan için en fazla üç giriş denemesi yapın. Yanlış girişten sonra aynı alan tekrar sorulur; EOF durumunda yeni deneme yapılmadan program sonlandırılır. Üç deneme de başarısızsa rapor oluşturulmaz.

Okuma ve aralık doğrulamasını `Oku(string istem, int alt, int ust, out int deger)` metodunda birleştirin. Başarı `true` dönüşüyle, okunan değer `out` parametresiyle verilir. Başarısızlıkta değer 0 olarak atanır ve kullanılmaz. Raporun hesaplama metotları konsola erişmez. Böylece giriş metodunun bilinçli kullanıcı etkileşimi ile hesaplama metotlarının yan etkisiz davranışı ayrılır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | Öğrenci sayısı | 1..10; her alan en fazla üç deneme. |
| Girdi | Notlar | Her öğrenci için ayrı satırda 0..100 tamsayı. |
| Ara çıktı | Yeniden deneme | `Uyarı: Geçersiz giriş.`; hatalı her denemede yazılır. |
| Sonuç | Rapor | `Ortalama: X` iki ondalık; `Geçen: N`. |

İstemler `Öğrenci sayısı: `, sonra `Not[0]: ` ile başlayan istemlerdir. Son hatalı deneme de uyarı üretir; ardından `Hata: Giriş tamamlanamadı.` yazılır. EOF yalnız bu hata satırını üretir; uyarı yoktur. Önceden alınmış notlar için kısmi rapor yazılmaz. Tamsayı okuma çevre boşlukları ve işaret kabul eder; ondalıklı değer kabul etmez.

## Algoritma

1. `Oku` ile öğrenci sayısını alın; başarısızsa programı bitirin.
2. Her not için aynı metodu farklı istem ve sınırlarla çağırın.
3. `Oku` içinde üç denemeyi sayın; EOF'ta veya başarılı ayrıştırmada hemen dönün.
4. Başarısız bir denemede uyarı yazıp kalan deneme varsa aynı alanı yeniden sorun.
5. Bütün notlar alınınca `Ortalama` ve `GecenSayisi` metotlarını çağırın.
6. Nokta ayırıcıyla iki ondalıklı ortalamayı ve geçen sayısını yazdırın.

`out`, metodun normal dönüş yollarının hepsinde bir değer atamasını gerektirir. 0 geçerli bir not da olabilir; başarıyı değerin 0 olmasıyla değil `bool` dönüşle kontrol edin. `Ortalama` metodunun önkoşulu boş olmayan, doğrulanmış not dizisidir. En fazla 33 okuma yapılır: öğrenci sayısı için üç ve on notun her biri için üç deneme.

İki öğrenci için `x`, `2`, `101`, `50`, `80` girişlerinin akışı:

| Alan | Deneme girdisi | İşlem |
| --- | --- | --- |
| Öğrenci sayısı | `x`, sonra `2` | Uyarı; ikinci deneme kabul edilir. |
| Not 0 | `101`, sonra `50` | Uyarı; ikinci deneme kabul edilir. |
| Not 1 | `80` | İlk denemede kabul edilir. |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Öğrenci sayısı: ", 1, 10, out int n))
{
    Console.WriteLine("Hata: Giriş tamamlanamadı.");
    return;
}
int[] notlar = new int[n];
for (int i = 0; i < n; i++)
{
    string istem = "Not[" + i.ToString(CultureInfo.InvariantCulture) + "]: ";
    if (!Oku(istem, 0, 100, out notlar[i]))
    {
        Console.WriteLine("Hata: Giriş tamamlanamadı.");
        return;
    }
}

Console.WriteLine("Ortalama: " +
    Ortalama(notlar).ToString("0.00", CultureInfo.InvariantCulture));
Console.WriteLine("Geçen: " +
    GecenSayisi(notlar).ToString(CultureInfo.InvariantCulture));

static bool Oku(string istem, int alt, int ust, out int deger)
{
    deger = 0;
    for (int deneme = 0; deneme < 3; deneme++)
    {
        Console.Write(istem);
        string? satir = Console.ReadLine();
        if (satir is null)
        {
            return false;
        }
        if (int.TryParse(satir, NumberStyles.Integer,
            CultureInfo.InvariantCulture, out int aday) &&
            aday >= alt && aday <= ust)
        {
            deger = aday;
            return true;
        }
        Console.WriteLine("Uyarı: Geçersiz giriş.");
    }
    return false;
}

static decimal Ortalama(int[] notlar)
{
    int toplam = 0;
    for (int i = 0; i < notlar.Length; i++)
    {
        toplam += notlar[i];
    }
    return (decimal)toplam / notlar.Length;
}

static int GecenSayisi(int[] notlar)
{
    int adet = 0;
    for (int i = 0; i < notlar.Length; i++)
    {
        if (notlar[i] >= 60)
        {
            adet++;
        }
    }
    return adet;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `out notlar[i]`, kabul edilen değeri doğrudan o dizi hücresine aktarır. Başarısız dönüşte program hücreyi rapora katmaz. `Oku` yalnız giriş işi yapar; ortalama veya geçme kuralını bilmez. Her çağrıda yerel deneme sayacı yeniden 0'dan başlar.

## Örnek çalıştırmalar

İstemler konsolda aynı satıra yazılır; aşağıdaki bloklar istemler çıkarıldığında kalan bütün uyarı ve sonuç satırlarıdır. Verilen giriş sırası ayrı satırlarda uygulanır.

`2, 50, 80`:

```text
Ortalama: 65.00
Geçen: 1
```

`x, 2, 101, 50, 80`:

```text
Uyarı: Geçersiz giriş.
Uyarı: Geçersiz giriş.
Ortalama: 65.00
Geçen: 1
```

`1, 60`:

```text
Ortalama: 60.00
Geçen: 1
```

Öğrenci sayısına `0, 11, x`:

```text
Uyarı: Geçersiz giriş.
Uyarı: Geçersiz giriş.
Uyarı: Geçersiz giriş.
Hata: Giriş tamamlanamadı.
```

`2, 100`, ardından EOF:

```text
Hata: Giriş tamamlanamadı.
```

## Sınır durumları

- Not 0 geçerlidir; başarısızlığın göstergesi değildir.
- Not 60 geçer; 59 geçmez. Not 100 üst sınırdır.
- Üçüncü denemede geçerli giriş kabul edilir; dördüncü deneme yoktur.
- Boş satır geçersiz bir denemedir; EOF hemen bitiştir.
- Öğrenci sayısı en az 1 olduğundan ortalamada sıfıra bölme oluşmaz.
- Toplam en fazla 1000 olur; `decimal` bölme küsuratı korur.

## Kazanımlar

- Başarı bilgisini dönüş değeriyle, veriyi `out` ile aktarma.
- Parametrelerle aynı giriş davranışını farklı alanlarda kullanma.
- EOF ile yanlış kullanıcı girdisini ayrı yönetme.
- Yeniden deneme sınırını her alan için bağımsız tutma.
- Doğrulanmamış veya eksik veriyle rapor üretmemeyi sağlama.

## Alıştırmalar

1. Deneme sınırını parametre yapın; sıfır veya negatif sınır için sözleşme belirleyin.
2. `GecenSayisi` metoduna geçme eşiği parametresi ekleyin.
3. İlk denemede kabul edilen alanları sayın; bu bilgiyi ek bir `out` parametresiyle döndürün.
