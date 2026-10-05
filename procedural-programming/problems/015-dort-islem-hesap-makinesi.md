---
id: "015"
order: 15
title: "Dört işlem hesap makinesi"
level: "Orta"
prerequisites: ["001", "003", "012", "014"]
concepts: ["Switch", "İşlem seçimi", "Decimal", "Sıfıra bölme", "Taşma", "Çıktı biçimlendirme"]
---

# Dört işlem hesap makinesi

## Problem tanımı

Kullanıcının girdiği iki ondalıklı sayı üzerinde seçilen tek bir işlemi uygulayan hesap makinesi yazın. Desteklenen işlemler toplama `+`, çıkarma `-`, çarpma `*` ve bölme `/` işlemleridir. Çıkarma ve bölmede sıra önemlidir: ilk sayıdan ikinci sayı çıkarılır veya ilk sayı ikinci sayıya bölünür.

Her sayı -1000000 ile 1000000 arasında olmalıdır. Bölmede ikinci sayı sıfır olamaz. Desteklenmeyen bir işlem işareti girilirse hesaplama yapılmaz. Bu sürüm tek işlem tamamlandıktan sonra sona erer; yeniden işlem istemez.

Ondalık değerleri saklamak için `decimal` kullanılır. Sonuç ekranda iki ondalık basamakla gösterilir. Örneğin 1'in 3'e bölümü `0.33` olarak yazılır; hesaplanan değer gösterimden önce iki basamağa yuvarlanmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `ilkSayi` | -1000000 ile 1000000 arasında `decimal` değer. |
| Girdi | `ikinciSayi` | Aynı aralıkta `decimal` değer; bölmede sıfır olamaz. |
| Girdi | `islem` | Tam olarak `+`, `-`, `*` veya `/` metni. |
| Çıktı | `sonuc` | `Sonuç: ` ile başlayan, iki ondalık basamaklı değer. |

`İlk sayı: `, `İkinci sayı: ` ve `İşlem (+, -, *, /): ` istemlerine sırayla ayrı satırlarda yanıt verin. Ondalık ayırıcı olarak nokta kullanın: `2.5` geçerlidir, `2,5` geçersizdir. Sayıların başında isteğe bağlı `+` veya `-` olabilir. Basamak ayırıcı, bilimsel gösterim ve girişin başında veya sonunda boşluk kabul edilmez. İşlem işaretinin yanında da boşluk yazmayın.

## Algoritma

1. İlk sayıyı okuyun, biçimini ve aralığını doğrulayın. Geçersizse hata yazıp bitirin.
2. İkinci sayıyı okuyun, aynı biçimde doğrulayın.
3. İşlem işaretini okuyun.
4. `switch` ile işlem işaretine uygun dalı seçin.
5. `+` için toplayın, `-` için ilk sayıdan ikinciyi çıkarın, `*` için çarpın.
6. `/` dalında ikinci sayı sıfırsa hata yazıp bitirin; değilse bölün. Bölüm `decimal` türüne sığmazsa hata yazıp bitirin.
7. İşaret desteklenmiyorsa hata yazıp bitirin.
8. Sonucu nokta ondalık ayırıcıyla ve iki ondalık basamakla yazdırın.

`switch`, tek bir değerin belirli seçeneklerden hangisine eşit olduğunu denetler. `case` ilgili seçeneği, `default` seçeneklerin hiçbirine uymayan girdiyi temsil eder. Her başarılı dalın sonundaki `break`, `switch` işleminden çıkılmasını sağlar; program ortak çıktı satırına devam eder.

## C# çözümü

```csharp
using System;
using System.Globalization;
NumberStyles sayiBicimi = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;

Console.Write("İlk sayı: ");
if (!decimal.TryParse(Console.ReadLine(), sayiBicimi,
    CultureInfo.InvariantCulture, out decimal ilkSayi) ||
    ilkSayi < -1000000m || ilkSayi > 1000000m)
{
    Console.WriteLine("Hata: İlk sayı -1000000 ile 1000000 arasında olmalıdır.");
    return;
}

Console.Write("İkinci sayı: ");
if (!decimal.TryParse(Console.ReadLine(), sayiBicimi,
    CultureInfo.InvariantCulture, out decimal ikinciSayi) ||
    ikinciSayi < -1000000m || ikinciSayi > 1000000m)
{
    Console.WriteLine("Hata: İkinci sayı -1000000 ile 1000000 arasında olmalıdır.");
    return;
}

Console.Write("İşlem (+, -, *, /): ");
string? islem = Console.ReadLine();
decimal sonuc;

switch (islem)
{
    case "+":
        sonuc = ilkSayi + ikinciSayi;
        break;
    case "-":
        sonuc = ilkSayi - ikinciSayi;
        break;
    case "*":
        sonuc = ilkSayi * ikinciSayi;
        break;
    case "/":
        if (ikinciSayi == 0m)
        {
            Console.WriteLine("Hata: Sıfıra bölme yapılamaz.");
            return;
        }
        try
        {
            sonuc = ilkSayi / ikinciSayi;
        }
        catch (OverflowException)
        {
            Console.WriteLine("Hata: Bölme sonucu decimal türüne sığmıyor.");
            return;
        }
        break;
    default:
        Console.WriteLine("Hata: İşlem +, -, * veya / olmalıdır.");
        return;
}
Console.WriteLine("Sonuç: " + sonuc.ToString("F2", CultureInfo.InvariantCulture));
```

Kod, bir konsol projesinin `Program.cs` dosyasına tek başına konabilir. Sayısal sabitlerdeki `m` eki `decimal` türünü belirtir. `NumberStyles` seçeneklerini birleştiren `|`, burada kabul edilen sayı biçimlerini seçer; koşullarda kullanılan `||` ile aynı işleç değildir. `string?`, giriş akışı sona erdiğinde işlem metninin `null` olabileceğini belirtir; bu değer de `default` dalında reddedilir.

`F2`, yalnızca çıktı metnini iki ondalık basamağa yuvarlar. `decimal` sonlu hassasiyete sahiptir; 1/3 gibi sonsuz ondalık açılımlar bu türde de tam olarak saklanamaz. Çözüm dört işlemi doğrudan uygular, hesaplamaları iki ondalık basamakla sınırlayan ek bir yuvarlama yapmaz.

Çok küçük bir bölen, girdi aralığı geçerli olsa bile saklanamayacak kadar büyük bir bölüm oluşturabilir. `try` içinde bölme denenir; böyle bir taşmada `catch (OverflowException)` hata mesajı yazıp programı bitirir. Toplama, çıkarma ve çarpma ise belirtilen girdi sınırlarıyla türün kapasitesi içinde kalır.

## Örnek çalıştırmalar

Tablo, istemleri hariç tutarak yazdırılan tam satırı gösterir. `—`, hata nedeniyle henüz istenmeyen girdidir.

| İlk sayı | İkinci sayı | İşlem | Sonuç satırı |
| --- | --- | --- | --- |
| `2.5` | `1.25` | `+` | `Sonuç: 3.75` |
| `2` | `5` | `-` | `Sonuç: -3.00` |
| `-4` | `2.5` | `*` | `Sonuç: -10.00` |
| `7` | `2` | `/` | `Sonuç: 3.50` |
| `1` | `3` | `/` | `Sonuç: 0.33` |
| `0` | `5` | `/` | `Sonuç: 0.00` |
| `1000000` | `1000000` | `*` | `Sonuç: 1000000000000.00` |
| `5` | `0` | `/` | `Hata: Sıfıra bölme yapılamaz.` |
| `5` | `-0.0` | `/` | `Hata: Sıfıra bölme yapılamaz.` |
| `5` | `2` | `%` | `Hata: İşlem +, -, * veya / olmalıdır.` |
| `2,5` | `—` | `—` | `Hata: İlk sayı -1000000 ile 1000000 arasında olmalıdır.` |
| `5` | `1000000.01` | `—` | `Hata: İkinci sayı -1000000 ile 1000000 arasında olmalıdır.` |
| `1e3` | `—` | `—` | `Hata: İlk sayı -1000000 ile 1000000 arasında olmalıdır.` |
| `1000000` | `0.0000000000000000000000000001` | `/` | `Hata: Bölme sonucu decimal türüne sığmıyor.` |

## Sınır durumları

- -1000000 ve 1000000 dahil edilir. Toplama, çıkarma ve çarpma sonuçları bu girdi aralığının dışında olabilir; çıktı aynı aralıkla sınırlanmaz.
- En büyük mutlak çarpım 1000000000000'dır ve `decimal` türüne sığar.
- Bölmede çok küçük ama sıfır olmayan bir bölen çok büyük bir bölüm üretebilir. Türün kapasitesini aşan bölüm için taşma yakalanır ve sonuç yazılmaz.
- `0`, `0.0` ve `-0.0` aynı sıfır değerini ifade eder ve bölen olarak reddedilir. Sıfırla çarpma veya sıfırı sıfır olmayan sayıya bölme geçerlidir.
- Büyük/küçük harf dönüşümü gerekmez; işlem girdisi tek bir işarettir. Boş satır veya boşluk eklenmiş işaret reddedilir.
- Gösterim yuvarlaması nedeniyle farklı hesaplanan değerler aynı iki basamaklı metni üretebilir.

## Kazanımlar

- Bir işlem seçimini `switch`, `case`, `default` ve `break` ile gerçekleştirme.
- İşleme özel bir ön koşulu, sıfıra bölme örneğinde doğru dalda doğrulama.
- `decimal` girdiyi belirtilen ondalık ayırıcı ve sayı biçimiyle okuma.
- Geçerli girdilerin bile taşma oluşturabileceğini bölme örneğiyle açıklama ve hata sonucunu denetleme.
- Hesaplanan değeri iki basamaklı çıktı gösteriminden ayırt etme.
- Çıkarma ve bölmede operand sırasının sonucunu örneklerle gösterme.

## Alıştırmalar

1. İşlem seçimini `if / else if / else` zinciriyle yeniden yazın ve dört işlem sonuçlarını karşılaştırın.
2. Sonucu önce `F2`, ardından `F4` biçiminde yazdırın; 1/3 girdisinde hesap değişmeden gösterimin nasıl değiştiğini açıklayın.
3. Bölmede sıfır olmayan bölenin mutlak değerinin en az `0.01` olmasını isteyen bir sürüm yazın. Bu yeni kuralın en büyük mutlak bölümü nasıl sınırladığını açıklayın.
